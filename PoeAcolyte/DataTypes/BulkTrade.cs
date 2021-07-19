using System;
using System.Windows.Forms;
using PoeAcolyte.UI;

namespace PoeAcolyte.DataTypes
{
    public class BulkTrade : Trade, IPoeTradeControl
    {
        private readonly BulkTradeControl _bulkTradeControl;
        public override UserControl GetUserControl => _bulkTradeControl;
        public sealed override PoeLogEntry ActiveLogEntry
        {
            get => ActiveEntry;
            set
            {
                ActiveEntry = value;
                _bulkTradeControl.UpdateControls(ActiveEntry);
            }
        }

        public BulkTrade(PoeLogEntry entry) : base(entry)
        {
            _bulkTradeControl = new BulkTradeControl();
            ActiveLogEntry = entry;
            SetContext();
        }

        private void SetContext()
        {
            _bulkTradeControl.ContextMenuStrip.Items.Clear();
            _bulkTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("No Thanks", Decline));
            _bulkTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Invite", Invite, true));
            _bulkTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Trade", DoTrade, true));
            _bulkTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Out of stock", NoStock));
            _bulkTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Close", Close));
            _bulkTradeControl.ContextMenuStrip.Items.Add(PlayersMenu);
            PlayersMenu.DropDownItems.Add(AddPlayerToMenu(ActiveEntry, SetActiveEntry));
        }
        
        public void AddLogEntry(PoeLogEntry entry)
        {
            throw new NotImplementedException();
        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (CheckWhisper(entry)) return true;
            
            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.BuyPriceUnits == ActiveLogEntry.BuyPriceUnits && entry.PriceUnits == ActiveLogEntry.PriceUnits)
            {
                LogEntries.Add(entry);
                var suffix = $"{entry.Player}  {entry.PriceAmount} {entry.PriceUnits} ► {entry.BuyPriceAmount} {entry.BuyPriceUnits}";
                PlayersMenu.DropDownItems.Add(AddPlayerToMenu(entry, SetActiveEntry, suffix));
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