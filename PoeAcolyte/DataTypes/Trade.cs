using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PoeAcolyte.DataTypes
{
    public abstract class Trade : IDisposable, ITrade
    {
        private readonly Timer _clickTimer = new() {Interval = SystemInformation.DoubleClickTime};
        private readonly List<PoeLogEntry> _logEntries = new();
        private readonly ToolStripMenuItem _playersMenu = new("Players");
        private PoeLogEntry _activeEntry;
        private ITrade.TradeStatus _activeTradeStatus;
        private bool _isBusy;

        protected Trade(PoeLogEntry entry)
        {
            _logEntries.Add(entry);

            _clickTimer.Tick += (sender, args) =>
            {
                _clickTimer.Stop();
                SingleClick().Invoke();
            };
        }

        protected abstract Control StatusControl { get; }
        protected abstract Control IsBusyControl { get; }

        protected Action AskToWait => () =>
        {
            Program.GameBroker.Service.SendCommandToClient(
                $"@{ActiveLogEntry.Player} Busy at the moment, will invite when I am done with what I am doing");
            ActiveTradeStatus = ITrade.TradeStatus.AskedToWait;
        };

        protected virtual Action Invite => () =>
        {
            Program.GameBroker.Service.SendCommandToClient(new[]
            {
                $"@{ActiveLogEntry.Player} Ready for pickup",
                $"/invite {ActiveLogEntry.Player}"
            });

            ActiveTradeStatus = ITrade.TradeStatus.Invited;
            //Program.GameBroker.Service.SendCommandToClient($"/invite {ActiveLogEntry.Player}");
        };

        protected Action Hideout => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"/hideout {ActiveLogEntry.Player}");
        };

        protected Action DoTrade => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"/tradewith {ActiveLogEntry.Player}");
            ActiveTradeStatus = ITrade.TradeStatus.Traded;
        };

        protected Action NoStock => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"@{ActiveLogEntry.Player} Sold out");
            ActiveTradeStatus = ITrade.TradeStatus.OutOfStock;
            RemoveTrade(_activeEntry);
        };

        protected Action Decline => () =>
        {
            Program.GameBroker.Service.SendCommandToClient(new[]
            {
                $"@{ActiveLogEntry.Player} No thanks",
                $"/kick {ActiveLogEntry.Player}"
            });
            ActiveTradeStatus = ITrade.TradeStatus.Declined;
            RemoveTrade(_activeEntry);
        };

        protected Action WhoIs => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"@/whois {ActiveLogEntry.Player}");
        };

        protected Action TradeComplete => () =>
        {
            Program.GameBroker.Service.SendCommandToClient(new[]
            {
                $"@{ActiveLogEntry.Player} Thank you and Good Luck",
                $"/kick {ActiveLogEntry.Player}"
            });
            ActiveTradeStatus = ITrade.TradeStatus.TradeComplete;
            RemoveTrade(_activeEntry);
        };

        protected Action Close => Dispose;

        public virtual void Dispose()
        {
            GetUserControl.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public abstract UserControl GetUserControl { get; }

        public virtual bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                IsBusyControl.BackColor = _isBusy ? Color.Red : Color.Green;
            }
        }

        public event EventHandler Disposed;

        public ITrade.TradeStatus ActiveTradeStatus
        {
            get => _activeTradeStatus;
            set
            {
                _activeTradeStatus = value;
                switch (value)
                {
                    case ITrade.TradeStatus.None:
                        StatusControl.Text = "";
                        StatusControl.ResetBackColor();
                        break;
                    case ITrade.TradeStatus.AskedToWait:
                        StatusControl.Text = "Asked to wait";
                        StatusControl.BackColor = Color.Cyan;
                        break;
                    case ITrade.TradeStatus.Invited:
                        StatusControl.Text = "Invited";
                        StatusControl.BackColor = Color.GreenYellow;
                        break;
                    case ITrade.TradeStatus.Traded:
                        StatusControl.Text = "Traded";
                        StatusControl.BackColor = Color.DodgerBlue;
                        break;
                    default:
                        StatusControl.Text = "";
                        StatusControl.ResetBackColor();
                        break;
                }
            }
        }

        public IEnumerable<string> Players => _logEntries.Select(o => o.Player).Distinct();

        public PoeLogEntry ActiveLogEntry
        {
            get => _activeEntry;
            set
            {
                _activeEntry = value;
                UpdateControls();
            }
        }

        public abstract bool TakeLogEntry(PoeLogEntry entry);

        public abstract void UpdateControls();

        protected void BindClickControl()
        {
            // TODO make an extension or roll into userform controls?
            foreach (Control control in GetUserControl.Controls)
            {
                control.Click += (sender, args) => { _clickTimer.Start(); };
                control.DoubleClick += (sender, args) =>
                {
                    _clickTimer.Stop();
                    DoubleClick().Invoke();
                };
            }
        }

        protected void BuildContextMenuStrip()
        {
            var menuItems = GetUserControl.ContextMenuStrip.Items;
            menuItems.Clear();
            menuItems.Add(MakeMenuItem("No Thanks", Decline));
            menuItems.Add(MakeMenuItem("Wait", AskToWait, true));
            menuItems.Add(MakeMenuItem("Invite", Invite, true));
            menuItems.Add(MakeMenuItem("Hideout", Hideout));
            menuItems.Add(MakeMenuItem("Trade", DoTrade, true));
            menuItems.Add(MakeMenuItem("TY GL", TradeComplete));
            menuItems.Add(MakeMenuItem("Out of stock", NoStock));
            menuItems.Add(MakeMenuItem("WhoIs", WhoIs));
            menuItems.Add(MakeMenuItem("Close", Close));
            menuItems.Add(_playersMenu);
            _playersMenu.DropDownItems.Add(AddPlayerToMenu(_activeEntry, logEntry => ActiveLogEntry = logEntry));
        }

        private void RemoveTrade(PoeLogEntry entry, bool bRemoveAll = false)
        {
            _logEntries.Remove(entry);
            var remainingTrades = _logEntries.Where(logEntry =>
                logEntry.PoeLogEntryType != IPoeLogEntry.PoeLogEntryTypeEnum.Whisper).ToArray();

            if (!remainingTrades.Any() || bRemoveAll)
            {
                Dispose();
                return;
            }

            // TODO causing program to hang
            var t = _playersMenu.DropDownItems.Find(entry.Player, false);
            if (t.Any())
                _playersMenu.DropDownItems.Remove(t.First());
            _activeEntry = remainingTrades.First();
            UpdateControls();
        }

        protected bool CheckWhisper(PoeLogEntry entry)
        {
            if (entry.PoeLogEntryType != IPoeLogEntry.PoeLogEntryTypeEnum.Whisper ||
                !Players.Contains(entry.Player)) return false;

            var match = _playersMenu.DropDownItems.Find(entry.Player, false);
            if (!match.Any()) return false;

            var menuItem = (ToolStripMenuItem) match.First();
            menuItem.DropDownItems.Add(entry.Other);
            _logEntries.Add(entry);
            return true;
        }

        protected virtual Action SingleClick()
        {
            return ActiveTradeStatus switch
            {
                ITrade.TradeStatus.None => AskToWait,
                ITrade.TradeStatus.AskedToWait => Invite,
                ITrade.TradeStatus.Invited => DoTrade,
                _ => DoTrade
            };
        }


        protected Action DoubleClick()
        {
            return ActiveTradeStatus switch
            {
                ITrade.TradeStatus.None => Invite,
                ITrade.TradeStatus.Invited => DoTrade,
                ITrade.TradeStatus.Traded => TradeComplete,
                _ => DoTrade
            };
        }


        /// <summary>
        ///     Generic context menu add
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <param name="checkOnClick"></param>
        /// <returns></returns>
        protected static ToolStripMenuItem MakeMenuItem(string text, Action action, bool checkOnClick = false)
        {
            var ret = new ToolStripMenuItem(text) {Name = text};
            ret.Click += (sender, args) =>
            {
                ret.Checked = checkOnClick;
                action();
            };
            return ret;
        }

        /// <summary>
        ///     Used to make player entries for context menu
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="action"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        protected static ToolStripMenuItem AddPlayerToMenu(PoeLogEntry entry, Action<PoeLogEntry> action,
            string suffix = "")
        {
            var ret = new ToolStripMenuItem(entry.Player + suffix) {Name = entry.Player};
            ret.Click += (sender, args) => { action(entry); };
            return ret;
        }

        protected void AddPlayer(PoeLogEntry entry, string suffix = "")
        {
            _logEntries.Add(entry);
            _playersMenu.DropDownItems.Add(AddPlayerToMenu(entry, logEntry => ActiveLogEntry = logEntry, suffix));
            // TODO add any suffix (other) information in the trade request here?
        }
    }
}