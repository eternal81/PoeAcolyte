using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using AutoIt;
using Timer = System.Timers.Timer;

namespace PoeAcolyte.Service
{
    /// <summary>
    /// POE game monitoring service. Detects when client is opened and handles sending input to client
    /// </summary>
    public class PoeGameService 
    {
        /// <summary>
        /// EventArgs used for poe client being open or closed
        /// </summary>
        public class ConnectEventArgs : EventArgs
        {
            /// <summary>
            /// Is client open or closed
            /// </summary>
            public bool IsConnected { get; }

            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="value">bool if client is open</param>
            public ConnectEventArgs(bool value)
            {
                IsConnected = value;
            }
        }

        private Process _poeProcess;
        private readonly Timer _searchTimer;
        public event EventHandler<ConnectEventArgs> PoeConnected;
        public bool IsPoeFound => _poeProcess == null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PoeGameService()
        {
            _poeProcess = GetPoeProcess();
            _searchTimer = new Timer()
            {
                Interval = 5000,
                Enabled = true
            };
            _searchTimer.Elapsed += SearchTimerOnElapsed;
        }

        /// <summary>
        /// Periodic check if the POE client is open or closed. <br></br>
        /// Invokes <see cref="PoeConnected"/> if found or when closed
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void SearchTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            //Debug.Print("Searching for POE");

            _poeProcess = GetPoeProcess();
            if (_poeProcess == null) return;
            _poeProcess.EnableRaisingEvents = true;
            
            //go back to searching for poe if it is closed
            _poeProcess.Exited += (o, args) =>
            {
                _searchTimer.Enabled = true;
                PoeConnected?.Invoke(this, new ConnectEventArgs(false));
                Debug.Print("Client is closed");
            };
            _searchTimer.Enabled = false;
            PoeConnected?.Invoke(this, new ConnectEventArgs(true));
            Debug.Print("Client is opened");
        }

        /// <summary>
        /// Set POE client to foreground if Process is not null
        /// </summary>
        /// <returns>true if successful, false if no process</returns>
        public bool FocusPoe()
        {
            if (_poeProcess == null) return false;
            WIN32.SetForegroundWindow(GetPoeProcess().MainWindowHandle);
            return true;
        }

        /// <summary>
        /// Send string of text to chat console in POE
        /// </summary>
        /// <param name="command">Text to send to POE (single line only)</param>
        /// <param name="waitTime">Time in milliseconds between action commands</param>
        /// <returns>true if executed to end, false otherwise</returns>
        public bool SendCommandToClient(string command, int waitTime = 100)
        {
            return SendCommandToClient(new[] {command});
            // if (!FocusPoe()) return false;
            // WindowsInput.Simulate.Events()
            //     .Wait(waitTime*2)
            //     .Click(KeyCode.Enter).Wait(waitTime)
            //     .Click(command).Wait(waitTime)
            //     .Click(KeyCode.Enter).Wait(waitTime)
            //     .Wait(waitTime*2)
            //     .Invoke()
            //     ;
            // return true;
        }
        /// <summary>
        /// Send string of text to chat console in POE
        /// </summary>
        /// <param name="command">Text to send to POE (single line only)</param>
        /// <param name="waitTime">Time in milliseconds between action commands</param>
        /// <returns>true if executed to end, false otherwise</returns>
        public bool SendCommandToClient(string[] command, int waitTime = 100)
        {
            if (!FocusPoe()) return false;
            AutoItX.AutoItSetOption("SendKeyDelay", 1);
            AutoItX.AutoItSetOption("SendKeyDownDelay", 1);
            foreach (var s in command)
            {
                Thread.Sleep(waitTime);
                AutoItX.Send("{Enter}");
                Thread.Sleep(waitTime);
                AutoItX.Send(s);
                AutoItX.Send("{Enter}");
                Thread.Sleep(waitTime);
            }
            
            return true;
        }

        /// <summary>
        /// Searches Process stack for a match of:
        /// <list type="table">
        /// <item>PathOfExile</item>
        /// <item>PathOfExile_x64</item>
        /// <item>PathOfExileSteam</item>
        /// <item>PathOfExile_x64Steam</item>
        /// <item>PathOfExile_x64_KG.exe</item>
        /// <item>PathOfExile_KG.exe</item>
        /// </list>
        /// </summary>
        /// <returns>Process if program found, null if not</returns>
        private static Process GetPoeProcess()
        {
            var processes = Process.GetProcesses();
            // var linqProcesses =
            //     from proc in processes
            //     where proc.MainWindowTitle is "Path of Exile" or "Path of Exile x64" or "Path of Exile_KG" 
            //     select proc;

            var linqProcesses = from proc in processes
                where proc.ProcessName is "PathOfExile" or "PathOfExile_x64" or "PathOfExileSteam" or
                    "PathOfExile_x64Steam" or "PathOfExile_x64_KG.exe" or "PathOfExile_KG.exe"
                select proc;

            // eliminate possible nested enumerates
            var result = linqProcesses.ToList();
            return result.Any() ? result.First() : null;
        }
    }
}