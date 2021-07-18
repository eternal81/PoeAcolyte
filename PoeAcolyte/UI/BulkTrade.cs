using System;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;

namespace PoeAcolyte.UI
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
            _bulkTradeControl.Contextmenu.Items.Add(MakeMenuItem("No Thanks", Decline));
            _bulkTradeControl.Contextmenu.Items.Add(MakeMenuItem("Invite", Invite, true));
            _bulkTradeControl.Contextmenu.Items.Add(MakeMenuItem("Trade", DoTrade, true));
            _bulkTradeControl.Contextmenu.Items.Add(MakeMenuItem("Out of stock", NoStock));
            _bulkTradeControl.Contextmenu.Items.Add(MakeMenuItem("Close", Close));
            _bulkTradeControl.Contextmenu.Items.Add(PlayersMenu);
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
                PlayersMenu.DropDownItems.Add(MakeMenuItem(entry, logEntry => { ActiveLogEntry = logEntry; }));
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