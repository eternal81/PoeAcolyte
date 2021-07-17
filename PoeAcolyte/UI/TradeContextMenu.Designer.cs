using System.ComponentModel;

namespace PoeAcolyte.UI
{
    partial class TradeContextMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.declineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inviteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tradeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tyglMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whoIsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.declineMenuItem, this.waitMenuItem, this.inviteMenuItem, this.tradeMenuItem, this.tyglMenuItem, this.closeMenuItem, this.playersMenuItem, this.whoIsMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            // 
            // declineMenuItem
            // 
            this.declineMenuItem.Name = "declineMenuItem";
            this.declineMenuItem.Size = new System.Drawing.Size(142, 22);
            this.declineMenuItem.Text = "Decline";
            // 
            // waitMenuItem
            // 
            this.waitMenuItem.Name = "waitMenuItem";
            this.waitMenuItem.Size = new System.Drawing.Size(142, 22);
            this.waitMenuItem.Text = "Ask to wait";
            // 
            // inviteMenuItem
            // 
            this.inviteMenuItem.Name = "inviteMenuItem";
            this.inviteMenuItem.Size = new System.Drawing.Size(142, 22);
            this.inviteMenuItem.Text = "Invite";
            // 
            // tradeMenuItem
            // 
            this.tradeMenuItem.Name = "tradeMenuItem";
            this.tradeMenuItem.Size = new System.Drawing.Size(142, 22);
            this.tradeMenuItem.Text = "Trade";
            // 
            // tyglMenuItem
            // 
            this.tyglMenuItem.Name = "tyglMenuItem";
            this.tyglMenuItem.Size = new System.Drawing.Size(142, 22);
            this.tyglMenuItem.Text = "Ty GL (Close)";
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(142, 22);
            this.closeMenuItem.Text = "Close";
            // 
            // playersMenuItem
            // 
            this.playersMenuItem.Name = "playersMenuItem";
            this.playersMenuItem.Size = new System.Drawing.Size(142, 22);
            this.playersMenuItem.Text = "Players";
            // 
            // whoIsMenuItem
            // 
            this.whoIsMenuItem.Name = "whoIsMenuItem";
            this.whoIsMenuItem.Size = new System.Drawing.Size(142, 22);
            this.whoIsMenuItem.Text = "WhoIs";
            this.contextMenuStrip.ResumeLayout(false);
        }

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem whoIsMenuItem;

        private System.Windows.Forms.ToolStripMenuItem declineMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inviteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tradeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tyglMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playersMenuItem;
        #endregion
    }
}