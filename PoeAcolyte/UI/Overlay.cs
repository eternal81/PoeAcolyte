using System;
using System.Windows.Forms;
using PoeAcolyte.Helpers;
using Serilog;

namespace PoeAcolyte.UI
{
    public partial class Overlay : Form
    {

        private PoeGameBroker _broker;
        public Overlay()
        {
            InitializeComponent();
            _broker = new PoeGameBroker(tradesPanel);
            _broker.RefreshUI();
            Program.Log.Verbose("Overlay - POE Acolyte started");
        }

        private void OnClose(object sender, EventArgs e)
        {
            Program.Log.Verbose("Overlay - POE Acolyte exited");
            Application.Exit();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
           
            _broker.ManualFire(
                @"2021/07/04 08:38:45 67873453 bad [INFO Client 22384] @From FuziCoc: Hi, I'd like to buy your 4 Ancient Orb for my 40 Chaos Orb in Ultimatum.");
            _broker.ManualFire(
                @"2020/12/26 18:23:22 182990578 ba9 [INFO Client 2268] @From TryingWinterO: Hi, I would like to buy your Demon Loop Coral Ring listed for 5 chaos in Flashback (stash tab ""~b/o 5 chaos""; position: left 3, top 24)");
            _broker.ManualFire(
                @"2020/12/26 18:23:22 182990578 ba9 [INFO Client 2268] @From FuziCoc: Hi, I would like to buy your Demon Loop Coral Ring in Flashback (stash tab ""~b/o 5 chaos""; position: left 3, top 22)");
            _broker.ManualFire(
                @"2020/12/26 18:23:22 182990578 ba9 [INFO Client 2268] @From FuziCoc2: Hi, I would like to buy your Demon Loop Coral Ring listed for 5 chaos in Flashback (stash tab ""~b/o 5 chaos""; position: left 3, top 24) offer 20c");
            _broker.ManualFire(
                @"2020/12/26 18:23:22 182990578 ba9 [INFO Client 2268] @From FuziCoc: still available");

        }

        private void setBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menu = (ToolStripMenuItem) sender;
            if (menu.Text == "Set Bounds")
            {
                
                _broker.Settings.ShowOverlay(Controls);
                menu.Text = "Save Bounds";
            }
            else
            {
                menu.Text = "Set Bounds";
                _broker.Settings.Save();
                _broker.Settings.HideOverlay(Controls);
                _broker.RefreshUI();
            };
        }
    }
}