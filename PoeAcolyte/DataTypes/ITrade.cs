using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PoeAcolyte.Service;
using PoeAcolyte.UI;

namespace PoeAcolyte.DataTypes
{
    public interface ITrade
    {
        public enum TradeStatus
        {
            None,
            AskedToWait,
            Invited,
            Traded
        }
        public bool IsBusy { get; set; }
        public IEnumerable<string> Players { get; }
        public PoeLogEntry ActiveLogEntry { get; set; }
        public UserControl GetUserControl { get; }

        event EventHandler Disposed;
        bool TakeLogEntry(PoeLogEntry entry);

    }
}