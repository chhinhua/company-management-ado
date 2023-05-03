using System;
using System.Linq;
using Guna.UI2.WinForms;
using System.Windows.Forms;
using System.Collections.Generic;
using company_management.DTO;
using company_management.DAO;
// ReSharper disable All

namespace company_management.BUS
{
    public class TaskBus
    {
        private readonly Lazy<TaskDao> _taskDao;
        private readonly Lazy<TeamDao> _teamDao;
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<UserBus> _userBus;
        private readonly Lazy<ProjectBus> _projectBus;
        private readonly Lazy<ProjectDao> _projectDao;
        private readonly Lazy<List<Task>> _listTask;

        public TaskBus()
        {
            _taskDao = new Lazy<TaskDao>(() => new TaskDao());
            _teamDao = new Lazy<TeamDao>(() => new TeamDao());
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _userBus = new Lazy<UserBus>(() => new UserBus());
            _projectBus = new Lazy<ProjectBus>(() => new ProjectBus());
            _projectDao = new Lazy<ProjectDao>(() => new ProjectDao());
            _listTask = new Lazy<List<Task>>(() => new List<Task>());
        }

        public void LoadDataGridview(List<Task> listTask, DataGridView dataGridView)
        {
            var userDao = _userDao.Value;
            var teamDao = _teamDao.Value;

            dataGridView.ColumnCount = 7;
            dataGridView.Columns[0].Name = "Mã";
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Name = "Người tạo";
            dataGridView.Columns[2].Name = "Tên task";
            dataGridView.Columns[2].Width = 275;
            dataGridView.Columns[3].Name = "Deadline";
            dataGridView.Columns[4].Name = "Tiến độ";
            dataGridView.Columns[5].Name = "Người được giao";
            dataGridView.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns[6].Name = "Team được giao";
            dataGridView.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Rows.Clear();

            // sort theo deadline tăng dần
            listTask.Sort((x, y) => DateTime.Compare(x.Deadline, y.Deadline));

            foreach (var t in listTask)
            {
                string creator = userDao.GetUserById(t.IdCreator).FullName;
                string assignee = userDao.GetUserById(t.IdAssignee).FullName;
                string team = teamDao.GetTeamById(t.IdTeam).Name;

                dataGridView.Rows.Add(t.Id, creator, t.TaskName, t.Deadline.ToString("dd/MM/yyyy"), t.Progress + " %",
                    assignee, team);
            }
        }

        public List<Task> GetListTaskByPosition()
        {
            int id = UserSession.LoggedInUser.Id;
            var tasks = _listTask.Value;
            var taskDao = _taskDao.Value;
            var userBus = _userBus.Value;
            ClearListTask(tasks);

            if (userBus.IsManager() || userBus.IsHumanResources())
            {
                tasks = taskDao.GetAllTask();
            }
            else if (userBus.IsLeader())
            {
                var team = _teamDao.Value.GetTeamByLeader(UserSession.LoggedInUser.Id);
                tasks.AddRange(taskDao.GetTasksByTeam(team.Id));
            }
            else
            {
                tasks = taskDao.GetTasksAssignedByCurrentUser(UserSession.LoggedInUser.Id);
            }

            return tasks;
        }

        public Task GetTaskFromFieldsForAdd(string taskName, string description, DateTimePicker deadline,
            ComboBox comboboxAssignee, ComboBox comboboxProject, string bonus)
        {
            var userBus = _userBus.Value;
            var teamDao = _teamDao.Value;
            var selectedProject = (Project)comboboxProject.SelectedItem;

            int idAssignee;
            int idCreator = idAssignee = UserSession.LoggedInUser.Id;
            int idTeam;
            int idProject = 0;
            if (selectedProject != null)
            {
                idProject = selectedProject.Id;
            }
            
            if (userBus.IsManager())
            {
                var selectedTeam = (Team)comboboxAssignee.SelectedItem;
                idAssignee = selectedTeam.IdLeader;
                idTeam = selectedTeam.Id; 
            }
            else if (userBus.IsLeader())
            {
                var selectedUser = (User)comboboxAssignee.SelectedItem;
                idAssignee = selectedUser.Id;
                idTeam = teamDao.GetTeamByLeader(idCreator).Id;
            }
            else
            {
                idTeam = teamDao.GetTeamByUser(UserSession.LoggedInUser.Id).Id;
            }
            
            decimal bonusValue = 0.00m;
            if (bonus != "")
                bonusValue = decimal.Parse(bonus);
            
            return new Task
            {
                TaskName = taskName,
                Description = description,
                IdCreator = idCreator,
                IdAssignee = idAssignee,
                IdTeam = idTeam,
                IdProject = idProject,
                Deadline = deadline.Value,
                Bonus = bonusValue,
                Progress = 0
            };
        }
        
