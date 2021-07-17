using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;

namespace PoeAcolyte.UI
{
    public interface IPoeTradeControl
    {
        public enum TradeStatus
        {
            None,
            AskedToWait,
            Invited,
            Traded
        }
        public bool IsBusy { get; set; }
        public TradeStatus ActiveTradeStatus { get; set; }
        public void AddLogEntry(PoeLogEntry entry);

        public void UpdateActiveTrade(PoeLogEntry entry);
        /// <summary>
        /// Needed for updating control collections it is added to / removed from
        /// </summary>
        public UserControl GetUserControl { get; }

        public IEnumerable<string> Players { get; }
        public PoeLogEntry ActiveLogEntry { get; }
        public IPoeCommands Commands { get; set; }
        public event EventHandler Disposed;
    }
}