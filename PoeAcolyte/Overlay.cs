using System;
using System.IO;
using System.Windows.Forms;
using PoeAcolyte.Helpers;

namespace PoeAcolyte
{
    public partial class Overlay : Form
    {
        //private PoeGameBroker _broker;
        public Overlay()
        {
            InitializeComponent();
            Program.GameBroker.TradePanel = tradesPanel;
            Program.GameBroker.RefreshUI();
            Program.Log.Verbose("Overlay - POE Acolyte started");
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