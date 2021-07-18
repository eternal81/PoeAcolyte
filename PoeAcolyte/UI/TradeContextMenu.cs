using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.UI
{
    public partial class TradeContextMenu : Component
    {
        public ContextMenuStrip ContextMenuStrip => contextMenuStrip;
        private IPoeTradeActions _actions;

        public TradeContextMenu(IPoeTradeActions actions)
        {
            InitializeComponent();
            _actions = actions;
            inviteMenuItem.Click += (_, _) =>
            {
                inviteMenuItem.Checked = true;
                _actions.Invite();
            };
            waitMenuItem.Click += (_, _) =>
            {
                waitMenuItem.Checked = true;
                _actions.AskToWait();
            };
            tradeMenuItem.Click += (_, _) =>
            {
                tradeMenuItem.Checked = true;
                _actions.Trade();
            };
            declineMenuItem.Click += (_, _) => { _actions.Decline(); };
            tyglMenuItem.Click += (_, _) => { _actions.ThanksGoodbye(); };
            whoIsMenuItem.Click += (_, _) => { _actions.WhoIs(); };
            closeMenuItem.Click += (_, _) => { _actions.Close(); };
        }

        public TradeContextMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void Refresh()
        {
            waitMenuItem.Checked = false;
            inviteMenuItem.Checked = false;
            tradeMenuItem.Checked = false;
        }

        public void AddPlayer(PoeLogEntry entry)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(entry.Player) {Name = entry.Player};
            playersMenuItem.DropDownItems.Add(item);

            // Change active trade to whoever is clicked on
            item.Click += (_, _) => { _actions.Reset(entry.Player); };

            // something at end of trade request (alternative offer)
            if (entry.Other.Length > 0) item.DropDownItems.Add(entry.Other);
        }

        public void addWhisper(PoeLogEntry entry)
        {
            var toolStripItems = playersMenuItem.DropDownItems.Find(entry.Player, false);
            if (toolStripItems.Any())
            {
                //var newitem = ((ToolStripMenuItem) toolStripItems[0]);
                ((ToolStripMenuItem) toolStripItems[0]).DropDownItems.Add(entry.Other);
            }
        }

        #region Refactoring Content

        public static ToolStripMenuItem MakeItem(string text, Action action, bool checkOnClick = false)
        {
            var ret = new ToolStripMenuItem(text);
            ret.Click += (sender, args) =>
            {
                ret.Checked = checkOnClick;
                action();
            };
            return ret;
        }
    #endregion
}
}