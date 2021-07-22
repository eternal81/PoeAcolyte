using System.Drawing;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Helpers;
using PoeAcolyte.UI;

namespace PoeAcolyte.Trades
{
    public class BulkTrade : Trade
    {
        protected override Control StatusControl => _bulkTradeControl.lblStatus;

        public override UserControl GetUserControl => _bulkTradeControl;
        
        private readonly BulkTradeControl _bulkTradeControl;
        protected override Control IsBusyControl => _bulkTradeControl.pbTradeDirection;
        public BulkTrade(PoeLogEntry entry) : base(entry)
        {
            _bulkTradeControl = new BulkTradeControl();

            ActiveLogEntry = entry;

            BindClickControl();
            BuildContextMenuStrip();
            CheckWhisper(entry);
        }

        protected override void UpdateControls()
        {
            _bulkTradeControl.pbBuyUnit.Image = PoeConverter.FromPriceString(ActiveLogEntry.BuyPriceUnits);
            _bulkTradeControl.pbPriceUnit.Image = PoeConverter.FromPriceString(ActiveLogEntry.PriceUnits);
            _bulkTradeControl.lblInfo.Text = ActiveLogEntry.ToString();
            _bulkTradeControl.lblBuyAmount.Text = ActiveLogEntry.BuyPriceAmount.ToString();
            _bulkTradeControl.lblPriceAmount.Text = ActiveLogEntry.PriceAmount.ToString();
            _bulkTradeControl.toolTips.SetToolTip(_bulkTradeControl.lblInfo, ActiveLogEntry.Raw);
            _bulkTradeControl.pbTradeDirection.Image = ActiveLogEntry.Incoming ? Resources.Region.UpArrow : Resources.Region.DownArrow;
            _bulkTradeControl.BackColor = ActiveLogEntry.Incoming ? Color.Bisque : Color.LightBlue;
            _bulkTradeControl.ContextMenuStrip = _bulkTradeControl.contextMenu;
        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (CheckWhisper(entry)) return true;

            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.BuyPriceUnits == ActiveLogEntry.BuyPriceUnits && entry.PriceUnits == ActiveLogEntry.PriceUnits)
            {
                var suffix =
                    $" {entry.PriceAmount} {entry.PriceUnits} ► {entry.BuyPriceAmount} {entry.BuyPriceUnits}";
                AddPlayer(entry, suffix);
                return true;
            }

            return false;
        }


    }
}