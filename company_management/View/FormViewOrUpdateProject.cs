using System;
using System.Globalization;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;
// ReSharper disable All

namespace company_management.View
{
    public partial class FormViewOrUpdateProject : Form
    {
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<TeamDao> _teamDao;
        private readonly Lazy<ImageDao> _imageDao;
        private readonly Lazy<TaskBus> _taskBus;
        private readonly Lazy<ProjectDao> _projectDao;
        private readonly Utils _utils;
        private int _projectId;

        public FormViewOrUpdateProject()
        {
            _utils = new Utils();
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _teamDao = new Lazy<TeamDao>(() => new TeamDao());
            _imageDao = new Lazy<ImageDao>(() => new ImageDao());
            _taskBus = new Lazy<TaskBus>(() => new TaskBus());
            _projectDao = new Lazy<ProjectDao>(() => new ProjectDao());
            InitializeComponent();
        }

        private void LoadData()
        {
            CheckControlStatusByPosition();
            BindingProjectToFields();
        }

        private void CheckControlStatusByPosition()
        {
            CheckControlStatusForEmployee();
            CheckControlStatusForLeader();
        }

        private void CheckControlStatusForEmployee()
        {
            _utils.CheckEmployeeIsReadOnlyStatus(textbox_projectName);
            _utils.CheckEmployeeIsReadOnlyStatus(textbox_Desciption);
            _utils.CheckEmployeeIsReadOnlyStatus(textBox_projectBonus);
            _utils.CheckEmployeeNotEnableStatus(combobox2_progress);
            _utils.CheckEmployeeNotEnableStatus(dateTime_startDate2);
            _utils.CheckEmployeeNotEnableStatus(dateTime_endDate2);
            _utils.CheckEmployeeNotVisibleStatus(button_save);
        }

        private void CheckControlStatusForLeader()
        {
            _utils.CheckLeaderIsReadOnlyStatus(textbox_projectName);
            _utils.CheckLeaderIsReadOnlyStatus(textbox_Desciption);
            _utils.CheckLeaderIsReadOnlyStatus(textBox_projectBonus);
            _utils.CheckLeaderNotEnableStatus(dateTime_startDate2);
            _utils.CheckLeaderNotEnableStatus(dateTime_endDate2);
        }

        private void combobox_progress_SelectedIndexChanged(object sender, EventArgs e)
        {
            int progress = Convert.ToInt32(combobox_progress.SelectedItem);
            circleProgressBar.Value = progress;
            progressValue.Text = progress.ToString() + @"%";
        }

        public void SetProjectId(int id) => _projectId = id;

        private void BindingProjectToFields()
        {
            var projectDao = _projectDao.Value;
            Project project = projectDao.GetProjectById(_projectId);

            var userDao = _userDao.Value;
            User assigneeUser = userDao.GetUserById(project.IdAssignee);
            
            var teamDao = _teamDao.Value;
            Team assigneeTeam = teamDao.GetTeamById(project.IdTeam);
            
            var imageDao = _imageDao.Value;
            imageDao.ShowImageInPictureBox(assigneeUser.Avatar, picturebox_userAvatar2);
            imageDao.ShowImageInPictureBox(assigneeTeam.Avatar, picturebox_teamAvatar2);

            textbox_projectName.Text = project.Name;
            textbox_Desciption.Text = project.Description;
            textBox_projectBonus.Text = project.Bonus.ToString(CultureInfo.InvariantCulture);
            
            label2_assigneedTeam.Text = assigneeTeam.Name;
            label2_assignedPerson.Text = assigneeUser.FullName;
            
            circleProgressBar2.Value = project.Progress;
            progressValue2.Text = project.Progress.ToString("0.'%'");

            var taskBus = _taskBus.Value;
            taskBus.SelectComboBoxItemByValue(combobox2_progress, project.Progress);
            try
            {
                dateTime_startDate2.Value = project.StartDate;
                dateTime_endDate2.Value = project.EndDate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private Project GetProjectForUpdate()
        {
            var projectDao = _projectDao.Value;
            Project project = projectDao.GetProjectById(_projectId);
            project.Name = textbox_projectName.Text;
            project.Description = textbox_Desciption.Text;
            project.Progress = int.Parse(combobox2_progress.SelectedItem.ToString());
            project.StartDate = dateTime_startDate2.Value;
            project.EndDate = dateTime_endDate2.Value;
            if (textBox_projectBonus.Text != "")
            {
                project.Bonus = decimal.Parse(textBox_projectBonus.Text);
            }
            
            return project;
        }
        
        private void FormViewOrUpdateProject_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void button_save_Click(object sender, EventArgs e)
        {
            if (CheckDataInput())
            {
                var projectDao = _projectDao.Value;
                projectDao.UpdateProject(GetProjectForUpdate());
            }
        }
        
        private bool CheckDataInput()
        {
            if (string.IsNullOrEmpty(textbox_projectName.Text) || string.IsNullOrEmpty(textbox_Desciption.Text))
            {
                MessageBox.Show(@"Các trường bắt buộc chưa được điền. Vui lòng điền đầy đủ thông tin!");
                return false;
            }
            return true;
        }

        private void combobox2_progress_SelectedIndexChanged(object sender, EventArgs e)
        {
            int progress = Convert.ToInt32(combobox2_progress.SelectedItem);
            circleProgressBar2.Value = progress;
            progressValue2.Text = progress.ToString("0.'%'");
        }
    }
}
