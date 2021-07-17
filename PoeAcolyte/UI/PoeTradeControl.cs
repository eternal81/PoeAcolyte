using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Helpers;
using PoeAcolyte.Service;

namespace PoeAcolyte.UI
{
    public partial class PoeTradeControl : UserControl, IPoeTradeControl, IPoeTradeActions
    {
        private readonly List<PoeLogEntry> _LogEntries = new();

        private readonly TradeContextMenu _tradeContextMenu;
        
        protected PoeSettings Settings = PoeSettings.Load();

        private PoeTradeControl()
        {
            InitializeComponent();
            //HandleContextMenu();
        }

        public PoeTradeControl(PoeLogEntry entry, IPoeCommands commands) : this()
        {
            Commands = commands;
            _tradeContextMenu = new TradeContextMenu(this);
            lblInfo.BackColor = entry.Incoming ? Color.Yellow : Color.White;
            lblInfo.ContextMenuStrip = _tradeContextMenu.ContextMenuStrip;
            AddLogEntry(entry);
            UpdateActiveTrade(entry);
        }

        public void AddLogEntry(PoeLogEntry entry)
        {
            if (entry.PoeLogEntryType == IPoeLogEntry.PoeLogEntryTypeEnum.Whisper)
            {
                _tradeContextMenu.addWhisper(entry);
            }
            else
            {
                _tradeContextMenu.AddPlayer(entry);
                _LogEntries.Add(entry);
            }
        }

        public void UpdateActiveTrade(PoeLogEntry entry)
        {
            ActiveLogEntry = entry;
            UpdateActiveTrade();
        }

        public void RemoveTrade(PoeLogEntry entry, bool bRemoveAll = false)
        {
            _LogEntries.Remove(entry);
            if (_LogEntries.Any() && !bRemoveAll)
                Reset(_LogEntries[0].Player);
            else
                Dispose();
        }

        private void UpdateActiveTrade()
        {
            // TODO setup default league and compare to incoming trade request
            lblInfo.Text = ActiveLogEntry.ToString();
            pbPriceUnit.Image = Converter.FromPriceString(ActiveLogEntry.PriceUnits);
            lblPriceAmount.Text = ActiveLogEntry.PriceAmount > 0 ? ActiveLogEntry.PriceAmount.ToString() : "???";
            toolTips.SetToolTip(lblInfo, ActiveLogEntry.Raw);

            //_tradeContextMenu.Refresh();
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {
            switch (ActiveTradeStatus)
            {
                case IPoeTradeControl.TradeStatus.None:
                    if (IsBusy) AskToWait();
                    break;
                case IPoeTradeControl.TradeStatus.AskedToWait:
                    if (!IsBusy) Invite();
                    break;
                case IPoeTradeControl.TradeStatus.Invited:
                    Trade();
                    break;
                // ///////////////
                // Need a better way of determining if trade accepted is for this one
                // case IPoeTradeControl.TradeStatus.Traded:
                //     // TODO check if trade accepted
                //     RunPoeEvent(IPoeTradeControl.TradeStatus.ThanksGoodbye);
                //     break;
            }
        }

        #region Members IPoeTradeControl

        public IPoeTradeControl.TradeStatus ActiveTradeStatus { get; set; }
        public bool IsBusy { get; set; } = true;
        public UserControl GetUserControl => this;
        public IPoeCommands Commands { get; set; }
        public PoeLogEntry ActiveLogEntry { get; set; }

        public IEnumerable<string> Players => _LogEntries.Select(o => o.Player).Distinct();

        #endregion

        #region Actions IPoeTradeActions

        public Action Invite => () =>
        {
            if (ActiveTradeStatus == IPoeTradeControl.TradeStatus.AskedToWait)
                Commands.SendPoeWhisper(ActiveLogEntry.Player, "Okay ready");

            Commands.SendPoeCommand(IPoeCommands.CommandType.Invite, ActiveLogEntry.Player);
            pbPriceUnit.BackColor = Color.Green;

            ActiveTradeStatus = IPoeTradeControl.TradeStatus.Invited;
        };

        public Action<string> Reset => playerName =>
        {
            if (playerName is null) Program.Log.Information("Tried to reset action without player name");
            pbPriceUnit.BackColor = Color.White;

            var entry = _LogEntries.Find(logEntry => logEntry.Player == playerName);
            if (entry is not null) UpdateActiveTrade(entry);
            _tradeContextMenu.Refresh();
        };

        public Action AskToWait => () =>
        {
            Commands.SendPoeWhisper(ActiveLogEntry.Player,
                "Busy atm, I will invite as soon as I am back in hideout");
            ActiveTradeStatus = IPoeTradeControl.TradeStatus.AskedToWait;
            pbPriceUnit.BackColor = Color.Yellow;
        };

        public Action Trade => () =>
        {
            Commands.SendPoeCommand(IPoeCommands.CommandType.Trade, ActiveLogEntry.Player);
            ActiveTradeStatus = IPoeTradeControl.TradeStatus.Traded;
            pbPriceUnit.BackColor = Color.Blue;
        };

        public Action Decline => () =>
        {
            Commands.SendPoeWhisper(ActiveLogEntry.Player, "No Thanks");
            RemoveTrade(ActiveLogEntry);
        };

        public Action ThanksGoodbye => () =>
        {
            Commands.SendPoeWhisper(ActiveLogEntry.Player, "Thank you and GL");
            RemoveTrade(ActiveLogEntry);
        };

        public Action WhoIs =>
            () => { Commands.SendPoeCommand(IPoeCommands.CommandType.WhoIs, ActiveLogEntry.Player); };

        public Action Close => Dispose;

        #endregion
    }
}