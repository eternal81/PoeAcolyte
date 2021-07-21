﻿using System.ComponentModel;
using Gma.System.MouseKeyHook;

namespace PoeAcolyte.UI
{
    partial class CellHighlight
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
            Hook.GlobalEvents().MouseClick -= OnGlobalMouseClick; 
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CellHighlight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.ClientSize = new System.Drawing.Size(170, 85);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CellHighlight";
            this.Opacity = 0.5D;
            this.ShowInTaskbar = false;
            this.Text = "CellHighlight";
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        #endregion
    }
}