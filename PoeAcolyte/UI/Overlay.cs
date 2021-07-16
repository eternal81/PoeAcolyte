using System;
using System.Drawing;
using System.Windows.Forms;
using PoeAcolyte.Helpers;
using PoeAcolyte.Service;

namespace PoeAcolyte.UI
{
    public partial class Overlay : Form
    {

        private PoeGameBroker _broker;
        public Overlay()
        {
            InitializeComponent();
            _broker = new PoeGameBroker()
            {
                TradePanel = tradesPanel
            };
            _broker.RefreshUI();
        }

        private void OnClose(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

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