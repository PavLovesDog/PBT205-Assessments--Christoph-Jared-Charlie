
namespace PBT205_Group_Project
{
    partial class tradeConfirmWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.confirmMessage = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // confirmMessage
            // 
            this.confirmMessage.AutoSize = true;
            this.confirmMessage.Location = new System.Drawing.Point(12, 9);
            this.confirmMessage.Name = "confirmMessage";
            this.confirmMessage.Size = new System.Drawing.Size(165, 13);
            this.confirmMessage.TabIndex = 0;
            this.confirmMessage.Text = "You have sent an order, good job";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(87, 68);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tradeConfirmWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 103);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.confirmMessage);
            this.Name = "tradeConfirmWindow";
            this.Text = "Trade Confirmation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label confirmMessage;
        private System.Windows.Forms.Button okButton;
    }
}