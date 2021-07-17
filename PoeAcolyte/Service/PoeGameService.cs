using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WindowsInput.Events;

namespace PoeAcolyte.Service
{
    public class PoeGameService : IPoeCommands
    {
        private const int SleepInterval = 5000;
        private Thread _search;
        private Process _poeProcess;
        public class ConnectEventArgs : EventArgs
        {
            public bool IsConnected { get; set; }

            public ConnectEventArgs(bool value)
            {
                IsConnected = value;
            }
        }
        public event EventHandler<ConnectEventArgs> PoeConnected;
        public PoeLogReader LogReader { get; set; }
        public bool IsPoeFound => _poeProcess == null;
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PoeGameService()
        {
        }

        public void Start()
        {
            //if (!IsPoeFound) SearchForPoe(null, null);
            _poeProcess = null;
            PoeConnected?.Invoke(this, new ConnectEventArgs(false)); 
            _search = new Thread(CheckForPoeInstance)
                {IsBackground = true, Name = "POE Window Monitor", Priority = ThreadPriority.Lowest};
            _search.Start();
        }

        public void Stop()
        {
            _poeProcess = null;
            PoeConnected?.Invoke(this, new ConnectEventArgs(false)); 
        }
            /// <summary>
        /// Background function to check if POE is running. Saves Process if found
        /// </summary>
        private void CheckForPoeInstance()
        {
            while (_poeProcess == null )
            {
                Debug.Print("Searching for POE");

                _poeProcess = GetPoeProcess();
                Thread.Sleep(SleepInterval);

                if (_poeProcess == null) continue;

                _poeProcess.EnableRaisingEvents = true;
                _poeProcess.Exited += SearchForPoe; //go back to searching for poe if it is closed
                PoeConnected?.Invoke(this, new ConnectEventArgs(true));
            }
        }

        /// <summary>
        /// Event Handler when POE window is closed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchForPoe(object sender, EventArgs e)
        {
            Start();
        }

        /// <summary>
        /// Set POE client to foreground if Process is not null
        /// </summary>
        /// <returns>true if successful, false if no process</returns>
        private bool FocusPoe()
        {
            if (_poeProcess == null) return false;
            WIN32.SetForegroundWindow(GetPoeProcess().MainWindowHandle);
            return true;
        }

        public void SendPoeWhisper(string player, string message)
        {
            SendCommandToClient("@" + player + " " + message);
        }

        public void SendPoeCommand(IPoeCommands.CommandType type, string player)
        {
            switch (type)
            {
                case IPoeCommands.CommandType.Invite:
                    SendCommandToClient("/invite " + player);
                    break;
                case IPoeCommands.CommandType.Trade:
                    SendCommandToClient("/tradewith " + player);
                    break;
                case IPoeCommands.CommandType.Kick:
                    SendCommandToClient("/kick " + player);
                    break;
                case IPoeCommands.CommandType.Hideout:
                    SendCommandToClient("/hideout " + player);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public void SendPoeCommand(IPoeCommands.CommandType type)
        {
            switch (type)
            {
                case IPoeCommands.CommandType.Hideout:
                    SendCommandToClient("/hideout");
                    break;
                case IPoeCommands.CommandType.Delve:
                    SendCommandToClient("/delve");
                    break;
                case IPoeCommands.CommandType.Menagerie:
                    SendCommandToClient("/menagerie");
                    break;
                case IPoeCommands.CommandType.Metamorph:
                    SendCommandToClient("/metamorph");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private bool SendCommandToClient(string command, int waitTime = 50)
        {
            if (!FocusPoe()) return false;
            WindowsInput.Simulate.Events()
                .Click(KeyCode.Enter).Wait(waitTime)
                .Click(command).Wait(waitTime)
                .Click(KeyCode.Enter).Wait(waitTime)
                .Invoke()
                ;
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