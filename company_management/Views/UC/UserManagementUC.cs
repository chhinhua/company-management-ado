﻿using System;
using System.Windows.Forms;
using company_management.Models;
using company_management.Controllers;
<<<<<<< HEAD
=======
using company_management.Views;
>>>>>>> hua

namespace company_management.Views
{
    public partial class UserManagementUC : UserControl
    {
        UserDAO userDAO = new UserDAO();

        public UserManagementUC()
        {
            InitializeComponent();
        }

        private void UserManagementUC_Load(object sender, EventArgs e)
<<<<<<< HEAD
        {
            userDAO.loadUser(dataGridView);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string selectedRoleString = comboBox_role.SelectedItem.ToString();
            UserRole selectedRole = (UserRole)Enum.Parse(typeof(UserRole), selectedRoleString);


            User user = new User(tbUsername.Text, tbPassword.Text, tbFullname.Text, 
                                 tbEmail.Text, tbAddress.Text, selectedRole);
            MessageBox.Show(user.ToString());
            
            //userDAO.addUser(user);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            userDAO.loadUser(dataGridView);
        }
=======
        {          
            userDAO.loadUser(dataGridView);
            addActionColumn();
        }

        private void addActionColumn()
        {
            // Tạo cột nút Delete
            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = "Delete";
            deleteColumn.Name = "Delete";
            deleteColumn.Text = "Delete";
            deleteColumn.UseColumnTextForButtonValue = true;   

            // Thêm cột nút Delete vào DataGridView
            dataGridView.Columns.Add(deleteColumn);
        }


        private void btnUpdatee_Click(object sender, EventArgs e)
        {

        }

        private void btbAdd_Click(object sender, EventArgs e)
        {
            /*string selectedRoleString = comboBox_role.SelectedItem.ToString();
           UserRole selectedRole = (UserRole)Enum.Parse(typeof(UserRole), selectedRoleString);

           User user = new User(tbUsername.Text, tbPassword.Text, tbFullname.Text, 
                                tbEmail.Text, tbPhone.Text, tbAddress.Text, selectedRole);          
           userDAO.addUser(user);
           userDAO.loadUser(dataGridView);*/
            AddUserForm addUserForm = new AddUserForm();
            addUserForm.ShowDialog();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem sự kiện click được kích hoạt từ cột Delete hay không
            if (dataGridView.Columns[e.ColumnIndex].Name == "Delete" && e.RowIndex >= 0)
            {
                // Hỏi người dùng có muốn xóa hay không
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Nếu người dùng chọn Yes thì xóa
                if (result == DialogResult.Yes)
                {
                    // Thực hiện logic xóa ở đây

                    // Ví dụ:
                    dataGridView.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Lấy chỉ số của dòng hiện tại
            string rowNumber = (e.RowIndex + 1).ToString();

            // Kiểm tra xem có cột STT trong DataGridView không
            if (dataGridView.RowHeadersVisible)
            {
                // Nếu có, thì hiển thị số thứ tự tương ứng với dòng
                dataGridView.Rows[e.RowIndex].HeaderCell.Value = rowNumber;
            }
            else
            {
                // Nếu không, thì thêm cột STT và hiển thị số thứ tự tương ứng với dòng
                dataGridView.Rows[e.RowIndex].Cells[0].Value = rowNumber;
            }

        }

    
>>>>>>> hua
    }
}
