using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using PoeAcolyte.Helpers;
using PoeAcolyte.Observers;

namespace PoeAcolyte
{
    public partial class Overlay : Form
    {
        //private PoeGameBroker _broker;
        private ClientLogObserver _observer;
        public Overlay()
        {
            InitializeComponent();
            _observer  = new ClientLogObserver();
            _observer.NewLogEntry += ObserverOnNewLogEntry;

            // Program.GameBroker.TradePanel = tradesPanel;
            // Program.GameBroker.RefreshUI();
            // Program.Log.Verbose("Overlay - POE Acolyte started");
        }

        private void ObserverOnNewLogEntry(object sender, ClientLogEventArgs e)
        {

               
                Debug.Print(e.LogEntry.ToString());
                lblInfo.Text = e.LogEntry.ToString();

        }

        private void WatchOnChanged(object sender, FileSystemEventArgs e)
        {
            Debug.Print(e.ChangeType.ToString());
        }

        private void OnClose(object sender, EventArgs e)
        {
            Program.Log.Verbose("Overlay - POE Acolyte exited");
            Application.Exit();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Program.GameBroker.ManualFire();
        }

        private void setBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem) sender;
            if (menu.Text == "Set Bounds")
            {
                Program.GameBroker.Settings.ShowOverlay(Controls);
                menu.Text = "Save Bounds";
            }
            else
            {
                menu.Text = "Set Bounds";
                Program.GameBroker.Settings.Save();
                Program.GameBroker.Settings.HideOverlay(Controls);
                Program.GameBroker.RefreshUI();
            }

            ;
        }
    }
}