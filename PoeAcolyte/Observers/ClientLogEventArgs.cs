using System;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Observers
{
    public class ClientLogEventArgs : EventArgs
    {
        public PoeLogEntry LogEntry { get; set; }

        public ClientLogEventArgs(PoeLogEntry entry)
        {
            LogEntry = entry;
        }
    }
}