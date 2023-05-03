using company_management.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using company_management.View;
using company_management.Utilities;
// ReSharper disable All

namespace company_management.DAO
{
    public class ProjectDao
    {
        private readonly DBConnection _dBConnection;
        private readonly Lazy<TeamDao> _teamDao;
        private readonly Utils _utils;

        public ProjectDao()
        {
            _dBConnection = new DBConnection();
            _teamDao = new Lazy<TeamDao>(() => new TeamDao());
            _utils = new Utils();
        }

        public void AddProject(Project project)
        {
            string query = string.Format("INSERT INTO project(idCreator, idAssignee, name, description, " +
                                         "startDate, endDate, progress, idTeam, bonus)" +
                                         "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                project.IdCreator, project.IdAssignee, project.Name, project.Description,
                project.StartDate, project.EndDate, project.Progress, project.IdTeam, project.Bonus);
            try
            {
                _dBConnection.ExecuteQuery(query);
                _utils.Alert("Add successful", FormAlert.enmType.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _utils.Alert("Add failed", FormAlert.enmType.Error);
            }
        }

        public void UpdateProject(Project project)
        {
            string query = string.Format("UPDATE project SET idCreator='{0}', idAssignee='{1}', name='{2}'," +
                                         "description='{3}', startDate='{4}', endDate='{5}', progress='{6}', idTeam='{7}', bonus='{8}' WHERE id='{9}'",
                project.IdCreator, project.IdAssignee, project.Name, project.Description,
                project.StartDate, project.EndDate, project.Progress, project.IdTeam, project.Bonus, project.Id);
            if (_dBConnection.ExecuteQuery(query))
            {
                _utils.Alert("Updated successful", FormAlert.enmType.Success);
            }
            else
            {
                _utils.Alert("Update failed", FormAlert.enmType.Error);
            }
        }

        public void DeleteProject(int id)
        {
            string query = string.Format("DELETE FROM project WHERE id = {0}", id);
            if (_dBConnection.ExecuteQuery(query))
            {
                _utils.Alert("Deleted project successful", FormAlert.enmType.Success);
            }
            else
            {
                _utils.Alert("Deleted project failed", FormAlert.enmType.Error);
            }
        }
        
        public void LoadTeamToCombobox(ComboBox comboBox)
        {
            var teamDao = _teamDao.Value;
            List<Team> teams;

            // Hiển thị danh sách team cho quản lý chọn
            teams = new List<Team>();
            teams.AddRange(teamDao.GetAllTeam());

            comboBox.Items.AddRange(teams.ToArray());
            comboBox.DisplayMember = "name";

            comboBox.ValueMember = "id";
            comboBox.SelectedIndex = 0;
        }

        public List<Project> GetAllProject()
        {
            string query = string.Format("SELECT * FROM project");
            return _dBConnection.GetListObjectsByQuery<Project>(query);
        }

        public List<Project> GetProjectsCreatedByCurrentUser(int idCreator)
        {
            return GetAllProject().Where(t => t.IdCreator == idCreator).ToList();
        }
        
        public List<Project> GetProjectsAssignedByCurrentUser(int idAssignee)
        {
            return GetAllProject().Where(t => t.IdAssignee == idAssignee).ToList();
        }
        
        public List<Project> GetMyProjects()
        {
            var team = _teamDao.Value.GetTeamByUser(UserSession.LoggedInUser.Id);
            return GetAllProject().Where(t => t.IdTeam == team.Id).ToList();
        }

        public Project GetProjectById(int id)
        {
            string query = $"SELECT * FROM project WHERE id = {id}";
            return _dBConnection.GetObjectByQuery<Project>(query);
        }
        
        public Project GetProjectByTeam(int idTeam)
        {
            string query = $"SELECT DISTINCT FROM project WHERE idTeam = {idTeam}";
            return _dBConnection.GetObjectByQuery<Project>(query);
        }
    }
}