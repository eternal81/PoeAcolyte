using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Service
{
    public class PoeLogReader : IPoeLogReader
    {
         #region Events

         public event EventHandler<IPoeLogReader.PoeLogEventArgs> LogEntry;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> Whisper;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> PricedTrade;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> UnpricedTrade;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> BulkTrade;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> AreaJoined;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> AreaLeft;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> YouJoin;
        public event EventHandler<IPoeLogReader.PoeLogEventArgs> SystemMessage;

        #endregion

        private readonly string _filePath;
        private const string DefaultPath = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\Client.txt";
        private long _lastLogIndex;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="filePath">use default installation file path if alternate is not specified</param>
        public PoeLogReader(string filePath = DefaultPath)
        {
            _filePath = filePath;
            _lastLogIndex = FindLogEOF(); // Set our last index as end of file (so we don't scan entire history)
            MonitorClientLog(); // automatically start monitoring
        }

        /// <summary>
        /// Scans Client.log file
        /// </summary>
        /// <returns>Number of line items (minus 1)</returns>
        private long FindLogEOF()
        {
            try
            {
                var file = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var retValue = file.Length - 1;
                file.Close();
                return retValue;
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                return -1;
            }
            
        }

        /// <summary>
        /// Async monitor chat log for additions; invoke events on new entries
        /// </summary>
        /// <param name="delay">default delay</param>
        private async void MonitorClientLog(int delay = 500)
        {
            while (true)
            {
                var newLines = new List<string>();
                do
                {
                    try
                    {
                        await Task.Delay(delay);
                        newLines = ReadNewLines();
                    }
                    catch (Exception e)
                    {
                        Debug.Print(e.Message);
                    }
                } while (!newLines.Any()); // loop until new lines found

                foreach (var entry in newLines.Select(line => new PoeLogEntry(line)).Where(entry => entry.IsValid))
                {
                    OnNewLogEntry(entry);
                }
            }
        } // TODO implement cancellation token?

        /// <summary>
        /// Event Handler for new log entries
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
        private List<string> ReadNewLines()
        {
            var lines = new List<string>();
            var currentPosition = _lastLogIndex;
            _lastLogIndex = FindLogEOF();

            if (currentPosition >= _lastLogIndex)
            {
                return lines; //no new entries
            }

            var file = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            file.Position = currentPosition;
            var reader = new StreamReader(file);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line); //RemoveSpecialChars(line));
                }
            }

            // cleanup
            reader.Close();
            file.Close();

            return lines;
        }

        /// <summary>
        /// Used for debugging and testing
        /// </summary>
        /// <param name="raw"></param>
        public void ManualFire(string raw)
        {
            OnNewLogEntry(new PoeLogEntry(raw));
        }
    }
}