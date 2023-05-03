namespace company_management.View
{
    partial class FormVerifyEmail
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnContinue = new Guna.UI2.WinForms.Guna2Button();
            this.tbEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSendOTP = new Guna.UI2.WinForms.Guna2Button();
            this.tbOtp = new Guna.UI2.WinForms.Guna2TextBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(135)))), ((int)(((byte)(48)))));
            this.label3.Location = new System.Drawing.Point(97, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(406, 40);
            this.label3.TabIndex = 32;
            this.label3.Text = "FORGOT PASSWORD";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(135)))), ((int)(((byte)(48)))));
            this.label2.Location = new System.Drawing.Point(48, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 26);
            this.label2.TabIndex = 31;
            this.label2.Text = "Enter your email address";
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.White;
            this.btnContinue.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(135)))), ((int)(((byte)(48)))));
            this.btnContinue.BorderRadius = 15;
            this.btnContinue.BorderThickness = 2;
            this.btnContinue.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnContinue.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnContinue.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnContinue.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnContinue.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(135)))), ((int)(((byte)(48)))));
            this.btnContinue.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.Location = new System.Drawing.Point(59, 382);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(480, 73);
            this.btnContinue.TabIndex = 33;
            this.btnContinue.Text = "CONTINUE";
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // tbEmail
            // 
            this.tbEmail.BorderColor = System.Drawing.Color.Black;
            this.tbEmail.BorderRadius = 10;
            this.tbEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbEmail.DefaultText = "";
            this.tbEmail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tbEmail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tbEmail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbEmail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbEmail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbEmail.Location = new System.Drawing.Point(59, 180);
            this.tbEmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.PasswordChar = '\0';
            this.tbEmail.PlaceholderText = "";
            this.tbEmail.SelectedText = "";
            this.tbEmail.Size = new System.Drawing.Size(480, 56);
            this.tbEmail.TabIndex = 36;
            // 
            // btnSendOTP
            // 
            this.btnSendOTP.BorderRadius = 6;
            this.btnSendOTP.BorderThickness = 1;
            this.btnSendOTP.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSendOTP.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSendOTP.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSendOTP.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSendOTP.FillColor = System.Drawing.Color.White;
            this.btnSendOTP.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendOTP.ForeColor = System.Drawing.Color.Black;
            this.btnSendOTP.Location = new System.Drawing.Point(61, 284);
            this.btnSendOTP.Name = "btnSendOTP";
            this.btnSendOTP.Size = new System.Drawing.Size(146, 45);
            this.btnSendOTP.TabIndex = 37;
            this.btnSendOTP.Text = "Send OTP";
            this.btnSendOTP.Click += new System.EventHandler(this.btnSendOTP_Click);
            // 
            // tbOtp
            // 
            this.tbOtp.BorderColor = System.Drawing.Color.Black;
            this.tbOtp.BorderRadius = 6;
            this.tbOtp.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbOtp.DefaultText = "";
            this.tbOtp.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tbOtp.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tbOtp.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbOtp.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbOtp.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbOtp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbOtp.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbOtp.Location = new System.Drawing.Point(229, 286);
            this.tbOtp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbOtp.Name = "tbOtp";
            this.tbOtp.PasswordChar = '\0';
            this.tbOtp.PlaceholderText = "";
            this.tbOtp.SelectedText = "";
            this.tbOtp.Size = new System.Drawing.Size(306, 41);
            this.tbOtp.TabIndex = 38;
            // 
            // VerifyEmailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(599, 498);
            this.Controls.Add(this.tbOtp);
            this.Controls.Add(this.btnSendOTP);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VerifyEmailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VerifyEmailForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button btnContinue;
        private Guna.UI2.WinForms.Guna2TextBox tbEmail;
        private Guna.UI2.WinForms.Guna2Button btnSendOTP;
        private Guna.UI2.WinForms.Guna2TextBox tbOtp;
    }
}