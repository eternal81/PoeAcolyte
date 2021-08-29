using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Observers
{
    /// <summary>
    /// Timer is used over filesystemwatcher since game does not flush after writing to
    /// client file urgo no OS events fired for a file change
    /// </summary>
    public class ClientLogObserver
    {
        public event EventHandler<ClientLogEventArgs> NewLogEntry;
        
        private readonly string _filePath;
        private const string DEFAULT_PATH = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\Client.txt";
        private long _lastLogIndex;
        private Timer _logTimer;

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="autoStart"></param>
        /// <param name="pollingRate"></param>
        public ClientLogObserver(string filePath = DEFAULT_PATH, bool autoStart = true, int pollingRate = 2000)
        {
            _filePath = filePath;
            _lastLogIndex = FindLog_EOF();
            _logTimer = new Timer()
            {
                Enabled = autoStart,
                Interval = pollingRate
            };
            _logTimer.Elapsed += LogTimerOnElapsed;
        }

        /// <summary>
        /// Periodic file check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var newEntries = GetNewLogEntries();

            if (newEntries is null) return;

            foreach (var entry in newEntries)
            {            
                PoeLogEntry logEntry = new PoeLogEntry(entry);
                if (!logEntry.IsValid) continue;
                NewLogEntry?.Invoke(entry, new ClientLogEventArgs(logEntry));
            }
        }
        
        /// <summary>
        /// Find what has changed 
        /// </summary>
        /// <returns>string of anything past last index</returns>
        private IEnumerable<string> GetNewLogEntries()
        {
            try
            {
                using var file = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var currentPosition = _lastLogIndex;
                _lastLogIndex = file.Length - 1;

                if (currentPosition >= _lastLogIndex) return null;

                file.Position = currentPosition;
                using var reader = new StreamReader(file);
                var lines = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line)) lines.Add(line);
                }
                return lines;
            }
            catch (FileNotFoundException exception)
            {
                Program.Log.Debug("PoeLogReader (LogTimeOnElapsed) - Exception: {exception}", exception.Message);
                Program.Log.Debug(exception.StackTrace);
                Debug.Print(exception.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// Constructor usage to find end of file
        /// </summary>
        /// <returns>long index of end of file</returns>
        private long FindLog_EOF()
        {
            try
            {
                using var file = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return file.Length - 1;
            }
            catch (Exception e)
            {
                Program.Log.Debug("PoeLogReader (FindLogEof) - Exception: {exception}", e.Message);
                return -1;
            }
        }
    }
}