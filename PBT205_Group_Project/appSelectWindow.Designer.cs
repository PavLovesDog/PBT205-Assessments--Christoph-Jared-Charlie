
namespace PBT205_Group_Project
{
    partial class appSelectWindow
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.titleText = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tradingButton = new System.Windows.Forms.Button();
            this.messagingButton = new System.Windows.Forms.Button();
            this.contactButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.05213F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.94787F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 281F));
            this.tableLayoutPanel1.Controls.Add(this.titleText, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.13181F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.86819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(844, 506);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // titleText
            // 
            this.titleText.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.titleText.AutoSize = true;
            this.titleText.Font = new System.Drawing.Font("Comic Sans MS", 19.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleText.Location = new System.Drawing.Point(296, 114);
            this.titleText.Name = "titleText";
            this.titleText.Size = new System.Drawing.Size(246, 47);
            this.titleText.TabIndex = 7;
            this.titleText.Text = "Project A.N.I";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.flowLayoutPanel1.Controls.Add(this.tradingButton);
            this.flowLayoutPanel1.Controls.Add(this.messagingButton);
            this.flowLayoutPanel1.Controls.Add(this.contactButton);
            this.flowLayoutPanel1.Controls.Add(this.closeButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(324, 204);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(189, 142);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // tradingButton
            // 
            this.tradingButton.AutoSize = true;
            this.tradingButton.Location = new System.Drawing.Point(3, 3);
            this.tradingButton.Name = "tradingButton";
            this.tradingButton.Size = new System.Drawing.Size(185, 27);
            this.tradingButton.TabIndex = 0;
            this.tradingButton.Text = "Trading App";
            this.tradingButton.UseVisualStyleBackColor = true;
            this.tradingButton.Click += new System.EventHandler(this.TradingButton_Click);
            // 
            // messagingButton
            // 
            this.messagingButton.AutoSize = true;
            this.messagingButton.Location = new System.Drawing.Point(3, 36);
            this.messagingButton.Name = "messagingButton";
            this.messagingButton.Size = new System.Drawing.Size(185, 27);
            this.messagingButton.TabIndex = 1;
            this.messagingButton.Text = "Messenging App";
            this.messagingButton.UseVisualStyleBackColor = true;
            this.messagingButton.Click += new System.EventHandler(this.MessagingButton_Click);
            // 
            // contactButton
            // 
            this.contactButton.AutoSize = true;
            this.contactButton.Location = new System.Drawing.Point(3, 69);
            this.contactButton.Name = "contactButton";
            this.contactButton.Size = new System.Drawing.Size(185, 27);
            this.contactButton.TabIndex = 2;
            this.contactButton.Text = "Contact Tracing App";
            this.contactButton.UseVisualStyleBackColor = true;
            this.contactButton.Click += new System.EventHandler(this.ContactButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.closeButton.Location = new System.Drawing.Point(58, 102);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 27);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // appSelectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 506);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "appSelectWindow";
            this.Text = "Choose Application";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button tradingButton;
        private System.Windows.Forms.Button messagingButton;
        private System.Windows.Forms.Button contactButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label titleText;
    }
}