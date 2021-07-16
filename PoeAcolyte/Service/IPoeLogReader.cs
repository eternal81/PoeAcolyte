using System;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Service
{
    public interface IPoeLogReader
    {
        public class PoeLogEventArgs : EventArgs
        {
            public PoeLogEntry LogEntry { get; set; }
            public override string ToString()
            {
                string ret = LogEntry.PoeLogEntryType.ToString() + " (";
                ret += LogEntry.Player + ") --- " + LogEntry.Raw;
                return ret;
            }
        }
        public event EventHandler<PoeLogEventArgs> LogEntry;
        public event EventHandler<PoeLogEventArgs> Whisper;
        public event EventHandler<PoeLogEventArgs> PricedTrade;
        public event EventHandler<PoeLogEventArgs> UnpricedTrade;
        public event EventHandler<PoeLogEventArgs> BulkTrade;
        public event EventHandler<PoeLogEventArgs> AreaJoined;
        public event EventHandler<PoeLogEventArgs> AreaLeft;
        public event EventHandler<PoeLogEventArgs> YouJoin;
        public event EventHandler<PoeLogEventArgs> SystemMessage;
    }
}