using System.Drawing;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Helpers;

namespace PoeAcolyte.UI
{
    public partial class BulkTradeControl : UserControl
    {
        public ContextMenuStrip Contextmenu => contextMenu;
        public BulkTradeControl()
        {
            InitializeComponent();
        }

        public void UpdateControls(PoeLogEntry entry)
        {
            pbBuyUnit.Image = Converter.FromPriceString(entry.BuyPriceUnits);
            pbPriceUnit.Image = Converter.FromPriceString(entry.PriceUnits);
            lblInfo.Text = entry.ToString();
            lblBuyAmount.Text = entry.BuyPriceAmount.ToString();
            lblPriceAmount.Text = entry.PriceAmount.ToString();
            toolTips.SetToolTip(lblInfo, entry.Raw);
            pbTradeDirection.Image = entry.Incoming ? Resources.Region.UpArrow : Resources.Region.DownArrow;
            BackColor = entry.Incoming ? Color.Bisque : Color.LightBlue;
            
            lblInfo.ContextMenuStrip = contextMenu;
            this.ContextMenuStrip = contextMenu;
        }
    }
}