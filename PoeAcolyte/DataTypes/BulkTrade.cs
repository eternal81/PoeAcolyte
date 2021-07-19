using System;
using System.Windows.Forms;
using PoeAcolyte.UI;

namespace PoeAcolyte.DataTypes
{
    public class BulkTrade : Trade
    {
        protected override Control StatusControl => _bulkTradeControl.lblStatus;
        protected override ToolStripItemCollection MenuItems => _bulkTradeControl.ContextMenuStrip.Items;
        public override UserControl GetUserControl => _bulkTradeControl;
        private readonly BulkTradeControl _bulkTradeControl;

        public BulkTrade(PoeLogEntry entry) : base(entry)
        {
            _bulkTradeControl = new BulkTradeControl();
            
            ActiveLogEntry = entry;
            BuildContextMenuStrip();
        }

        public override void UpdateControls()
        {
            _bulkTradeControl.UpdateControls(ActiveLogEntry);
        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (CheckWhisper(entry)) return true;
            
            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.BuyPriceUnits == ActiveLogEntry.BuyPriceUnits && entry.PriceUnits == ActiveLogEntry.PriceUnits)
            {
                _LogEntries.Add(entry);
                var suffix = $"{entry.Player}  {entry.PriceAmount} {entry.PriceUnits} ► {entry.BuyPriceAmount} {entry.BuyPriceUnits}";
                _PlayersMenu.DropDownItems.Add(AddPlayerToMenu(entry, SetActiveEntry, suffix));
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
            _bulkTradeControl.Dispose();
            
        }
        
    }
}