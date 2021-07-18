using System;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.UI
{
    public class BulkTrade : Trade, IPoeTradeControl
    {
        private readonly BulkTradeControl _bulkTradeControl;
        public UserControl GetUserControl => _bulkTradeControl;
        public override PoeLogEntry ActiveLogEntry
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
            _bulkTradeControl.UpdateControls(entry);
        }

        public void AddLogEntry(PoeLogEntry entry)
        {
            throw new NotImplementedException();
        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (entry.PoeLogEntryType == IPoeLogEntry.PoeLogEntryTypeEnum.Whisper && Players.Contains(entry.Player))
            {
                LogEntries.Add(entry);
                return true;
            }

            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.BuyPriceUnits == ActiveLogEntry.PriceUnits)
            {
                LogEntries.Add(entry);
                return true;
            }

            return false;
        }
    }
}