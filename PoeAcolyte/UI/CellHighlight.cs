using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Helpers;

namespace PoeAcolyte.UI
{
    public partial class CellHighlight : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x80000;
        const int WS_EX_TRANSPARENT = 0x20;

        public CellHighlight()
        {
            InitializeComponent();
        }

        public CellHighlight(PoeLogEntry logEntry) : this()
        {
            float gridX = 24;
            float gridY = 24;
            float widthPerCell = Program.GameBroker.Settings.StashTab.Size.Width / gridX;
            float heightPerCell = Program.GameBroker.Settings.StashTab.Size.Height / gridY;
            float x = Program.GameBroker.Settings.StashTab.Location.X +( (logEntry.Left - 1) * widthPerCell);
            float y = Program.GameBroker.Settings.StashTab.Location.Y +( (logEntry.Top - 1) * heightPerCell);
            Show();
            Location = new Point((int)x, (int)y);
            Size = new Size((int)widthPerCell, (int)heightPerCell);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, style | WS_EX_LAYERED | WS_EX_TRANSPARENT);
        }
    }
}