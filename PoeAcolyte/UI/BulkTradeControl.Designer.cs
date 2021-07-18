using System.ComponentModel;

namespace PoeAcolyte.UI
{
    partial class BulkTradeControl
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
            this.pbBuyUnit = new System.Windows.Forms.PictureBox();
            this.lblPriceAmount = new System.Windows.Forms.Label();
            this.lblBuyAmount = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pbTradeDirection = new System.Windows.Forms.PictureBox();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.pbPriceUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pbBuyUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pbTradeDirection)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPriceUnit
            // 
            this.pbPriceUnit.Location = new System.Drawing.Point(3, 3);
            this.pbPriceUnit.Name = "pbPriceUnit";
            this.pbPriceUnit.Size = new System.Drawing.Size(32, 32);
            this.pbPriceUnit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPriceUnit.TabIndex = 0;
            this.pbPriceUnit.TabStop = false;
            // 
            // pbBuyUnit
            // 
            this.pbBuyUnit.Location = new System.Drawing.Point(3, 45);
            this.pbBuyUnit.Name = "pbBuyUnit";
            this.pbBuyUnit.Size = new System.Drawing.Size(32, 32);
            this.pbBuyUnit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBuyUnit.TabIndex = 1;
            this.pbBuyUnit.TabStop = false;
            // 
            // lblPriceAmount
            // 
            this.lblPriceAmount.Location = new System.Drawing.Point(41, 3);
            this.lblPriceAmount.Name = "lblPriceAmount";
            this.lblPriceAmount.Size = new System.Drawing.Size(32, 18);
            this.lblPriceAmount.TabIndex = 2;
            this.lblPriceAmount.Text = "999k";
            this.lblPriceAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblBuyAmount
            // 
            this.lblBuyAmount.Location = new System.Drawing.Point(41, 60);
            this.lblBuyAmount.Name = "lblBuyAmount";
            this.lblBuyAmount.Size = new System.Drawing.Size(32, 17);
            this.lblBuyAmount.TabIndex = 3;
            this.lblBuyAmount.Text = "999k";
            this.lblBuyAmount.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblInfo.Location = new System.Drawing.Point(79, 3);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(108, 77);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "label3";
            // 
            // pbTradeDirection
            // 
            this.pbTradeDirection.Location = new System.Drawing.Point(41, 24);
            this.pbTradeDirection.Name = "pbTradeDirection";
            this.pbTradeDirection.Size = new System.Drawing.Size(32, 32);
            this.pbTradeDirection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTradeDirection.TabIndex = 5;
            this.pbTradeDirection.TabStop = false;
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(153, 26);
            // 
            // BulkTradeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (255)))), ((int) (((byte) (192)))));
            this.Controls.Add(this.pbTradeDirection);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblBuyAmount);
            this.Controls.Add(this.lblPriceAmount);
            this.Controls.Add(this.pbBuyUnit);
            this.Controls.Add(this.pbPriceUnit);
            this.Name = "BulkTradeControl";
            this.Size = new System.Drawing.Size(190, 80);
            ((System.ComponentModel.ISupportInitialize) (this.pbPriceUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pbBuyUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pbTradeDirection)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ContextMenuStrip contextMenu;

        private System.Windows.Forms.ToolTip toolTips;

        public System.Windows.Forms.PictureBox pbTradeDirection;

        public System.Windows.Forms.Label lblInfo;

        public System.Windows.Forms.PictureBox pbPriceUnit;
        public System.Windows.Forms.PictureBox pbBuyUnit;
        public System.Windows.Forms.Label lblPriceAmount;
        public System.Windows.Forms.Label lblBuyAmount;

        #endregion
    }
}