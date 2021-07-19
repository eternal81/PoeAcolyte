using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PoeAcolyte.Service;
using PoeAcolyte.UI;

namespace PoeAcolyte.DataTypes
{
    public interface ITrade
    {
        bool IsBusy { get; set; }
        IPoeTradeControl.TradeStatus ActiveTradeStatus { get; set; }
        IEnumerable<string> Players { get; }
        PoeLogEntry ActiveLogEntry { get; set; }
        UserControl GetUserControl { get; set; }
        Action Invite { get; }
        Action DoTrade { get; }
        Action NoStock { get; }
        Action Decline { get; }
        Action WhoIs { get; }
        Action Close { get; }
        Action<PoeLogEntry> SetActiveEntry { get; }
        event EventHandler Disposed;
        bool TakeLogEntry(PoeLogEntry entry);
        void RemoveTrade(PoeLogEntry entry, bool bRemoveAll = false);
        bool CheckWhisper(PoeLogEntry entry);
        void Dispose();
    }
}