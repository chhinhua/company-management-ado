using company_management.DTO;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using company_management.BUS;
using company_management.Utilities;
using company_management.View;

// ReSharper disable All

namespace company_management.DAO
{
    public sealed class UserDao : IDisposable
    {
        private readonly DBConnection _dBConnection;
        private readonly Utils _utils;
        private readonly UserBus _userBus;
        private bool _disposed;

        public UserDao()
        {
            _userBus = new UserBus();
            _dBConnection = new DBConnection();
            _utils = new Utils();
        }

        public List<User> GetAllUser()
        {
            string query = "SELECT * FROM users";
            return _dBConnection.GetListObjectsByQuery<User>(query);
        }

        public void LoadData(DataGridView dataGridView, List<User> users)
        {
            dataGridView.ColumnCount = 7;
            dataGridView.Columns[0].Name = "Mã";
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Name = "Tên tài khoản";
            dataGridView.Columns[2].Name = "Họ và tên";
            dataGridView.Columns[3].Name = "Email";
            dataGridView.Columns[4].Name = "Số điện thoại";
            dataGridView.Columns[5].Name = "Địa chỉ";
            dataGridView.Columns[6].Name = "Chức vụ";
            dataGridView.Rows.Clear();

            foreach (var user in users)
            {
                string position = _userBus.GetPosition(user);
                dataGridView.Rows.Add(user.Id, user.Username, user.FullName, user.Email, user.PhoneNumber, user.Address,
                    position);
            }
        }

        public List<User> SearchUsers(string txtSearch)
        {
            string query = string.Format("SELECT * FROM users WHERE username like '%{0}%' OR fullName like '%{0}%' " +
                                         "OR email like '%{0}%' OR address like '%{0}%' OR phoneNumber like '%{0}%'",
                txtSearch);
            return _dBConnection.GetListObjectsByQuery<User>(query);
        }

        public List<User> GetAllLeader()
        {
            string query = "SELECT * FROM users WHERE users.idPosition = 2";
            return _dBConnection.GetListObjectsByQuery<User>(query);
        }

        public List<User> GetAllEmployee()
        {
            string query = "SELECT * FROM users WHERE users.idPosition = 3";
            return _dBConnection.GetListObjectsByQuery<User>(query).ToList();
        }

        public List<User> GetEmployeesByTeam(int teamId)
        {
            string query =
                string.Format(
                    "SELECT u.* FROM users u JOIN user_team ut ON u.id=ut.idUser WHERE ut.idTeam={0} AND u.idPosition=3",
                    teamId);
            return _dBConnection.GetListObjectsByQuery<User>(query).ToList();
        }

        public void AddUser(User user)
        {
            string sqlStr = string.Format(
                "INSERT INTO users(username, password, fullname, email, phoneNumber, address, idRole, idPosition)" +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
                user.Username, user.Password, user.FullName, user.Email, user.PhoneNumber, user.Address, user.IdRole,
                user.IdPosition);
            if (_dBConnection.ExecuteQuery(sqlStr))
            {
                _utils.Alert("Thêm user thành công", FormAlert.enmType.Success);

                var newUser = GetUserByUsername(user.Username);
                string query = string.Format("INSERT INTO user_salary(idUser, basicSalary, allowance, tax, insurance)" +
                                             "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    newUser.Id, Constants.BasicSalary, Constants.Allowance, Constants.Tax, Constants.Insurance);
                _dBConnection.ExecuteQuery(query);
            }
        }

        public void UpdateUser(User user)
        {
            string sqlStr = string.Format("UPDATE users SET " +
                                          "username = '{0}', fullname = '{1}', email = '{2}', phoneNumber = '{3}', address = '{4}', password = '{5}' WHERE id = '{6}'",
                user.Username, user.FullName, user.Email, user.PhoneNumber, user.Address, user.Password, user.Id);
            _dBConnection.ExecuteQuery(sqlStr);
        }

        public void UpdateUserPassword(User user)
        {
            string query = string.Format("UPDATE users SET password = '{0}' WHERE id = '{1}'", user.Password, user.Id);
            if (_dBConnection.ExecuteQuery(query))
            {
                _utils.Alert("Đổi mật khẩu thành công", FormAlert.enmType.Success);
            }
            else
            {
                _utils.Alert("Đổi không thành công", FormAlert.enmType.Error);
            }
        }

        public void DeleteUser(int id)
        {
            string query = string.Format("DELETE FROM user_salary WHERE idUser = '{0}'", id);
           
            if (_dBConnection.ExecuteQuery(query))
            {
                string sqlStr = string.Format("DELETE FROM users WHERE id = '{0}'", id);
                if (_dBConnection.ExecuteQuery(sqlStr))
                {
                    _utils.Alert("Xóa thành công", FormAlert.enmType.Success);
                }else
                {
                    _utils.Alert("Xóa không thành công", FormAlert.enmType.Error);
                }
            }
        }

        public User GetUserById(int id)
        {
            string query = string.Format("SELECT * FROM users WHERE users.id = {0}", id);
            return _dBConnection.GetObjectByQuery<User>(query);
        }

        public User GetUserByUsername(string username)
        {
            string query = string.Format("SELECT * FROM users WHERE users.username = '{0}'", username);
            return _dBConnection.GetObjectByQuery<User>(query);
        }

        public User GetUserByEmail(string email)
        {
            string query = string.Format("SELECT * FROM users WHERE users.email = '{0}'", email);
            return _dBConnection.GetObjectByQuery<User>(query);
        }

        public User GetLeaderByTeam(Team team)
        {
            string query = string.Format("SELECT * FROM users WHERE users.id = '{0}'", team.IdLeader);
            return _dBConnection.GetObjectByQuery<User>(query);
        }

        public User GetLeaderByUser(User user)
        {
            string query = string.Format("select * from users where id = (select idLeader " +
                                         "from teams " +
                                         "where id = (select idTeam " +
                                         "from user_team " +
                                         "where idUser = '{0}'))", user.Id);
            return _dBConnection.GetObjectByQuery<User>(query);
        }

        public List<User> GetListLeader()
        {
            List<User> users = GetAllUser();
            var listLeaderUsers = users.Where(u => u.IdPosition == 2).Distinct().ToList();
            return listLeaderUsers;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Giải phóng tài nguyên được sử dụng trong class
                    _dBConnection.Dispose();
                }

                _disposed = true;
            }
        }
    }
}