using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PoeAcolyte.DataTypes
{
    public abstract class Trade : IDisposable, ITrade
    {
        protected abstract Control StatusControl { get; }
        protected abstract ToolStripItemCollection MenuItems { get; }
        public abstract UserControl GetUserControl { get; }
        public event EventHandler Disposed;
        private ITrade.TradeStatus _activeTradeStatus;
        protected readonly ToolStripMenuItem _PlayersMenu = new("Players");
        private PoeLogEntry _activeEntry;
        protected readonly List<PoeLogEntry> _LogEntries = new();
        public bool IsBusy { get; set; }

        private ITrade.TradeStatus ActiveTradeStatus
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

        public IEnumerable<string> Players => _LogEntries.Select(o => o.Player).Distinct();

        public PoeLogEntry ActiveLogEntry
        {
            get => _activeEntry;
            set
            {
                _activeEntry = value;
                UpdateControls();
            }
        }

        protected Trade(PoeLogEntry entry)
        {
            _LogEntries.Add(entry);
        }

        public abstract void UpdateControls();

        public abstract bool TakeLogEntry(PoeLogEntry entry);

        public abstract void Dispose();

        protected void BuildContextMenuStrip()
        {
            MenuItems.Clear();
            MenuItems.Add(MakeMenuItem("No Thanks", Decline));
            MenuItems.Add(MakeMenuItem("Wait", AskToWait, true));
            MenuItems.Add(MakeMenuItem("Invite", Invite, true));
            MenuItems.Add(MakeMenuItem("Trade", DoTrade, true));
            MenuItems.Add(MakeMenuItem("Out of stock", NoStock));
            MenuItems.Add(MakeMenuItem("WhoIs", WhoIs));
            MenuItems.Add(MakeMenuItem("Close", Close));
            MenuItems.Add(_PlayersMenu);
            _PlayersMenu.DropDownItems.Add(AddPlayerToMenu(_activeEntry, SetActiveEntry));
        }

        private void RemoveTrade(PoeLogEntry entry, bool bRemoveAll = false)
        {
            _LogEntries.Remove(entry);
            var remainingTrades = _LogEntries.Where(logEntry =>
                logEntry.PoeLogEntryType != IPoeLogEntry.PoeLogEntryTypeEnum.Whisper).ToArray();
            if (remainingTrades.Any() && !bRemoveAll)
            {
                _PlayersMenu.DropDownItems.RemoveByKey(entry.Player);
                _activeEntry = remainingTrades.First();
                UpdateControls();
            }
            else
                Dispose();
        }

        protected bool CheckWhisper(PoeLogEntry entry)
        {
            if (entry.PoeLogEntryType != IPoeLogEntry.PoeLogEntryTypeEnum.Whisper ||
                !Players.Contains(entry.Player)) return false;

            var match = _PlayersMenu.DropDownItems.Find(entry.Player, false);
            if (!match.Any()) return false;

            var menuItem = (ToolStripMenuItem) match.First();
            menuItem.DropDownItems.Add(entry.Other);
            _LogEntries.Add(entry);
            return true;
        }

        protected Action AskToWait => () =>
        {
            // TODO add area search pattern with average time between map or specific zones
            // (i.e. avg time to kill shaper) minus time currently spent in zone
            Program.GameBroker.Service.SendCommandToClient(
                $"@{ActiveLogEntry.Player} Busy at the moment, will invite when I am done with what I am doing");
            ActiveTradeStatus = ITrade.TradeStatus.AskedToWait;
        };

        protected Action Invite => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"/invite {ActiveLogEntry.Player}");
            Program.GameBroker.Service.SendCommandToClient($"@{ActiveLogEntry.Player} Ready for pickup");
            ActiveTradeStatus = ITrade.TradeStatus.Invited;
        };

        protected Action DoTrade => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"/tradewith {ActiveLogEntry.Player}");
            ActiveTradeStatus = ITrade.TradeStatus.Traded;
        };

        protected Action NoStock => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"@{ActiveLogEntry.Player} Sold out");
            RemoveTrade(_activeEntry);
        };

        protected Action Decline => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"@{ActiveLogEntry.Player} No thanks");
            Program.GameBroker.Service.SendCommandToClient($"/kick {ActiveLogEntry.Player}");
            RemoveTrade(_activeEntry);
        };

        protected Action WhoIs => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"@/whois {ActiveLogEntry.Player}");
        };

        protected Action Close => Dispose;

        protected Action<PoeLogEntry> SetActiveEntry => delegate(PoeLogEntry entry)
        {
            ActiveLogEntry = entry;
            // TODO do we need to update controls or leave it to the property?
        };


        /// <summary>
        /// Generic context menu add
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <param name="checkOnClick"></param>
        /// <returns></returns>
        private static ToolStripMenuItem MakeMenuItem(string text, Action action, bool checkOnClick = false)
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
        /// Used to make player entries for context menu
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="action"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        protected static ToolStripMenuItem AddPlayerToMenu(PoeLogEntry entry, Action<PoeLogEntry> action,
            string suffix = "")
        {
            // TODO refactor this to just inline the action or just put active entry in the ret.click?
            var ret = new ToolStripMenuItem(entry.Player + suffix) {Name = entry.Player};
            ret.Click += (sender, args) => { action(entry); };
            return ret;
        }
    }
}