using System;
using System.Drawing;
using System.Windows.Forms;
using PoeAcolyte.Helpers;
using PoeAcolyte.UI;

namespace PoeAcolyte.DataTypes
{
    public class SingleTrade : Trade
    {
        protected override Control StatusControl => _singleTradeControl.lblStatus;

        private readonly SingleTradeControl _singleTradeControl;
        public override UserControl GetUserControl => _singleTradeControl;

        public SingleTrade(PoeLogEntry entry) : base(entry)
        {
            _singleTradeControl = new SingleTradeControl();
            ActiveLogEntry = entry;
            BindClickControl();
            BuildContextMenuStrip();
        }
        
        public override void UpdateControls()
        {
            _singleTradeControl.pbPriceUnit.Image = Converter.FromPriceString(ActiveLogEntry.PriceUnits);
            _singleTradeControl.lblInfo.Text = ActiveLogEntry.ToString();
            _singleTradeControl.lblPriceAmount.Text = ActiveLogEntry.PriceAmount.ToString();
            _singleTradeControl.toolTips.SetToolTip(_singleTradeControl.lblInfo, ActiveLogEntry.Raw);
            _singleTradeControl.pbTradeDirection.Image = ActiveLogEntry.Incoming ? Resources.Region.UpArrow : Resources.Region.DownArrow;
            _singleTradeControl.BackColor = ActiveLogEntry.Incoming ? Color.Bisque : Color.LightBlue;
            _singleTradeControl.ContextMenuStrip = _singleTradeControl.contextMenu;
        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (CheckWhisper(entry)) return true;
            
            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.Item == ActiveLogEntry.Item && entry.Top == ActiveLogEntry.Top && entry.Left == ActiveLogEntry.Left )
            {
                AddPlayer(entry);
                return true;
            }

            return false;
        }

    }
}