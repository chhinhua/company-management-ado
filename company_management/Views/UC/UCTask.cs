﻿using company_management.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DTO;

namespace company_management.Views.UC
{
    public partial class UCTask : UserControl
    {
        private List<Task> listTask;
        private TaskBUS taskBUS;
        private TaskDAO taskDAO; 
        public static Task viewTask;

        public UCTask()
        {
            InitializeComponent();
            listTask = new List<Task>();
            taskBUS = new TaskBUS();
            taskDAO = new TaskDAO();
            viewTask = new Task();
        }

        private void UCTask_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void CheckAddButtonStatus()
        {
            taskBUS.CheckButtonStatus(buttonAdd);
            taskBUS.CheckButtonStatus(buttonRemove);
        }

        private void LoadData()
        {
            LoadDataGridview();
            LoadProgressChart();
            CheckAddButtonStatus();
        }

        private void LoadProgressChart()
        {
            TaskStatusPercentage taskStatus = taskDAO.GetTaskStatusPercentage(listTask);

            double todoPercent = taskStatus.TodoPercent;
            double inprogressPercent = taskStatus.InprogressPercent;
            double donePercent = taskStatus.DonePercent;

            // Định dạng giá trị với 2 chữ số sau dấu thập phân
            string todoPercentFormatted = todoPercent.ToString("0.00");
            string inprogressPercentFormatted = inprogressPercent.ToString("0.00");
            string donePercentFormatted = donePercent.ToString("0.00");

            chart_taskProgress.Series["SeriesProgress"].Points.Clear();

            // Thêm các phần tử vào danh sách
            chart_taskProgress.Series["SeriesProgress"].Points.AddXY("", todoPercent);
            chart_taskProgress.Series["SeriesProgress"].Points.AddXY("", inprogressPercent);
            chart_taskProgress.Series["SeriesProgress"].Points.AddXY("", donePercent);
            // chart_taskProgress.Series["SeriesProgress"].Points[2].LegendText = "Done";

            // Ẩn nhãn trên biểu đồ tròn
            chart_taskProgress.Series["SeriesProgress"].IsValueShownAsLabel = false;

            chart_taskProgress.Legends.Clear();

            chart_taskProgress.Series["SeriesProgress"].Points[0].Color = Color.FromArgb(214, 40, 40);
            chart_taskProgress.Series["SeriesProgress"].Points[1].Color = Color.FromArgb(0, 255, 0);
            chart_taskProgress.Series["SeriesProgress"].Points[2].Color = Color.FromArgb(67, 97, 238);

            label_todoTask.Text = todoPercentFormatted + "%";
            label_inprogressTask.Text = inprogressPercentFormatted + "%";
            label_doneTask.Text = donePercentFormatted + "%";
        }

        private void LoadDataGridview()
        {
            listTask = taskBUS.GetListTaskByPosition();
            taskBUS.LoadDataGridview(listTask, dataGridView_Task);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        }

        private void dgvTask_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == dataGridView_Task.Columns["progress"].Index)
            {
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);

                // Thêm ký tự % vào header của cột progress
                string headerText = dataGridView_Task.Columns["deadline"].HeaderText.Replace("deadline", "") + "(%)";
                e.Graphics.DrawString(headerText, e.CellStyle.Font, Brushes.Black, e.CellBounds, new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far });

                e.Handled = true;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddTaskForm addTask = new AddTaskForm();
            addTask.Show();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (viewTask.Id != 0)
            {
                DialogResult result = MessageBox.Show("Delete task?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    taskDAO.DeleteTask(viewTask.Id);
                    LoadDataGridview();
                }
            }
            else MessageBox.Show("Task not selected!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;
            int clmnCreator = 1;
            int clmnTaskName = 2;
            int clmnAssignee = 3;
            int clmnTeam = 4;

            listTask = taskBUS.SearchTasksByKeyword(dataGridView_Task, keyword, clmnCreator, clmnTaskName, clmnAssignee, clmnTeam);
            taskBUS.LoadDataGridview(listTask, dataGridView_Task);
        }

        private void btnViewOrUpdate_Click(object sender, EventArgs e)
        {
            if (viewTask.Id != 0)
            {
                ViewOrUpdateTaskForm viewOrUpdate = new ViewOrUpdateTaskForm();
                viewOrUpdate.Show();
            }
            else MessageBox.Show("Select a task to view", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button_Edit_Click(object sender, EventArgs e)
        {
            if (viewTask.Id != 0)
            {
                ViewOrUpdateTaskForm viewOrUpdate = new ViewOrUpdateTaskForm();
                viewOrUpdate.Show();
            }
            else MessageBox.Show("Select a task to view", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dataGridView_Task_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView_Task.CurrentRow.Selected = true;

            if (e.RowIndex != -1)
            {
                object value = dataGridView_Task.Rows[e.RowIndex].Cells[0].Value;
                if (value != DBNull.Value)
                {
                    int id = Convert.ToInt32(value);
                    viewTask = taskDAO.GetTaskById(id);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
