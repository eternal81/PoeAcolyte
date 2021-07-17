using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Helpers;
using PoeAcolyte.Resources;
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
            AddPlayer(entry);
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
            inviteMenuItem.Click += (_, _) => { SetTradeStatus(IPoeTradeControl.TradeStatus.Invited); };
            closeMenuItem.Click += (_, _) => { Dispose(); };
            declineMenuItem.Click += (_, _) => { SetTradeStatus(IPoeTradeControl.TradeStatus.Declined); };
            tradeMenuItem.Click += (_, _) => { SetTradeStatus(IPoeTradeControl.TradeStatus.Traded); };
            tyglMenuItem.Click += (_, _) => { SetTradeStatus(IPoeTradeControl.TradeStatus.ThanksGoodbye); };
            whoIsMenuItem.Click += (_, _) =>
            {
                Commands.SendPoeCommand(IPoeCommands.CommandType.WhoIs, ActiveLogEntry.Player);
            };
            waitMenuItem.Click += (_, _) => { SetTradeStatus(IPoeTradeControl.TradeStatus.AskedToWait); };
        }

        private void SetTradeStatus(IPoeTradeControl.TradeStatus newStatus)
        {
            switch (newStatus)
            {
                case IPoeTradeControl.TradeStatus.None:
                    pbPriceUnit.BackColor = Color.White;
                    UpdateActiveTrade();
                    break;
                case IPoeTradeControl.TradeStatus.AskedToWait:
                    Commands.SendPoeWhisper(ActiveLogEntry.Player,
                        "Busy atm, I will invite as soon as I am back in hideout");
                    ActiveTradeStatus = IPoeTradeControl.TradeStatus.AskedToWait;
                    pbPriceUnit.BackColor = Color.Yellow;
                    waitMenuItem.Text = "♦" + whoIsMenuItem.Text;
                    break;
                case IPoeTradeControl.TradeStatus.Invited:
                    if (ActiveTradeStatus == IPoeTradeControl.TradeStatus.AskedToWait)
                    {
                        // TODO format correct response messages
                        Commands.SendPoeWhisper(ActiveLogEntry.Player, "Okay ready");
                    }

                    Commands.SendPoeCommand(IPoeCommands.CommandType.Invite, ActiveLogEntry.Player);
                    pbPriceUnit.BackColor = Color.Green;
                    inviteMenuItem.Text = "♦" + inviteMenuItem.Text;
                    ActiveTradeStatus = IPoeTradeControl.TradeStatus.Invited;
                    break;
                case IPoeTradeControl.TradeStatus.Traded:
                    Commands.SendPoeCommand(IPoeCommands.CommandType.Trade, ActiveLogEntry.Player);
                    ActiveTradeStatus = IPoeTradeControl.TradeStatus.Traded;
                    tradeMenuItem.Text = "♦" + tradeMenuItem.Text;
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

            ActiveTradeStatus = newStatus;
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

            SetTradeStatus(IPoeTradeControl.TradeStatus.None);
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
            playersMenuItem.DropDownItems.Add(item);

            // Change active trade to whoever is clicked on
            item.Click += (_, _) =>
            {
                SetTradeStatus(IPoeTradeControl.TradeStatus.None);
                UpdateActiveTrade(entry);
            };

            // something at end of trade request (alternative offer)
            if (entry.Other.Length > 0)
            {
                item.DropDownItems.Add(entry.Other);
            }

            _LogEntries.Add(entry);
        }

        private void UpdateActiveTrade()
        {
            // TODO setup default league and compare to incoming trade request
            lblInfo.Text = ActiveLogEntry.ToString();
            pbPriceUnit.Image = Converter.FromPriceString(ActiveLogEntry.PriceUnits);
            lblPriceAmount.Text = ActiveLogEntry.PriceAmount > 0? ActiveLogEntry.PriceAmount.ToString(): "???";
            toolTips.SetToolTip(lblInfo, ActiveLogEntry.Raw);
            
            // reset menu
            playersMenuItem.Text = "Player (reset)";
            waitMenuItem.Text = "Wait";
            inviteMenuItem.Text = "Invite";
            tradeMenuItem.Text = "Trade";
            lblInfo.ContextMenuStrip = contextMenuStrip;
        }

        public void UpdateActiveTrade(PoeLogEntry entry)
        {
            ActiveLogEntry = entry;
            UpdateActiveTrade();
        }

        private void lblInfo_Click(object sender, EventArgs e)
        {
            switch (ActiveTradeStatus)
            {
                case IPoeTradeControl.TradeStatus.None:
                    if (IsBusy) SetTradeStatus(IPoeTradeControl.TradeStatus.AskedToWait);
                    break;
                case IPoeTradeControl.TradeStatus.AskedToWait:
                    if (!IsBusy) SetTradeStatus(IPoeTradeControl.TradeStatus.Invited);
                    break;
                case IPoeTradeControl.TradeStatus.Invited:
                    SetTradeStatus(IPoeTradeControl.TradeStatus.Traded);
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