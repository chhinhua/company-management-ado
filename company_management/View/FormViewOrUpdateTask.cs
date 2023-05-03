using System;
using System.Globalization;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;
using company_management.BUS;
using company_management.View.UC;
using company_management.Utilities;

namespace company_management.View
{
    public partial class FormViewOrUpdateTask : Form
    {
        private readonly Lazy<TaskDao> _taskDao;
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<TeamDao> _teamDao;
        private readonly Lazy<ImageDao> _imageDao;
        private readonly Lazy<TaskBus> _taskBus;
        private readonly Utils _utils;
        private int _taskId;

        public FormViewOrUpdateTask()
        {
            InitializeComponent();
            _taskDao = new Lazy<TaskDao>(() => new TaskDao());
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _teamDao = new Lazy<TeamDao>(() => new TeamDao());
            _imageDao = new Lazy<ImageDao>(() => new ImageDao());
            _taskBus = new Lazy<TaskBus>(() => new TaskBus());
            _utils =  new Utils();
        }

        private void LoadData()
        {
            _taskBus.Value.GetDataToCombobox(combbox_Assignee, combbox_Project);
            BindingTaskToFields();
            CheckControlStatus();
        }

        private void BindingTaskToFields()
        {
            Task task = _taskDao.Value.GetTaskById(_taskId);
            User assigneeUser = _userDao.Value.GetUserById(task.IdAssignee);
            Team assigneeTeam = _teamDao.Value.GetTeamById(task.IdTeam);

            var imageDao = _imageDao.Value;
            imageDao.ShowImageInPictureBox(assigneeUser.Avatar, picturebox_userAvatar);
            imageDao.ShowImageInPictureBox(assigneeTeam.Avatar, picturebox_teamAvatar);

            txtbox_Taskname.Text = task.TaskName;
            txtbox_Desciption.Text = task.Description;
            textBox_Bonus.Text = task.Bonus.ToString(CultureInfo.CurrentCulture);
            
            label_assigneedTeam.Text = assigneeTeam.Name;
            label_assigneedPerson.Text = assigneeUser.FullName;

            circleProgressBar.Value = task.Progress;
            progressValue.Text = task.Progress + @"%";

            var taskBus = _taskBus.Value;
            taskBus.SelectComboBoxItemByValue(combobox_progress, task.Progress);
            GetSelectedValueToCombobox(taskBus, task.IdProject, assigneeUser, assigneeTeam);
        }

        private void CheckControlStatus()
        {
            _utils.CheckEmployeeIsReadOnlyStatus(txtbox_Taskname);
            _utils.CheckEmployeeIsReadOnlyStatus(textBox_Bonus);
            _utils.CheckEmployeeNotEnableStatus(combbox_Assignee);
            _utils.CheckEmployeeNotEnableStatus(combbox_Project);
            _utils.CheckEmployeeNotEnableStatus(dateTime_deadline);
            _utils.CheckHrNotVisibleStatus(btnSave);
        }
        
        private void GetSelectedValueToCombobox(TaskBus taskBus, int idProject, User assigneeUser, Team assigneeTeam)
        {
            if (UserSession.LoggedInUser.IdPosition == 1)
            {
                taskBus.SelectComboboxItemById<Team>(combbox_Assignee, assigneeTeam.Id);
            }
            else if (UserSession.LoggedInUser.IdPosition == 2)
            {
                taskBus.SelectComboboxItemById<User>(combbox_Assignee, assigneeUser.Id);
            }
            taskBus.SelectComboboxItemById<Project>(combbox_Project, idProject);

            try
            {
                dateTime_deadline.Value = UcTask.ViewTask.Deadline;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private bool CheckDataInput()
        {
            if (string.IsNullOrEmpty(txtbox_Taskname.Text))
            {
                _utils.Alert("Field required", FormAlert.enmType.Warning);
                MessageBox.Show(@"Các trường là bắt buộc. Vui lòng điền đầy đủ thông tin!");
                return false;
            }
            return true;
        }
        
        public void SetTaskId(int id) => _taskId = id;

        private void combobox_progress_SelectedIndexChanged(object sender, EventArgs e)
        {
            int progress = Convert.ToInt32(combobox_progress.SelectedItem);
            circleProgressBar.Value = progress;
            progressValue.Text = progress + @"%";
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        { 
            if (CheckDataInput())
            {
                int progress = int.Parse(combobox_progress.SelectedItem.ToString());
                
                Task task = _taskBus.Value.GetTaskFromFieldsForUpdate(_taskId,txtbox_Taskname, txtbox_Desciption,
                                     dateTime_deadline, combbox_Assignee, progress, textBox_Bonus.Text, combbox_Project);

                _taskDao.Value.UpdateTask(task);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void chooseImage_Click(object sender, EventArgs e)
        {
            var imageDao = _imageDao.Value;
            imageDao.ChooseImageToPictureBox(picturebox_teamAvatar);
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            var imageDao = _imageDao.Value;
            byte[] imageBytes = imageDao.ImageToByte(picturebox_teamAvatar);
            imageDao.SaveTeamAvatar(imageBytes, UcTask.ViewTask.IdTeam);
            imageDao.ShowImageInPictureBox(imageBytes, picturebox_teamAvatar);
        }

        private void Form_ViewOrUpdateTask_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
