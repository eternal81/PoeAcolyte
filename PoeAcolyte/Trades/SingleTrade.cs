using System;
using System.Drawing;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Helpers;
using PoeAcolyte.UI;

namespace PoeAcolyte.Trades
{
    public class SingleTrade : Trade
    {
        protected override Control StatusControl => _singleTradeControl.lblStatus;

        private readonly SingleTradeControl _singleTradeControl;
        public override UserControl GetUserControl => _singleTradeControl;

        protected CellHighlight _CellHighlight;
        protected override Control IsBusyControl => _singleTradeControl.pbTradeDirection;


        public SingleTrade(PoeLogEntry entry) : base(entry)
        {
            _singleTradeControl = new SingleTradeControl();
            ActiveLogEntry = entry;
            BindClickControl();
            BuildContextMenuStrip();
            CheckWhisper(entry);
            // add single trade only context menu items
            var menuItems = _singleTradeControl.ContextMenuStrip.Items;
            menuItems.Add(MakeMenuItem("Show Cell", ShowCellHighlight));
            
            _CellHighlight ??= new CellHighlight(ActiveLogEntry);
            _CellHighlight.Visible = false;
        }

        protected override void UpdateControls()
        {
            _singleTradeControl.pbPriceUnit.Image = PoeConverter.FromPriceString(ActiveLogEntry.PriceUnits);
            _singleTradeControl.lblInfo.Text = ActiveLogEntry.ToString();
            _singleTradeControl.lblPriceAmount.Text = ActiveLogEntry.PriceAmount.ToString();
            _singleTradeControl.toolTips.SetToolTip(_singleTradeControl.lblInfo, ActiveLogEntry.Raw);
            _singleTradeControl.pbTradeDirection.Image =
                ActiveLogEntry.Incoming ? Resources.Region.UpArrow : Resources.Region.DownArrow;
            _singleTradeControl.BackColor = ActiveLogEntry.Incoming ? Color.Bisque : Color.LightBlue;
            _singleTradeControl.ContextMenuStrip = _singleTradeControl.contextMenu;
        }

        /// <summary>
        /// overriding default action if in hideout (not busy)
        /// </summary>
        /// <returns></returns>
        protected override Action SingleClick()
        {
            if (IsBusy)
            {
                return ActiveTradeStatus switch
                {
                    ITrade.TradeStatus.None => AskToWait,
                    ITrade.TradeStatus.AskedToWait => Invite,
                    ITrade.TradeStatus.Invited => DoTrade,
                    _ => DoTrade
                };
            }

            return ActiveTradeStatus switch
            {
                ITrade.TradeStatus.None => Invite,
                ITrade.TradeStatus.AskedToWait => Invite,
                ITrade.TradeStatus.Invited => DoTrade,
                ITrade.TradeStatus.Traded => TradeComplete,
                _ => DoTrade
            };
        }

        protected override Action Invite => () =>
        {
            base.Invite();
            ShowCellHighlight();
        };

        protected Action ShowCellHighlight => () => { _CellHighlight.Visible = true; };

        public override bool TakeMouseClick(MouseEventArgs e)
        {
            if (_CellHighlight is not null && _CellHighlight.Visible && new Rectangle(_CellHighlight.Location, _CellHighlight.Size).Contains(e.Location))
            {
                _CellHighlight.Visible = false;
                return true;
            } 
            return false;

        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (CheckWhisper(entry)) return true;

            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.Item == ActiveLogEntry.Item && entry.Top == ActiveLogEntry.Top &&
                entry.Left == ActiveLogEntry.Left)
            {
                AddPlayer(entry);
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
            _CellHighlight?.Dispose();
            base.Dispose();
        }
    }
}