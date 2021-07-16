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
    public partial class PoeTradeControl : UserControl, IPoeTradeControl
    {
        public IPoeTradeControl.TradeStatus ActiveTradeStatus { get; set; }
        public bool IsBusy { get; set; } = true;
        public UserControl GetUserControl => this;
        public IPoeCommands Commands { get; set; }
        public PoeLogEntry ActiveLogEntry { get; set; }
        private List<PoeLogEntry> _LogEntries = new();

        public IEnumerable<string> Players => _LogEntries.Select(o => o.Player).Distinct();

        //protected CellHighlight _cell;
        protected PoeSettings Settings = PoeSettings.Load();

        private PoeTradeControl()
        {
            InitializeComponent();
            HandleContextMenu();
        }

        public PoeTradeControl(PoeLogEntry entry, IPoeCommands commands) : this()
        {
            Commands = commands;
            UpdateActiveTrade(entry);
        }

        private void pbPriceUnit_Click(object sender, EventArgs e)
        {
            //ShowCellHighlight?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Designer does not play friendly with simple bubble up invocations 
        /// </summary>
        private void HandleContextMenu()
        {
            inviteMenuItem.Click += (_, _) => { RunPoeEvent(IPoeTradeControl.TradeStatus.Invited); };
            closeMenuItem.Click += (_, _) => { Dispose(); };
            declineMenuItem.Click += (_, _) => { RunPoeEvent(IPoeTradeControl.TradeStatus.Declined); };
            tradeMenuItem.Click += (_, _) => { RunPoeEvent(IPoeTradeControl.TradeStatus.Traded); };
            tyglMenuItem.Click += (_, _) => { RunPoeEvent(IPoeTradeControl.TradeStatus.ThanksGoodbye); };
        }

        private void RunPoeEvent(IPoeTradeControl.TradeStatus newStatus)
        {
            switch (newStatus)
            {
                case IPoeTradeControl.TradeStatus.None:
                    pbPriceUnit.BackColor = Color.White;
                    break;
                case IPoeTradeControl.TradeStatus.AskedToWait:
                    Commands.SendPoeWhisper(ActiveLogEntry.Player,
                        "Busy atm, I will invite as soon as I am back in hideout");
                    ActiveTradeStatus = IPoeTradeControl.TradeStatus.AskedToWait;
                    pbPriceUnit.BackColor = Color.Yellow;
                    break;
                case IPoeTradeControl.TradeStatus.Invited:
                    if (ActiveTradeStatus == IPoeTradeControl.TradeStatus.AskedToWait)
                    {
                        Commands.SendPoeWhisper(ActiveLogEntry.Player, "Okay ready");
                    }

                    Commands.SendPoeCommand(IPoeCommands.CommandType.Invite, ActiveLogEntry.Player);
                    pbPriceUnit.BackColor = Color.Green;
                    ActiveTradeStatus = IPoeTradeControl.TradeStatus.Invited;
                    break;
                case IPoeTradeControl.TradeStatus.Traded:
                    Commands.SendPoeCommand(IPoeCommands.CommandType.Trade, ActiveLogEntry.Player);
                    ActiveTradeStatus = IPoeTradeControl.TradeStatus.Traded;
                    pbPriceUnit.BackColor = Color.Blue;
                    break;
                case IPoeTradeControl.TradeStatus.Declined:
                    Commands.SendPoeWhisper(ActiveLogEntry.Player, "No Thanks");
                    RemoveTrade(ActiveLogEntry);
                    break;
                case IPoeTradeControl.TradeStatus.ThanksGoodbye:
                    Commands.SendPoeWhisper(ActiveLogEntry.Player, "Thank you and GL");
                    RemoveTrade(ActiveLogEntry);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
            }
        }

        public void RemoveTrade(PoeLogEntry entry)
        {
            _LogEntries.Remove(entry);
            if (_LogEntries.Any())
            {
                UpdateActiveTrade(_LogEntries[0]);
            }
            else
            {
                Dispose();
            }

            RunPoeEvent(IPoeTradeControl.TradeStatus.None);
        }

        public void AddWhisper(PoeLogEntry entry)
        {
            var toolStripItems = playersMenuItem.DropDownItems.Find(entry.Player, false);
            if (toolStripItems.Any())
            {
                //var newitem = ((ToolStripMenuItem) toolStripItems[0]);
                ((ToolStripMenuItem) toolStripItems[0]).DropDownItems.Add(entry.Other);
            }
        }

        public void AddPlayer(PoeLogEntry entry)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(entry.Player) {Name = entry.Player};
            playersMenuItem.DropDownItems.Add(item); // TODO add click functionality
            // something at end of trade request
            if (entry.Other.Length > 0)
            {
                item.DropDownItems.Add(entry.Other);
            }

            _LogEntries.Add(entry);
        }

        private void UpdateActiveTrade()
        {
            // TODO setup default league and compare to incoming trade request
            if (ActiveLogEntry.PoeLogEntryType == IPoeLogEntry.PoeLogEntryTypeEnum.BulkTrade)
            {
                lblInfo.Text = ActiveLogEntry.Player + " (" + ActiveLogEntry.PoeLogEntryType + ")\r\n" +
                               ActiveLogEntry.BuyPriceAmount + " " + ActiveLogEntry.BuyPriceUnits + "\r\n";

                lblPriceAmount.Text = ActiveLogEntry.PriceAmount.ToString();
                //pbPriceUnit.BackgroundImage = CurrencyImages.CurrencyRerollRare; // TODO add currency lookups
            }
            else if (ActiveLogEntry.PoeLogEntryType == IPoeLogEntry.PoeLogEntryTypeEnum.PricedTrade)
            {
                lblInfo.Text = ActiveLogEntry.Player + " (" + ActiveLogEntry.PoeLogEntryType + ")\r\n" +
                               ActiveLogEntry.Item + "\r\n" +
                               " (" + ActiveLogEntry.StashTab + ")" + "\r\n" +
                               "Top: " + ActiveLogEntry.Top + ", Left: " + ActiveLogEntry.Left;
                lblPriceAmount.Text = ActiveLogEntry.PriceAmount.ToString();
            }
            else if (ActiveLogEntry.PoeLogEntryType == IPoeLogEntry.PoeLogEntryTypeEnum.UnpricedTrade)
            {
                lblInfo.Text = ActiveLogEntry.Player + " (" + ActiveLogEntry.PoeLogEntryType + ")\r\n" +
                               ActiveLogEntry.Item + "\r\n";
                lblPriceAmount.Text = "???";
            }

            toolTips.SetToolTip(lblInfo, ActiveLogEntry.League);
            playersMenuItem.Text = ActiveLogEntry.Player;
            lblInfo.ContextMenuStrip = contextMenuStrip;
        }

        public void UpdateActiveTrade(PoeLogEntry entry)
        {
            // TODO add back tool strip info
            ActiveLogEntry = entry;
            UpdateActiveTrade();
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {
            switch (ActiveTradeStatus)
            {
                case IPoeTradeControl.TradeStatus.None:
                    if (IsBusy) RunPoeEvent(IPoeTradeControl.TradeStatus.AskedToWait);
                    break;
                case IPoeTradeControl.TradeStatus.AskedToWait:
                    if (!IsBusy) RunPoeEvent(IPoeTradeControl.TradeStatus.Invited);
                    break;
                case IPoeTradeControl.TradeStatus.Invited:
                    RunPoeEvent(IPoeTradeControl.TradeStatus.Traded);
                    break;
                // ///////////////
                // Need a better way of determining if trade accepted is for this one
                // case IPoeTradeControl.TradeStatus.Traded:
                //     // TODO check if trade accepted
                //     RunPoeEvent(IPoeTradeControl.TradeStatus.ThanksGoodbye);
                //     break;
                default:
                    break;
            }
        }
    }
}