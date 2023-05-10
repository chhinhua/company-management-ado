using System;
using System.Windows.Forms;
using company_management.DAO;
// ReSharper disable All

namespace company_management.View
{
    public partial class FormPasswordChange : Form
    {
        private readonly Lazy<UserDao> _userDao;
        public FormPasswordChange()
        {
            _userDao = new Lazy<UserDao>(() => new UserDao());
            InitializeComponent();
        }

        private bool CheckPasswordInput()
        {
            if (!string.IsNullOrEmpty(tbNewpass.Text) && !string.IsNullOrEmpty(tbConfirmpass.Text))
            {
                if (tbNewpass.Text.Equals(tbConfirmpass.Text))
                {
                    return true;
                }
                MessageBox.Show("Mật khẩu nhập lại không khớp. Vui lòng thử lại!");
            }
            MessageBox.Show("Không được để trống!");

            return false;
        }
        
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (CheckPasswordInput())
            {
                var user = FormVerifyEmail.VerifyUser;
                user.Password = tbConfirmpass.Text;
                _userDao.Value.UpdateUserPassword(user);
                
                FormLogin login = new FormLogin();
                this.Hide();
                login.Show();
            }
        }
    }
}
