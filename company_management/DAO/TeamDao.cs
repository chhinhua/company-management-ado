using company_management.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace company_management.DAO
{
    public class TeamDao
    {
        private readonly DBConnection _dBConnection;
        private readonly Lazy<string> _connString;
        private readonly Lazy<UserDao> _userDao;

        public TeamDao()
        {
            _dBConnection = new DBConnection();
            _connString = new Lazy<string>(() => Properties.Settings.Default.connStr);
            _userDao = new Lazy<UserDao>(() => new UserDao());
        }

        public Team GetTeamById(int id)
        {
            string query = $"SELECT * FROM teams WHERE id = {id}";
            return _dBConnection.GetObjectByQuery<Team>(query);
        }

        public Team GetTeamByLeader(int id)
        {
            string query = $"SELECT * FROM teams WHERE idLeader = {id}";
            return _dBConnection.GetObjectByQuery<Team>(query);
        }

        public Team GetTeamByTask(Task task)
        {
            Team team = GetTeamById(task.IdTeam);
            if (team != null)
            {
                return team;
            } 
            return null;
            //else
            //{
            //    return GetTeamByLeaderId(task.IdAssignee);
            //}
        }

        public List<User> GetUserInTeam(int idTeam)
        {
            string query = string.Format("select id, fullname, email, phoneNumber, address from users " +
                "where id in (select idUser from user_team " +
                            "where idTeam in (select id from teams " +
                            "                   where idLeader = {0}))", idTeam);
            return _dBConnection.GetListObjectsByQuery<User>(query);
        }

        public List<Team> GetAllTeam()
        {
            string query = "SELECT * FROM teams";
            return _dBConnection.GetListObjectsByQuery<Team>(query);
        }

        public List<Team> GetMyTeam(int userId)
        {
            string query = "SELECT * FROM teams WHERE id IN " +
                           $"(SELECT DISTINCT idTeam FROM user_team WHERE idUser = {userId})";
            return _dBConnection.GetListObjectsByQuery<Team>(query);
        }
        
        public void LoadData(DataGridView dataGridView)
        {
            dataGridView.ColumnCount = 4;

            dataGridView.Columns[0].Name = "Mã nhóm";
            dataGridView.Columns[0].Width = 40;

            dataGridView.Columns[1].Name = "Tên nhóm";
            dataGridView.Columns[1].Width = 300;
            dataGridView.Columns[2].Name = "Mô tả nhóm";
            dataGridView.Columns[3].Name = "Trưởng nhóm";
            dataGridView.Rows.Clear();

            // sử lý tên team
            var userDao = _userDao.Value;
            var listTeams = GetAllTeam();

            foreach (var t in listTeams)
            {
                dataGridView.Rows.Add(t.Id, t.Name, t.Description, userDao.GetLeaderByTeam(t).FullName);
            }
        }

        public List<Team> GetTeamsForLeader(int idLeader)
        {
            return GetAllTeam().Where(t => t.IdLeader == idLeader).ToList();
        }

        public List<Team> GetTeamsByUser(int idUser)
        {
            string query = string.Format("SELECT t.* FROM teams t " +
                 "JOIN user_team ut ON t.id = ut.idTeam " +
                 "WHERE ut.idUser = '{0}'", idUser);
            return _dBConnection.GetListObjectsByQuery<Team>(query);
        }
        
        public Team GetTeamByUser(int idUser)
        {
            string query = "SELECT DISTINCT t.* FROM teams t " + "JOIN user_team ut ON t.id = ut.idTeam " +
                           $"WHERE ut.idUser = '{idUser}'";
            return _dBConnection.GetObjectByQuery<Team>(query);
        }

        public int CountMembers(int idTeam)
        {
            int result;
        
            string query = $"SELECT COUNT(*) FROM user_team WHERE idTeam='{idTeam}'";

            var cnnString = _connString.Value;
            using (SqlConnection connection = new SqlConnection(cnnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    result = (int)command.ExecuteScalar();
                }
                connection.Close();
            }

            return result;
        }
    }
}
