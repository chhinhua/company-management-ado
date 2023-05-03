using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace company_management.View
{
    public partial class FormVerifyEmail : Form
    {
        readonly Random _random = new Random();
        private int _otp;
        
        public FormVerifyEmail()
        {
            InitializeComponent();
        }


        private void btnContinue_Click(object sender, EventArgs e)
        {
            if(tbOtp.Text == _otp.ToString())
            {
                FormPasswordChange passwordChange = new FormPasswordChange();
                this.Hide();
                passwordChange.Show();
            }
            else
            {
                MessageBox.Show("Mã OTP không chính xác!");
            }
        }

        private void btnSendOTP_Click(object sender, EventArgs e)
        {
            try
            {
                _otp = _random.Next(100000, 1000000);

                var fromAddress = new MailAddress("phamtrungnghia232@gmail.com");//mail dùng để gửi otp
                var toAddress = new MailAddress("trungnghiaazd@gmail.com");//mail nhận otp
                const string frompass = "mkfedlsgytikzfou";//mở xác thực 2 bước
                const string subject = "OTP code";
                string body = _otp.ToString();

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, frompass),
                    Timeout = 200000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                })
                {
                    smtp.Send(message);
                }
                MessageBox.Show("OPT đã được gửi qua mail");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
