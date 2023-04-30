﻿using company_management.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using company_management.Utilities;
using company_management.View;
using company_management.View.UC;

namespace company_management.DAO
{
    public sealed class TaskDao : IDisposable
    {
        private readonly DBConnection _dBConnection;
        private readonly Lazy<string> _connString;
        private Lazy<List<Task>> _listTask;
        private readonly Lazy<TeamDao> _teamDao;
        private readonly Lazy<UserDao> _userDao;
        private bool _disposed = false;
        private Utils _utils;
        

        public TaskDao()
        {
            _dBConnection = new DBConnection();
            _connString = new Lazy<string>(() => Properties.Settings.Default.connStr);         
            _listTask = new Lazy<List<Task>>(() => new List<Task>());
            _teamDao = new Lazy<TeamDao>(() => new TeamDao());
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _utils = new Utils();
        }

        public void LoadUserToCombobox(ComboBox comboBox)
        {
            List<Team> teams;
            List<User> users;
            var teamDao = _teamDao.Value;
            var userDao = _userDao.Value;

            if (UserSession.LoggedInUser != null)
            {
                if (UserSession.LoggedInUser.IdPosition == 1) // IdPosition = 1 (manager)
                {
                    teams = new List<Team>();
                    teams.AddRange(teamDao.GetAllTeam());

                    comboBox.Items.AddRange(teams.ToArray());
                    comboBox.DisplayMember = "name";
                }
                else if (UserSession.LoggedInUser.IdPosition == 2) // IdPosition = 2 (leader)
                {
                    Team team = teamDao.GetTeamByLeader(UserSession.LoggedInUser.Id);
                    users = new List<User>();
                    users.AddRange(userDao.GetEmployeesByTeam(team.Id));

                    comboBox.Items.AddRange(users.ToArray());
                    comboBox.DisplayMember = "fullName";
                }
                else // (employee)
                { 
                    // Không có quyền truy cập
                    comboBox.Enabled = false;
                    return;
                }

                comboBox.ValueMember = "id";
                if (UcTask.ViewTask != null)
                {
                    comboBox.SelectedValue = UcTask.ViewTask.IdAssignee;
                }              
            }

        }

        public void AddTask(Task task)
        {
            string query = string.Format("INSERT INTO task(idCreator, idAssignee, taskName, description, deadline, progress, idTeam, bonus, idProject)" +
                   "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                   task.IdCreator, task.IdAssignee, task.TaskName, task.Description, task.Deadline, task.Progress, task.IdTeam, task.Bonus, task.IdProject);
            try
            {
                _dBConnection.ExecuteQuery(query);
                _utils.Alert("Added successful", FormAlert.enmType.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _utils.Alert("Added failed", FormAlert.enmType.Error);
            }
        }

        public void UpdateTask(Task updateTask)
        {
            string query = string.Format("UPDATE task SET " +
                                         "idAssignee = '{0}', taskName = '{1}', description = '{2}', deadline = '{3}', progress = '{4}', idTeam = '{5}', bonus = '{6}', idProject = '{7}' WHERE id = '{8}'",
                   updateTask.IdAssignee, updateTask.TaskName, updateTask.Description, updateTask.Deadline, updateTask.Progress, updateTask.IdTeam, updateTask.Bonus, updateTask.IdProject, updateTask.Id);
            try
            {
                _dBConnection.ExecuteQuery(query);
                _utils.Alert("Updated successful", FormAlert.enmType.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _utils.Alert("Updated failed", FormAlert.enmType.Error);
            }
        }

        public void DeleteTask(int id)
        {
            string query = string.Format("DELETE FROM task WHERE id = {0}", id);
            try
            {
                _dBConnection.ExecuteQuery(query);
                _utils.Alert("Deleted successful", FormAlert.enmType.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _utils.Alert("Deleted failed", FormAlert.enmType.Error);
            }
        }

        public Task GetTaskById(int id)
        {
            string query = string.Format("SELECT * FROM task WHERE id = {0}", id);
            return _dBConnection.GetObjectByQuery<Task>(query);
        }

        public TaskStatusPercentage GetTaskStatusPercentage(List<Task> tasks)
        {
            TaskStatusPercentage taskStatus = new TaskStatusPercentage(0, 0, 0);

            if (tasks.Count > 0)
            {
                double totalTasks = tasks.Count;
                double todoCount = tasks.Count(task => task.Progress == 0);
                double inprogressCount = tasks.Count(task => task.Progress > 0 && task.Progress < 100);
                double doneCount = tasks.Count(task => task.Progress == 100);

                taskStatus.TodoPercent = (todoCount / totalTasks) * 100;
                taskStatus.InprogressPercent = (inprogressCount / totalTasks) * 100;
                taskStatus.DonePercent = (doneCount / totalTasks) * 100;
            }

            return taskStatus;
        }

        private List<Task> GetAllTask()
        {
            string query = string.Format("SELECT * FROM task");
            return _dBConnection.GetListObjectsByQuery<Task>(query);
        }

        public List<Task> GetTasksCreatedByCurrentUser(int idCreator)
        {
            return GetAllTask().Where(t => t.IdCreator == idCreator).ToList();
        }

        public List<Task> GetTasksAssignedByCurrentUser(int idAssignee)
        {
            return GetAllTask().Where(t => t.IdAssignee == idAssignee).ToList();
        }

        public List<Task> SearchTasks(string txtSearch)
        {
            string query = string.Format("SELECT t.* FROM task t " +
                "INNER JOIN teams tm ON t.idTeam = tm.id " +
                "INNER JOIN users u ON(t.idCreator = u.id OR t.idAssignee = u.id) " +
                "WHERE t.taskName LIKE '%{0}%' " +
                "OR t.description LIKE '%{0}%' " +
                "OR tm.name LIKE '%{0}%' " +
                "OR u.username LIKE '%{0}%' ", txtSearch);
            return _dBConnection.GetListObjectsByQuery<Task>(query);
        }

        public decimal CalculateBonusForEmployee(int idUser, DateTime fromDate, DateTime toDate)
        {
            decimal totalBonus = 0;

            var cnnString = _connString.Value;
            using (SqlConnection connection = new SqlConnection(cnnString))
            {
                connection.Open();

                // Tìm tất cả các task được giao cho nhân viên đó với deadline trong khoảng thời gian từ fromDate đến toDate
                string query = "SELECT SUM(bonus) FROM task " +
                               "WHERE idAssignee = @idUser " +
                               "AND deadline >= @fromDate AND deadline <= @toDate " +
                               "AND progress = 100";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUser", idUser);
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);

                    // Tính tổng tiền bonus của tất cả các task được giao cho nhân viên đó
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        totalBonus = Convert.ToDecimal(result);
                    }
                }
            }

            return totalBonus;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _dBConnection.Dispose();
            }

            _disposed = true;
        }

        ~TaskDao()
        {
            Dispose(false);
        }
    }
}
