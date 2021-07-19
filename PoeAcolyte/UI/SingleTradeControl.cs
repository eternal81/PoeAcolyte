using System.Drawing;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Helpers;

namespace PoeAcolyte.UI
{
    public partial class SingleTradeControl : UserControl
    {
        public SingleTradeControl()
        {
            InitializeComponent();
        }
        public void UpdateControls(PoeLogEntry entry)
        {
            pbPriceUnit.Image = Converter.FromPriceString(entry.PriceUnits);
            lblInfo.Text = entry.ToString();
            lblPriceAmount.Text = entry.PriceAmount.ToString();
            toolTips.SetToolTip(lblInfo, entry.Raw);
            pbTradeDirection.Image = entry.Incoming ? Resources.Region.UpArrow : Resources.Region.DownArrow;
            BackColor = entry.Incoming ? Color.Bisque : Color.LightBlue;
            ContextMenuStrip = contextMenu;
        }
    }
}