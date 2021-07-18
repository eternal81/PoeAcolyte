using System;
using System.Collections.Generic;
using System.Linq;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;

namespace PoeAcolyte.UI
{
    public class Trade
    {
        protected PoeLogEntry ActiveEntry;
        protected readonly IPoeCommands PoeCommands;
        protected readonly List<PoeLogEntry> LogEntries = new();

        public bool IsBusy { get; set; }
        public IPoeTradeControl.TradeStatus ActiveTradeStatus { get; set; }
        public IEnumerable<string> Players => LogEntries.Select(o => o.Player).Distinct();
        public virtual PoeLogEntry ActiveLogEntry { get; set; }
        public IPoeCommands Commands { get; set; }
        public event EventHandler Disposed;

        public Trade(PoeLogEntry entry)
        {
            ActiveEntry = entry;
            
        }

        public virtual bool TakeLogEntry(PoeLogEntry entry)
        {
            return false;
            
        }
    }
}