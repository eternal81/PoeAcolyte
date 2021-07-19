using System;
using System.Windows.Forms;
using PoeAcolyte.UI;

namespace PoeAcolyte.DataTypes
{
    public class SingleTrade : Trade, IPoeTradeControl
    {
        private readonly SingleTradeControl _singleTradeControl;
        public override UserControl GetUserControl => _singleTradeControl;
        public sealed override PoeLogEntry ActiveLogEntry
        {
            get => ActiveEntry;
            set
            {
                ActiveEntry = value;
                _singleTradeControl.UpdateControls(ActiveEntry);
            }
        }
        public SingleTrade(PoeLogEntry entry) : base(entry)
        {
            _singleTradeControl = new SingleTradeControl();
            ActiveLogEntry = entry;
            SetContext();
        }
        private void SetContext()
        {
            _singleTradeControl.ContextMenuStrip.Items.Clear();
            _singleTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("No Thanks", Decline));
            _singleTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Invite", Invite, true));
            _singleTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Trade", DoTrade, true));
            _singleTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Out of stock", NoStock));
            _singleTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("Close", Close));
            _singleTradeControl.ContextMenuStrip.Items.Add(MakeMenuItem("WhoIs", WhoIs));
            _singleTradeControl.ContextMenuStrip.Items.Add(PlayersMenu);
            PlayersMenu.DropDownItems.Add(AddPlayerToMenu(ActiveEntry, SetActiveEntry));
        }
        
        public void AddLogEntry(PoeLogEntry entry)
        {
            throw new NotImplementedException();
        }

        public override bool TakeLogEntry(PoeLogEntry entry)
        {
            if (CheckWhisper(entry)) return true;
            
            if (entry.Incoming == ActiveLogEntry.Incoming && entry.Outgoing == ActiveLogEntry.Outgoing &&
                entry.Item == ActiveLogEntry.Item && entry.Top == ActiveLogEntry.Top && entry.Left == ActiveLogEntry.Left )
            {
                LogEntries.Add(entry);
                PlayersMenu.DropDownItems.Add(AddPlayerToMenu(entry, SetActiveEntry));
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