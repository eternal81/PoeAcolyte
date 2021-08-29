using System;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Service
{
    /// <summary>
    /// Interface for handling events from client.txt log entries
    /// </summary>
    public interface IPoeLogReader
    {
        /// <summary>
        /// Event log arguments 
        /// </summary>
        /// <value><see cref="PoeLogEntry"/></value>
        public class PoeLogEventArgs : EventArgs
        {
            public PoeLogEntry LogEntry { get; set; }
            public override string ToString()
            {
                return $"{LogEntry.PoeLogEntryType} ({LogEntry.Player}) --- {LogEntry.Raw}";
            }
        }
        /// <summary>
        /// Any log entry is found
        /// </summary>
        public event EventHandler<PoeLogEventArgs> LogEntry;
        /// <summary>
        /// Contains @From / @To
        /// </summary>
        public event EventHandler<PoeLogEventArgs> Whisper;
        /// <summary>
        /// Single item trade with a price
        /// </summary>
        public event EventHandler<PoeLogEventArgs> PricedTrade;
        /// <summary>
        /// Single item trade without a price
        /// </summary>
        public event EventHandler<PoeLogEventArgs> UnpricedTrade;
        /// <summary>
        /// Multiple item for multiple item trade
        /// </summary>
        public event EventHandler<PoeLogEventArgs> BulkTrade;
        /// <summary>
        /// Player joined
        /// </summary>
        public event EventHandler<PoeLogEventArgs> AreaJoined;
        /// <summary>
        /// Player left
        /// </summary>
        public event EventHandler<PoeLogEventArgs> AreaLeft;
        /// <summary>
        /// You entered an area
        /// </summary>
        public event EventHandler<PoeLogEventArgs> YouJoin;
        /// <summary>
        /// Any log entry that is not a trade, whisper or predefined system message
        /// </summary>
        public event EventHandler<PoeLogEventArgs> SystemMessage;
        /// <summary>
        /// Get or Set whether the log reader is running
        /// </summary>
        public bool IsRunning { get; set; }
    }
}