        public Task GetTaskFromFieldsForUpdate(int taskId, Guna2TextBox taskName, Guna2TextBox description, DateTimePicker deadline,
            ComboBox comboboxAssignee, int progress, string bonus, ComboBox comboboxProject)
        {
            Task task = _taskDao.Value.GetTaskById(taskId);
            
            if (bonus != "")
            {
                task.Bonus = decimal.Parse(bonus);
            }
            
            var selectedProject = (Project)comboboxProject.SelectedItem;
            if (_userBus.Value.IsManager())
            {
                var selectedTeam = (Team)comboboxAssignee.SelectedItem;
                task.IdAssignee = selectedTeam.IdLeader;
                task.IdTeam = selectedTeam.Id;
                task.IdProject = selectedProject.Id;
            }
            else if (_userBus.Value.IsLeader())
            {
                var selectedUser = (User)comboboxAssignee.SelectedItem;
                task.IdAssignee = selectedUser.Id;
                task.IdProject = selectedProject.Id;
            }

            task.TaskName = taskName.Text;
            task.Description = description.Text;
            task.Deadline = deadline.Value;
            task.Progress = progress;
            
            return task;
        }

        public List<Task> SearchTasks(string txtSearch)
        {
            var taskDao = _taskDao.Value;
            return taskDao.SearchTasks(txtSearch);
        }

        public List<Task> GetTodoTasks() => GetListTaskByPosition().Where(t => t.Progress == 0).ToList();

        public List<Task> GetInprogressTasks() =>
            GetListTaskByPosition().Where(t => t.Progress > 0 && t.Progress < 100).ToList();

        public List<Task> GetDoneTasks() => GetListTaskByPosition().Where(t => t.Progress == 100).ToList();

        public List<Task> GetMyTasks() =>
            GetListTaskByPosition().Where(t => t.IdAssignee == UserSession.LoggedInUser.Id).ToList();

        public List<Task> GetMyCreatedTasks() =>
            GetListTaskByPosition().Where(t => t.IdCreator == UserSession.LoggedInUser.Id).ToList();

        public List<Task> SearchTasksByKeyword(Guna2DataGridView gridView, string keyword,
            int creator, int taskName, int assignee, int team)
        {
            var tasks = _listTask.Value;
            var taskDao = _taskDao.Value;
            ClearListTask(tasks);

            foreach (DataGridViewRow row in gridView.Rows)
            {
                bool found = false;
                foreach (int i in new int[] { creator, taskName, assignee, team })
                {
                    if (row.Cells[i].Value != null && row.Cells[i].Value.ToString().ToLower().Contains(keyword))
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    int taskId = Convert.ToInt32(row.Cells[0].Value);
                    Task task = taskDao.GetTaskById(taskId);
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        public void SelectComboBoxItemByValue(Guna2ComboBox comboBox, int value)
        {
            string valueString = value.ToString();
            int index = comboBox.FindStringExact(valueString);
            if (index != -1)
            {
                comboBox.SelectedIndex = index;
            }
        }

        public void SelectComboboxItemById<T>(ComboBox comboBox, int id) where T : class
        {
            // Lấy danh sách các item từ ComboBox
            List<T> items = comboBox.Items.Cast<T>().ToList();

            // Tìm kiếm item có ID trùng với tham số đầu vào và chọn nó
            T selectedItem = items.Find(item => (int)item.GetType().GetProperty("Id").GetValue(item) == id);
            if (selectedItem != null)
            {
                comboBox.SelectedItem = selectedItem;
            }
        }

        public void GetDataToCombobox(ComboBox assignees, ComboBox project)
        {
            _taskDao.Value.LoadUserToCombobox(assignees);
            _projectBus.Value.LoadProjectToCombobox(project);
        }

        private void ClearListTask(List<Task> listTask)
        {
            listTask.Clear();
            listTask.TrimExcess();
        }
    }
}