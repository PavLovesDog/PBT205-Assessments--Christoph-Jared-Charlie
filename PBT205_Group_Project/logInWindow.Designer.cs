
namespace PBT205_Group_Project
{
    partial class logInWindow
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
            this.btnLogIn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.lblRetort = new System.Windows.Forms.Label();
            this.lblTitleText = new System.Windows.Forms.Label();
            this.lblUserReply = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLogIn
            // 
            this.btnLogIn.Location = new System.Drawing.Point(329, 386);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(123, 63);
            this.btnLogIn.TabIndex = 0;
            this.btnLogIn.Text = "Log In";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(267, 285);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(279, 26);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(267, 341);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(278, 26);
            this.textBox2.TabIndex = 2;
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(188, 288);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(78, 19);
            this.userLabel.TabIndex = 3;
            this.userLabel.Text = "Username:";
            this.userLabel.Click += new System.EventHandler(this.UserLabel_Click);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(188, 345);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(73, 19);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password:";
            // 
            // lblRetort
            // 
            this.lblRetort.AutoSize = true;
            this.lblRetort.Location = new System.Drawing.Point(368, 453);
            this.lblRetort.Name = "lblRetort";
            this.lblRetort.Size = new System.Drawing.Size(0, 19);
            this.lblRetort.TabIndex = 5;
            // 
            // lblTitleText
            // 
            this.lblTitleText.AutoSize = true;
            this.lblTitleText.Font = new System.Drawing.Font("Comic Sans MS", 25.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleText.Location = new System.Drawing.Point(266, 118);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(324, 61);
            this.lblTitleText.TabIndex = 6;
            this.lblTitleText.Text = "Project A.N.I";
            this.lblTitleText.Click += new System.EventHandler(this.LblTitleText_Click);
            //
            // lblUserReply
            // 
            this.lblUserReply.AutoSize = true;
            this.lblUserReply.Location = new System.Drawing.Point(345, 263);
            this.lblUserReply.Name = "lblUserReply";
            this.lblUserReply.Size = new System.Drawing.Size(0, 19);
            this.lblUserReply.TabIndex = 7;
            // 
            // logInWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 539);
            this.Controls.Add(this.lblUserReply);
            this.Controls.Add(this.lblTitleText);
            this.Controls.Add(this.lblRetort);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnLogIn);
            this.Font = new System.Drawing.Font("Comic Sans MS", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "logInWindow";
            this.Text = "A.N.I";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label lblRetort;
        private System.Windows.Forms.Label lblTitleText;
        private System.Windows.Forms.Label lblUserReply;
    }
}

