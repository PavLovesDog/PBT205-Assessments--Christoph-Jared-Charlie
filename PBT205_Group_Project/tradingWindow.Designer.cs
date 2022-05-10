
namespace PBT205_Group_Project
{
    partial class tradingWindow
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
            this.lstTopics = new System.Windows.Forms.ListBox();
            this.priceLabel = new System.Windows.Forms.Label();
            this.priceTextBox = new System.Windows.Forms.TextBox();
            this.buySellCombo = new System.Windows.Forms.ComboBox();
            this.confirmOrderBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.messageBox = new System.Windows.Forms.Label();
            this.quitBtn = new System.Windows.Forms.Button();
            this.logOutBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstTopics
            // 
            this.lstTopics.FormattingEnabled = true;
            this.lstTopics.Items.AddRange(new object[] {
            "Orders",
            "Trades"});
            this.lstTopics.Location = new System.Drawing.Point(12, 12);
            this.lstTopics.Name = "lstTopics";
            this.lstTopics.Size = new System.Drawing.Size(73, 121);
            this.lstTopics.TabIndex = 0;
            this.lstTopics.SelectedIndexChanged += new System.EventHandler(this.lstTopics_SelectedIndexChanged);
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.Location = new System.Drawing.Point(19, 152);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(34, 13);
            this.priceLabel.TabIndex = 1;
            this.priceLabel.Text = "Price:";
            // 
            // priceTextBox
            // 
            this.priceTextBox.Location = new System.Drawing.Point(61, 148);
            this.priceTextBox.Name = "priceTextBox";
            this.priceTextBox.Size = new System.Drawing.Size(62, 20);
            this.priceTextBox.TabIndex = 2;
            this.priceTextBox.Text = "0";
            // 
            // buySellCombo
            // 
            this.buySellCombo.FormattingEnabled = true;
            this.buySellCombo.Items.AddRange(new object[] {
            "Buy",
            "Sell"});
            this.buySellCombo.Location = new System.Drawing.Point(129, 148);
            this.buySellCombo.Name = "buySellCombo";
            this.buySellCombo.Size = new System.Drawing.Size(121, 21);
            this.buySellCombo.TabIndex = 3;
            this.buySellCombo.Text = "Buy/Sell";
            // 
            // confirmOrderBtn
            // 
            this.confirmOrderBtn.Location = new System.Drawing.Point(256, 148);
            this.confirmOrderBtn.Name = "confirmOrderBtn";
            this.confirmOrderBtn.Size = new System.Drawing.Size(96, 23);
            this.confirmOrderBtn.TabIndex = 4;
            this.confirmOrderBtn.Text = "Send";
            this.confirmOrderBtn.UseVisualStyleBackColor = true;
            this.confirmOrderBtn.Click += new System.EventHandler(this.confirmOrderBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.messageBox);
            this.panel1.Location = new System.Drawing.Point(90, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 121);
            this.panel1.TabIndex = 5;
            // 
            // messageBox
            // 
            this.messageBox.AutoSize = true;
            this.messageBox.Location = new System.Drawing.Point(1, 1);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(112, 13);
            this.messageBox.TabIndex = 0;
            this.messageBox.Text = "Placeholder for testing";
            // 
            // quitBtn
            // 
            this.quitBtn.Location = new System.Drawing.Point(114, 201);
            this.quitBtn.Name = "quitBtn";
            this.quitBtn.Size = new System.Drawing.Size(75, 23);
            this.quitBtn.TabIndex = 6;
            this.quitBtn.Text = "Quit";
            this.quitBtn.UseVisualStyleBackColor = true;
            this.quitBtn.Click += new System.EventHandler(this.quitBtn_Click);
            // 
            // logOutBtn
            // 
            this.logOutBtn.Location = new System.Drawing.Point(195, 201);
            this.logOutBtn.Name = "logOutBtn";
            this.logOutBtn.Size = new System.Drawing.Size(75, 23);
            this.logOutBtn.TabIndex = 7;
            this.logOutBtn.Text = "Log Out";
            this.logOutBtn.UseVisualStyleBackColor = true;
            this.logOutBtn.Click += new System.EventHandler(this.logOutBtn_Click);
            // 
            // tradingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 236);
            this.Controls.Add(this.logOutBtn);
            this.Controls.Add(this.quitBtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.confirmOrderBtn);
            this.Controls.Add(this.buySellCombo);
            this.Controls.Add(this.priceTextBox);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.lstTopics);
            this.Name = "tradingWindow";
            this.Text = "Trading XYZ-Corp";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstTopics;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.TextBox priceTextBox;
        private System.Windows.Forms.ComboBox buySellCombo;
        private System.Windows.Forms.Button confirmOrderBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label messageBox;
        private System.Windows.Forms.Button quitBtn;
        private System.Windows.Forms.Button logOutBtn;
    }
}