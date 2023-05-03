using System;
using System.Collections.Generic;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;
// ReSharper disable All

namespace company_management.View.UC
{
    public partial class UcProject : UserControl
    {
        private readonly Lazy<Utils> _utils;
        private readonly Lazy<ProjectBus> _projectBus;
        private readonly Lazy<ProjectDao> _projectDao;
        private readonly Lazy<List<Project>> _listProject;
        private readonly Lazy<TaskDao> _taskDao;
        private int _selectedId;

        public UcProject()
        {
            InitializeComponent();
            _utils = new Lazy<Utils>(() => new Utils());
            _listProject = new Lazy<List<Project>>(() => new List<Project>());
            _projectBus = new Lazy<ProjectBus>(() => new ProjectBus());
            _projectDao = new Lazy<ProjectDao>(() => new ProjectDao());
            _taskDao = new Lazy<TaskDao>(() => new TaskDao());
        }

        private void UC_Project_Load(object sender, EventArgs e)
        {
            LoadData(GetData());
        }

        private List<Project> GetData()
        {
            return _projectBus.Value.GetListProjectByPosition();
        }

        private void LoadData(List<Project> projects)
        {
            LoadDataGridview(projects);
            LoadProgressChart(projects);
            CheckAddButtonStatus();
        }

        private void LoadProgressChart(List<Project> projects)
        {
            var util = _utils.Value;
            util.LoadProgressChart(chart_projectProgress, label_todoProject, label_inprogressProject, label_doneProject, projects);
        }
        
        private void LoadDataGridview(List<Project> listProjects)
        {
            var projectBus = _projectBus.Value;
            projectBus.LoadDataGridview(listProjects, dataGridView_Project);
        }

        private void CheckAddButtonStatus()
        {
            var util = _utils.Value;
            util.CheckManagerIsVisibleStatus(buttonAdd);
            util.CheckManagerIsVisibleStatus(button_edit);
            util.CheckManagerIsVisibleStatus(button_remove);
            util.CheckEmployeeNotVisibleStatus(button_edit);
        }

        private void dataGridView_Project_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                object value = dataGridView_Project.Rows[e.RowIndex].Cells[0].Value;
                if (value != DBNull.Value)
                {
                    _selectedId = Convert.ToInt32(value);
                }
            }
        }
        
        private void combobox_progressFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var projectBus = _projectBus.Value;
            var projects = _listProject.Value;
            projects.Clear();
            
            int selectedIndex = combobox_progressFilter.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    projects = projectBus.GetListProjectByPosition();
                    break;
                case 1:
                    projects = projectBus.GetTodoProjects();
                    break;
                case 2:
                    projects = projectBus.GetInprogressProjects();
                    break;
                default:
                    projects = projectBus.GetDoneProjects();
                    break;
            }

            LoadDataGridview(projects);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData(GetData());
        }

        private void btnViewOrUpdate_Click_1(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                FormViewOrUpdateProject formProject = new FormViewOrUpdateProject();
                formProject.SetProjectId(_selectedId);
                formProject.Show();
            }
            else MessageBox.Show("Select a project to view", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {
            FormAddProject addProject = new FormAddProject();
            addProject.Show();
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                FormViewOrUpdateProject formProject = new FormViewOrUpdateProject();
                formProject.SetProjectId(_selectedId);
                formProject.Show();
            }
            else MessageBox.Show("Select a project to view", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                DialogResult result = MessageBox.Show("Xác nhận xóa dự án? CHÚ Ý: nếu bạn quyết định xóa dự án thì các task liên quan đến dự án đều bị xóa theo!", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DialogResult result2= MessageBox.Show("Xác nhận xóa dự án?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result2 == DialogResult.Yes)
                    {
                        var taskDao = _taskDao.Value;
                        taskDao.DeleteTasksByProject(_selectedId);
                    
                        var projectDao = _projectDao.Value;
                        projectDao.DeleteProject(_selectedId);
                    
                        LoadData(GetData());
                    }
                }
            }
            else MessageBox.Show("Vui lòng chọn dự án!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
        }
    }
}