using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Trades
{
    public interface ITrade : IDisposable
    {
        public enum TradeStatus
        {
            None,
            AskedToWait,
            Invited,
            Traded, 
            TradeComplete,
            Declined,
            OutOfStock
        }

        public bool IsBusy { get; set; }
        public IEnumerable<string> Players { get; }
        public PoeLogEntry ActiveLogEntry { get; set; }
        public UserControl GetUserControl { get; }
        //public TradeStatus ActiveTradeStatus { get; }
        event EventHandler Disposed;
        bool TakeLogEntry(PoeLogEntry entry);
        public bool TakeMouseClick(MouseEventArgs args);

    }
}