using System.ComponentModel;

namespace PoeAcolyte.UI
{
    partial class SingleTradeControl
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblPriceAmount = new System.Windows.Forms.Label();
            this.pbPriceUnit = new System.Windows.Forms.PictureBox();
            this.pbTradeDirection = new System.Windows.Forms.PictureBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.pbPriceUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pbTradeDirection)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblInfo.Location = new System.Drawing.Point(41, 2);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(146, 53);
            this.lblInfo.TabIndex = 8;
            this.lblInfo.Text = "label3";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(41, 64);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(146, 16);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "999k";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPriceAmount
            // 
            this.lblPriceAmount.Location = new System.Drawing.Point(3, 37);
            this.lblPriceAmount.Name = "lblPriceAmount";
            this.lblPriceAmount.Size = new System.Drawing.Size(32, 18);
            this.lblPriceAmount.TabIndex = 6;
            this.lblPriceAmount.Text = "999k";
            this.lblPriceAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbPriceUnit
            // 
            this.pbPriceUnit.Location = new System.Drawing.Point(3, 2);
            this.pbPriceUnit.Name = "pbPriceUnit";
            this.pbPriceUnit.Size = new System.Drawing.Size(32, 32);
            this.pbPriceUnit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPriceUnit.TabIndex = 5;
            this.pbPriceUnit.TabStop = false;
            // 
            // pbTradeDirection
            // 
            this.pbTradeDirection.Location = new System.Drawing.Point(3, 51);
            this.pbTradeDirection.Name = "pbTradeDirection";
            this.pbTradeDirection.Size = new System.Drawing.Size(32, 29);
            this.pbTradeDirection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTradeDirection.TabIndex = 9;
            this.pbTradeDirection.TabStop = false;
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(153, 26);
            // 
            // SingleTradeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (255)))), ((int) (((byte) (192)))));
            this.Controls.Add(this.pbTradeDirection);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblPriceAmount);
            this.Controls.Add(this.pbPriceUnit);
            this.Name = "SingleTradeControl";
            this.Size = new System.Drawing.Size(190, 80);
            ((System.ComponentModel.ISupportInitialize) (this.pbPriceUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pbTradeDirection)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ContextMenuStrip contextMenu;

        private System.Windows.Forms.ToolTip toolTips;

        public System.Windows.Forms.PictureBox pbTradeDirection;

        public System.Windows.Forms.Label lblStatus;

        public System.Windows.Forms.Label lblInfo;
        public System.Windows.Forms.Label lblPriceAmount;
        public System.Windows.Forms.PictureBox pbPriceUnit;

        #endregion
    }
}