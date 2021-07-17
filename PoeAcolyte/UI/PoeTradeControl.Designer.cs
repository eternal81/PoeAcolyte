using System.ComponentModel;

namespace PoeAcolyte.UI
{
    partial class PoeTradeControl
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
            this.pbPriceUnit = new System.Windows.Forms.PictureBox();
            this.lblPriceAmount = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.declineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inviteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tradeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tyglMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whoIsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.pbPriceUnit)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbPriceUnit
            // 
            this.pbPriceUnit.BackColor = System.Drawing.Color.White;
            this.pbPriceUnit.Location = new System.Drawing.Point(0, 0);
            this.pbPriceUnit.Name = "pbPriceUnit";
            this.pbPriceUnit.Size = new System.Drawing.Size(32, 32);
            this.pbPriceUnit.TabIndex = 0;
            this.pbPriceUnit.TabStop = false;
            // 
            // lblPriceAmount
            // 
            this.lblPriceAmount.BackColor = System.Drawing.Color.White;
            this.lblPriceAmount.Location = new System.Drawing.Point(0, 35);
            this.lblPriceAmount.Name = "lblPriceAmount";
            this.lblPriceAmount.Size = new System.Drawing.Size(32, 16);
            this.lblPriceAmount.TabIndex = 1;
            this.lblPriceAmount.Text = "999";
            this.lblPriceAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.Color.White;
            this.lblInfo.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblInfo.Location = new System.Drawing.Point(38, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(152, 51);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "999";
            this.lblInfo.Click += new System.EventHandler(this.lblInfo_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.declineMenuItem, this.waitMenuItem, this.inviteMenuItem, this.tradeMenuItem, this.tyglMenuItem, this.closeMenuItem, this.playersMenuItem, this.whoIsMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(143, 180);
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
            this.waitMenuItem.Text = "Wait";
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
            // 
            // PoeTradeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblPriceAmount);
            this.Controls.Add(this.pbPriceUnit);
            this.Name = "PoeTradeControl";
            this.Size = new System.Drawing.Size(193, 56);
            ((System.ComponentModel.ISupportInitialize) (this.pbPriceUnit)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ToolStripMenuItem whoIsMenuItem;

        private System.Windows.Forms.ToolStripMenuItem declineMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inviteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tradeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tyglMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playersMenuItem;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.PictureBox pbPriceUnit;
        private System.Windows.Forms.Label lblPriceAmount;

        #endregion
    }
}