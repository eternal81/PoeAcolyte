using System;
using System.Windows.Forms;
using PoeAcolyte.UI;

namespace PoeAcolyte.DataTypes
{
    public class SingleTrade : Trade
    {
        protected override Control StatusControl => _singleTradeControl.lblStatus;
        protected override ToolStripItemCollection MenuItems => _singleTradeControl.ContextMenuStrip.Items;
        private readonly SingleTradeControl _singleTradeControl;
        public override UserControl GetUserControl => _singleTradeControl;

        public SingleTrade(PoeLogEntry entry) : base(entry)
        {
            _singleTradeControl = new SingleTradeControl();
            ActiveLogEntry = entry;
            
            BuildContextMenuStrip();
        }
        
        public override void UpdateControls()
        {
            _singleTradeControl.UpdateControls(ActiveLogEntry);
        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (CheckWhisper(entry)) return true;
            
            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.Item == ActiveLogEntry.Item && entry.Top == ActiveLogEntry.Top && entry.Left == ActiveLogEntry.Left )
            {
                _LogEntries.Add(entry);
                _PlayersMenu.DropDownItems.Add(AddPlayerToMenu(entry, SetActiveEntry));
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
            _singleTradeControl.Dispose();
        }
        

    }
}