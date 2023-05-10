using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;
// ReSharper disable All

namespace company_management.View
{
    public partial class FormVerifyEmail : Form
    {
        private readonly Lazy<UserDao> _userDao;
        readonly Random _random = new Random();
        private readonly Utils _utils;
        private int _otp;
        
        public static User VerifyUser { get; set; }
        
        public FormVerifyEmail()
        {
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _utils = new Utils();
            InitializeComponent();
        }
        
        private void btnContinue_Click(object sender, EventArgs e)
        {
            if(tbOtp.Text == _otp.ToString())
            {
                VerifyUser = _userDao.Value.GetUserByEmail(tbEmail.Text);
                FormPasswordChange passwordChange = new FormPasswordChange();
                this.Hide();
                passwordChange.Show();
            }
            else
            {
                MessageBox.Show("Mã OTP không chính xác!");
            }
        }

        private bool ValidateEmail()
        {
            if (!string.IsNullOrEmpty(tbEmail.Text))
            {
                User user = _userDao.Value.GetUserByEmail(tbEmail.Text);
                if (user != null)
                {
                    return true;
                }
                MessageBox.Show(@"Không tìm thấy địa chỉ email!");
            }
            else
            {
                MessageBox.Show(@"Email không được để trống!");
            }
            return false;
        } 
        
        private void btnSendOTP_Click(object sender, EventArgs e)
        {
            if (ValidateEmail())
            {
                try
                {
                    _otp = _random.Next(100000, 1000000);

                    var fromAddress = new MailAddress("phamtrungnghia232@gmail.com");
                    var toAddress = new MailAddress(tbEmail.Text);
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
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
