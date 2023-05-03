using company_management.DAO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DTO;
using company_management.Utilities;
// ReSharper disable All

namespace company_management.View.UC
{
    public partial class UcTask : UserControl
    {
        public static Task ViewTask;
        private readonly Lazy<TaskBus> _taskBus;
        private readonly Lazy<TaskDao> _taskDao;
        private readonly Lazy<List<Task>> _listTask;
        private readonly Lazy<Utils> _utils;
        private int _selectedId;

        public UcTask()
        {
            InitializeComponent();
            _listTask = new Lazy<List<Task>>(() => new List<Task>());
            _taskBus = new Lazy<TaskBus>(() => new TaskBus());
            _taskDao = new Lazy<TaskDao>(() => new TaskDao());
            _utils = new Lazy<Utils>(() => new Utils());
        }
        
        private void UC_Task_Load(object sender, EventArgs e)
        {
            LoadData(GetData());
        }
        
        private void LoadData(List<Task> tasks)
        {
            LoadDataGridview(tasks);
            LoadProgressChart(tasks);
            CheckAddButtonStatus();
        }
        
        private List<Task> GetData()
        {
            var taskBus = _taskBus.Value;
            return taskBus.GetListTaskByPosition();
        }
        
        private void CheckAddButtonStatus()
        {
            _utils.Value.CheckEmployeeNotVisibleStatus(buttonRemove);
            _utils.Value.CheckHrNotVisibleStatus(buttonAdd);
            _utils.Value.CheckHrNotVisibleStatus(buttonRemove);
        }
        
        private void LoadProgressChart(List<Task> tasks)
        {
            var util = _utils.Value;
            util.LoadProgressChart(chart_taskProgress, label_todoTask, label_inprogressTask, label_doneTask, tasks);
        }

        private void LoadDataGridview(List<Task> tasks)
        {
            var taskBus = _taskBus.Value;
            taskBus.LoadDataGridview(tasks, dataGridView_Task);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormAddTask addTask = new FormAddTask();
            addTask.Show();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                DialogResult result = MessageBox.Show("Delete task?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _taskDao.Value.DeleteTask(ViewTask.Id);
                    LoadData(GetData());
                }
            }
            else MessageBox.Show("Task not selected!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnViewOrUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                FormViewOrUpdateTask viewOrUpdate = new FormViewOrUpdateTask();
                viewOrUpdate.SetTaskId(_selectedId);
                viewOrUpdate.Show();
            }
            else MessageBox.Show("Bọn chưa chọn task nào!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button_Edit_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                FormViewOrUpdateTask viewOrUpdate = new FormViewOrUpdateTask();
                viewOrUpdate.SetTaskId(_selectedId);
                viewOrUpdate.Show();
            }
            else MessageBox.Show("Bọn chưa chọn task nào!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dataGridView_Task_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                object value = dataGridView_Task.Rows[e.RowIndex].Cells[0].Value;
                if (value != DBNull.Value)
                {
                    _selectedId = Convert.ToInt32(value);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData(GetData());
        }

        private void combobox_taskStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var taskBus = _taskBus.Value;
            var tasks = _listTask.Value;
            tasks.Clear();
            
            int selectedIndex = combobox_taskStatusFilter.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    tasks = taskBus.GetListTaskByPosition();
                    break;
                case 1:
                    tasks = taskBus.GetMyCreatedTasks();
                    break;
                default:
                    tasks = taskBus.GetMyTasks();
                    break;
            }

            LoadData(tasks);
        }
        
        private void combobox_taskProgressFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var taskBus = _taskBus.Value;
            var tasks = _listTask.Value;
            tasks.Clear();
            
            int selectedIndex = combobox_taskProgressFilter.SelectedIndex;
            switch (selectedIndex)
            {
                case 1:
                    tasks = taskBus.GetListTaskByPosition();
                    break;
                case 2:
                    tasks = taskBus.GetTodoTasks();
                    break;
                case 3:
                    tasks = taskBus.GetInprogressTasks();
                    break;
                case 4:
                    tasks = taskBus.GetDoneTasks();
                    break;
                default:
                    tasks = taskBus.GetListTaskByPosition();
                    break;
            }
            
            LoadDataGridview(tasks);
        }
    }
}
