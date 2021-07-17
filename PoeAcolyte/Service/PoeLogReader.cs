using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Service
{
    public class PoeLogReader : IPoeLogReader
    {
        #region IPoeLogReader

        public event EventHandler<IPoeLogReader.PoeLogEventArgs> LogEntry;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> Whisper;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> PricedTrade;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> UnpricedTrade;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> BulkTrade;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> AreaJoined;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> AreaLeft;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> YouJoin;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> SystemMessage;
        public bool IsRunning
        {
            get => _logTimer.Enabled;
            set
            {
                // reset to end of file so we don't get bombarded by entries sent while idle
                _lastLogIndex = FindLogEOF(); 
                _logTimer.Enabled = value;
            }
        }

        #endregion

        private readonly string _filePath;
        private const string DEFAULT_PATH = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\Client.txt";
        private long _lastLogIndex;
        private readonly Timer _logTimer;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="filePath">use default installation file path if alternate is not specified</param>
        public PoeLogReader(string filePath = DEFAULT_PATH)
        {
            _filePath = filePath;
            _lastLogIndex = FindLogEOF(); // Set our last index as end of file (so we don't scan entire history)
            _logTimer = new Timer()
            {
                Interval = 1000,
                Enabled = true
            };
            _logTimer.Elapsed += LogTimerOnElapsed;
        }

        /// <summary>
        /// Periodic inspection of the client.txt file for changes
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void LogTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var newLines = ReadNewLines();
                foreach (var entry in newLines.Select(line => new PoeLogEntry(line)).Where(entry => entry.IsValid))
                {
                    OnNewLogEntry(entry);
                }
            }
            catch (Exception exception)
            {
                Program.Log.Debug("PoeLogReader (LogTimeOnElapsed) - Exception: {exception}",exception.Message);
            }
        }
        
        /// <summary>
        /// Scans Client.txt file
        /// </summary>
        /// <returns>Number of line items (minus 1) or -1 on exception</returns>
        private long FindLogEOF()
        {
            try
            {
                using var file = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return file.Length - 1;
            }
            catch (Exception e)
            {
                Program.Log.Debug("PoeLogReader (FindLogEof) - Exception: {exception}",e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Event Invoker for new log entries
        /// </summary>
        /// <param name="entry">entry to be passed on as EventArgs</param>
        /// <exception cref="ArgumentOutOfRangeException">message type not specified</exception>
        private void OnNewLogEntry(PoeLogEntry entry)
        {
            IPoeLogReader.PoeLogEventArgs eventArgs = new IPoeLogReader.PoeLogEventArgs() {LogEntry = entry};
            LogEntry?.Invoke(this, eventArgs);

            switch (entry.PoeLogEntryType)
            {
                case IPoeLogEntry.PoeLogEntryTypeEnum.Whisper:
                    Whisper?.Invoke(this, eventArgs);
                    break;
                case IPoeLogEntry.PoeLogEntryTypeEnum.PricedTrade:
                    PricedTrade?.Invoke(this, eventArgs);
                    break;
                case IPoeLogEntry.PoeLogEntryTypeEnum.UnpricedTrade:
                    UnpricedTrade?.Invoke(this, eventArgs);
                    break;
                case IPoeLogEntry.PoeLogEntryTypeEnum.BulkTrade:
                    BulkTrade?.Invoke(this, eventArgs);
                    break;
                case IPoeLogEntry.PoeLogEntryTypeEnum.AreaJoined:
                    AreaJoined?.Invoke(this, eventArgs);
                    break;
                case IPoeLogEntry.PoeLogEntryTypeEnum.AreaLeft:
                    AreaLeft?.Invoke(this, eventArgs);
                    break;
                case IPoeLogEntry.PoeLogEntryTypeEnum.YouJoin:
                    YouJoin?.Invoke(this, eventArgs);
                    break;
                case IPoeLogEntry.PoeLogEntryTypeEnum.SystemMessage:
                    SystemMessage?.Invoke(this, eventArgs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Find new entries in client log
        /// </summary>
        /// <returns>new entries</returns>
        private IEnumerable<string> ReadNewLines()
        {
            var lines = new List<string>();
            var currentPosition = _lastLogIndex;
            _lastLogIndex = FindLogEOF();

            if (currentPosition >= _lastLogIndex) return lines; //no new entries

            using var file = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            file.Position = currentPosition;
            using var reader = new StreamReader(file);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line)) lines.Add(line);
            }

            return lines;
        }

        /// <summary>
        /// Used for debugging and testing. Simulates an event fire on provided value
        /// </summary>
        /// <param name="raw">Properly formatted log entry used in client.txt file</param>
        public void ManualFire(string raw)
        {
            OnNewLogEntry(new PoeLogEntry(raw));
        }
    }
}