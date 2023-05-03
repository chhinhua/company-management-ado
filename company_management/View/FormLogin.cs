using System;
using System.Windows.Forms;
using company_management.DTO;
using company_management.DAO;
using company_management.Utilities;

namespace company_management.View
{
    public partial class FormLogin : Form
    {
        private readonly UserDao _userDao;
        private readonly Utils _utils;

        public FormLogin()
        {
            InitializeComponent();
            _userDao = new UserDao();
            _utils = new Utils();
        }

        private bool CheckDataInput()
        {
            if (string.IsNullOrEmpty(tbUsername.Text) || string.IsNullOrEmpty(tbPassword.Text))
            {
                _utils.Alert("Field required", FormAlert.enmType.Warning);
                MessageBox.Show(@"Tên đăng nhập hoặc mật khẩu không được để trống!");
                return false;
            }
            return true;
        }

        private bool ValidateLogin(string username, string password)
        {
            // Kiểm tra thông tin đăng nhập ở đây
            User loginUser = _userDao.GetUserByUsername(username);
            if (loginUser == null)
                return false;
            else if (loginUser.Password.Equals(password))
                return true;
            else
                return false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
             if (CheckDataInput())
            {
                string username = tbUsername.Text;
                string password = tbPassword.Text;

                // Kiểm tra thông tin đăng nhập
                if (ValidateLogin(username, password))
                {
                    User loginUser;

                    // Lấy thông tin người dùng đăng nhập
                    //loginUser = _userDao.GetUserByUsername("edalziell2"); // manager
                    //loginUser = _userDao.GetUserByUsername("chhinhua"); // manager


                    // Human Resources
                    loginUser = _userDao.GetUserByUsername(username); // Hr

                    // Team System Integration
                    //loginUser = _userDao.GetUserByUsername("ádfád"); // employee
                    // loginUser = _userDao.GetUserByUsername("mboardera"); // employee
                    // loginUser = _userDao.GetUserByUsername("dlaven9"); // employee
                    // loginUser = _userDao.GetUserByUsername("agors8"); // employee

                    // // Team Quality Assurance
                    //loginUser = _userDao.GetUserByUsername("abaldacchinob"); // leader
                    // loginUser = _userDao.GetUserByUsername("econstablee"); // employee
                    // loginUser = _userDao.GetUserByUsername("walimand"); // employee

                    // Team Database
                    //loginUser = _userDao.GetUserByUsername("linglishc"); // leader
                    // loginUser = _userDao.GetUserByUsername("eriknguyen"); // employee
                    // loginUser = _userDao.GetUserByUsername("esparsholtf"); // employee

                    //Team Development
                    //loginUser = _userDao.GetUserByUsername("tmccoish4"); // leader 
                    //loginUser = _userDao.GetUserByUsername("dsillyh"); // employee 
                    // loginUser = _userDao.GetUserByUsername("ipedlerg"); // employee 
                    //loginUser = _userDao.GetUserByUsername("ntute3"); // employee 

                    // Team Technical Support
                    //loginUser = _userDao.GetUserByUsername("wdionisi7"); // leader
                    //loginUser = _userDao.GetUserByUsername("newuser"); // employee
                    // loginUser = _userDao.GetUserByUsername("pbartulj"); // employee
                    // loginUser = _userDao.GetUserByUsername("taulti"); // employee
                    // loginUser = _userDao.GetUserByUsername("abrilleman6"); // employee
                    // loginUser = _userDao.GetUserByUsername("dmacard5"); // employee

                    // Lưu thông tin người dùng đăng nhập và chuyển sang form chính
                    UserSession.LoginUser(loginUser);

                    FormMain main = new FormMain();
                    this.Hide();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                    lbCannotLogin.Visible = true;
                }
            }
        }

        private void btnForgotpw_Click(object sender, EventArgs e)
        {
            FormVerifyEmail verifyEmail = new FormVerifyEmail();
            this.Hide();
            verifyEmail.Show();
        }
    }
}