using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.Observers
{
    public class ClientLogObserver
    {
        public event EventHandler<ClientLogEventArgs> NewLogEntry;
        
        private readonly string _filePath;
        private const string DEFAULT_PATH = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\Client.txt";
        private long _lastLogIndex;

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
            var logTimer = new Timer()
            {
                Enabled = autoStart,
                Interval = pollingRate
            };
            logTimer.Elapsed += LogTimerOnElapsed;
        }

        /// <summary>
        /// Periodic file check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var newEntries = GetNewLogEntries()? // null check
                .Split("\r\n".ToCharArray()); // separate by new line
                //.Where(s => s.Length>2); // filter out empty lines

            if (newEntries is null) return;
                // foreach (var str in newEntries)
                // {
                //     Debug.Print(str);
                // }
            foreach (var strEntry in newEntries)
            {
                Debug.Print(strEntry);
                OnNewLogEntry(strEntry);
            }

        }

        /// <summary>
        /// Find what has changed 
        /// </summary>
        /// <returns>string of anything past last index</returns>
        private string GetNewLogEntries()
        {
            try
            {
                using var file = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var currentPosition = _lastLogIndex;
                _lastLogIndex = file.Length - 1;

                if (currentPosition >= _lastLogIndex) return null;

                file.Position = currentPosition;
                using var reader = new StreamReader(file);
                return reader.ReadToEnd();
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
        /// Event Invoker for new log entries
        /// </summary>
        /// <param name="entry">entry to be passed on as EventArgs</param>
        /// <exception cref="ArgumentOutOfRangeException">message type not specified</exception>
        private void OnNewLogEntry(string entry)
        {
// Debug.Print($"Invoking: {entry}");
            PoeLogEntry logEntry = new PoeLogEntry(entry);
            if (logEntry.IsValid)
            {
                
                // invoke
                NewLogEntry?.Invoke(entry, new ClientLogEventArgs(logEntry));
            }
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