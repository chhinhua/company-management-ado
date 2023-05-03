using System;
using System.Collections.Generic;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;

namespace company_management.View.UC
{
    public partial class UcUser : UserControl
    {
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<Utils> _utils;
        private int _selectedUserId;
        private User _user;

        public UcUser()
        {
            InitializeComponent();
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _utils = new Lazy<Utils>(() => new Utils());
            _user = new User();
        }

        private void UcUser_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Alert(string msg, FormAlert.enmType type)
        {
            FormAlert frm = new FormAlert();
            frm.showAlert(msg, type);
        }

        private void LoadData()
        {
            LoadDataGridview();
            CheckControlStatus();
        }

        private void LoadDataGridview()
        {
            var userDao = _userDao.Value;
            var users = userDao.GetAllUser();
            userDao.LoadData(dataGridView_User, users);
        }

        private User GetUserFromTextBox()
        {
            return new User(txtbox_username.Text, Constants.DEFAULT_INIT_PASSWORD, txtbox_fullname.Text,
                txtbox_email.Text, txtbox_phoneNumber.Text, txtbox_address.Text, Constants.DEFAULT_USER_ROLE_ID);
        }

        private User GetUserEditedUser()
        {
            _user.Username = txtbox_username.Text;
            _user.FullName = txtbox_fullname.Text;
            _user.Email = txtbox_email.Text;
            _user.PhoneNumber = txtbox_phoneNumber.Text;
            _user.Address = txtbox_address.Text;

            return _user;
        }

        private bool CheckDataInput()
        {
            if (CheckEmptyInput())
            {
                var util = _utils.Value;
                if (util.IsValidEmail(txtbox_email.Text))
                {
                    if (util.IsPhoneNumber(txtbox_phoneNumber.Text))
                    {
                        return true;
                    }
                    else
                    {
                        this.Alert("Invalid phoneNumber!", FormAlert.enmType.Warning);
                        MessageBox.Show(@"Invalid phone number!");
                    }
                }
                else
                {
                    this.Alert("Invalid email!", FormAlert.enmType.Warning);
                    MessageBox.Show(@"Invalid email!");
                }
            }
            else
            {
                this.Alert("Field required!", FormAlert.enmType.Warning);
                MessageBox.Show(@"Required fields Empty. Please fill in all fields!");
            }

            return false;
        }

        private bool CheckEmptyInput()
        {
            if (string.IsNullOrEmpty(txtbox_username.Text) || string.IsNullOrEmpty(txtbox_fullname.Text) ||
                string.IsNullOrEmpty(txtbox_email.Text) || string.IsNullOrEmpty(txtbox_phoneNumber.Text) ||
                string.IsNullOrEmpty(txtbox_address.Text))
            {
                return false;
            }

            return true;
        }

        private void ClearAll()
        {
            txtbox_username.Clear();
            txtbox_fullname.Clear();
            txtbox_email.Clear();
            txtbox_phoneNumber.Clear();
            txtbox_address.Clear();
            btn_Save.Enabled = false;
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (_selectedUserId != 0)
            {
                DialogResult result = MessageBox.Show(@"Delete user?", @"Confirm", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var userDao = _userDao.Value;
                    userDao.DeleteUser(_selectedUserId);
                    LoadData();
                }
            }
            else MessageBox.Show(@"User not selected!", @"Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;

            var userDao = _userDao.Value;
            List<User> listUser = userDao.SearchUsers(keyword);
            userDao.LoadData(dataGridView_User, listUser);
        }

        private void dataGridView_User_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var userDao = _userDao.Value;

                DataGridViewRow selectedRow = dataGridView_User.Rows[e.RowIndex];
                _selectedUserId = (int)selectedRow.Cells[0].Value;
                _user = userDao.GetUserById(_selectedUserId);
                btn_Save.Enabled = false;

                object value = dataGridView_User.Rows[e.RowIndex].Cells[0].Value;
                if (value != DBNull.Value)
                {
                    DataGridViewRow row = dataGridView_User.Rows[e.RowIndex];

                    // Hiển thị dữ liệu lên các đối tượng TextBox
                    txtbox_username.Text = row.Cells[1].Value.ToString();
                    txtbox_fullname.Text = row.Cells[2].Value.ToString();
                    txtbox_email.Text = row.Cells[3].Value.ToString();
                    txtbox_phoneNumber.Text = row.Cells[4].Value.ToString();
                    txtbox_address.Text = row.Cells[5].Value.ToString();
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (CheckDataInput())
            {
                var userDao = _userDao.Value;
                if (_user == null)
                {
                    userDao.AddUser(GetUserFromTextBox());
                    this.Alert("Add successful", FormAlert.enmType.Success);
                }
                else
                {
                    userDao.UpdateUser(GetUserEditedUser());
                    this.Alert("Update successful", FormAlert.enmType.Success);
                }

                LoadData();
            }
        }

        private void CheckSaveButton()
        {
            if (_user != null)
            {
                if (_user.Id != 0)
                {
                    // Và đã có sự thay đổi so với dữ liệu ban đầu thì enable nút Lưu
                    if (txtbox_username.Text != _user.Username
                        || txtbox_email.Text != _user.Email
                        || txtbox_phoneNumber.Text != _user.PhoneNumber
                        || txtbox_fullname.Text != _user.FullName
                        || txtbox_address.Text != _user.Address)
                    {
                        btn_Save.Enabled = true;
                    }
                    else
                    {
                        btn_Save.Enabled = false;
                    }
                }
                else if (txtbox_username.Text != ""
                         || txtbox_email.Text != ""
                         || txtbox_phoneNumber.Text != ""
                         || txtbox_fullname.Text != ""
                         || txtbox_address.Text != "")
                {
                    btn_Save.Enabled = true;
                }
                else
                {
                    btn_Save.Enabled = false;
                }
            }
            else
            {
                if (txtbox_username.Text != ""
                    || txtbox_email.Text != ""
                    || txtbox_phoneNumber.Text != ""
                    || txtbox_fullname.Text != ""
                    || txtbox_address.Text != "")
                {
                    btn_Save.Enabled = true;
                }
                else
                {
                    btn_Save.Enabled = false;
                }
            }
        }

        private void CheckControlStatus()
        {
            var util = _utils.Value;
            util.CheckManagerIsVisibleStatus(btnAdd);
            util.CheckManagerIsVisibleStatus(btnDelete);
            util.CheckManagerIsVisibleStatus(btn_Save);
        }

        // TextBox changed and leaved event
        private void txtbox_email_TextChanged(object sender, EventArgs e) => CheckSaveButton();

        private void txtbox_username_TextChanged(object sender, EventArgs e) => CheckSaveButton();

        private void txtbox_fullname_TextChanged(object sender, EventArgs e) => CheckSaveButton();

        private void txtbox_phoneNumber_TextChanged(object sender, EventArgs e) => CheckSaveButton();

        private void txtbox_address_TextChanged(object sender, EventArgs e) => CheckSaveButton();
    }
}