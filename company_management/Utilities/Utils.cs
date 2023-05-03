using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Guna.UI2.WinForms;
using System.Windows.Forms;
using company_management.BUS;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using company_management.DAO;
using company_management.DTO;
using company_management.View;
// ReSharper disable All

namespace company_management.Utilities
{
    public class Utils
    {
        private readonly Lazy<UserBus> _userBus;
        private readonly Lazy<TaskDao> _taskDao;
        private readonly Lazy<ProjectBus> _projectBus;

        public Utils()
        {
            _userBus = new Lazy<UserBus>(() => new UserBus());
            _taskDao = new Lazy<TaskDao>(() => new TaskDao());
            _projectBus = new Lazy<ProjectBus>(() => new ProjectBus());
        }

        public bool IsPhoneNumber(string number)
        {
            // Loại bỏ các ký tự trống và dấu gạch ngang
            number = number.Replace(" ", "").Replace("-", "");

            // Kiểm tra chuỗi bắt đầu bằng "+" nếu là số điện thoại quốc tế
            if (number.StartsWith("+"))
            {
                // Chuỗi phải có ít nhất 8 ký tự sau dấu "+"
                return Regex.IsMatch(number.Substring(1), @"^\d{8,}$");
            }
            else
            {
                // Chuỗi phải có ít nhất 10 ký tự và không vượt quá 11 ký tự
                return Regex.IsMatch(number, @"^\d{10,11}$");
            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                string[] allowedDomains = { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
                return addr.Address == email && allowedDomains.Any(d => addr.Host.EndsWith(d));
            }
            catch
            {
                return false;
            }
        }

        public void CheckEmployeeNotVisibleStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Visible = !userBus.IsEmployee();
        }
        
        public void CheckHrNotVisibleStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Visible = !userBus.IsHumanResources();
        }
        
        public void CheckEmployeeNotEnableStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Enabled = !userBus.IsEmployee();
        }

        public void CheckEmployeeIsReadOnlyStatus<T>(T control) where T : Guna2TextBox
        {
            var userBus = _userBus.Value;
            control.ReadOnly = userBus.IsEmployee();
        }

        public void CheckLeaderIsReadOnlyStatus<T>(T control) where T : Guna2TextBox
        {
            var userBus = _userBus.Value;
            control.ReadOnly = userBus.IsLeader();
        }
        
        public void CheckLeaderNotEnableStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Enabled = !userBus.IsLeader();
        }
        
        public void CheckLeaderNotVisibleStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Visible = !userBus.IsLeader();
        }
        
        public void CheckManagerIsReadOnlyStatus<T>(T control) where T : Guna2TextBox
        {
            var userBus = _userBus.Value;
            control.ReadOnly = userBus.IsManager();
        }

        public void CheckManagerNotEnableStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Enabled = !userBus.IsManager();
        }
        
        public void CheckManagerIsVisibleStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Visible = userBus.IsManager();
        }
        
        public void CheckManagerNotVisibleStatus<T>(T control) where T : Control
        {
            var userBus = _userBus.Value;
            control.Visible = !userBus.IsManager();
        }

        public void CheckCancelMyRequestStatus<T>(T control, string status, int writerId) where T : Control
        {
            control.Visible = writerId == UserSession.LoggedInUser.Id;
            control.Enabled = status == "Pending";
        }
        
        public void CheckCalculateSalaryStatus<T>(T control) where T : Control
        {
            control.Visible = _userBus.Value.IsHumanResources();
        }
        
        public void SetFormShadow(Form form)
        {
            Guna2ShadowForm shadowForm = new Guna2ShadowForm();

            // Set the shadow properties
            shadowForm.SetShadowForm(form);
            shadowForm.ShadowColor = Color.Gray;
            shadowForm.BorderRadius = 20;

            form.Dock = DockStyle.Fill;
            form.Show();
        }
        
        public void Alert(string msg, FormAlert.enmType type)
        {
            FormAlert frm = new FormAlert();
            frm.showAlert(msg, type);
        }
        
        public string EscapeSqlString(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return inputString;
            }

            return inputString
                .Replace("'", "''")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t")
                .Replace("\b", "\\b")
                .Replace("\f", "\\f");
        }
        
        public void LoadProgressChart<T>(Chart chart, Label todo, Label inprogress, Label done, List<T> list)
        {
            TaskStatusPercentage taskStatus;

            if (typeof(T) == typeof(Task))
            {
                var taskDao = _taskDao.Value;
                List<Task> taskList = list.Cast<Task>().ToList();
                taskStatus = taskDao.GetTaskStatusPercentage(taskList);
            }
            else
            {
                var projectBus = _projectBus.Value;
                List<Project> projects = list.Cast<Project>().ToList();
                taskStatus = projectBus.GetProjectStatusPercentage(projects);
            }

            double todoPercent = taskStatus.TodoPercent;
            double inprogressPercent = taskStatus.InprogressPercent;
            double donePercent = taskStatus.DonePercent;

            // Định dạng giá trị với 2 chữ số sau dấu thập phân
            string todoPercentFormatted = todoPercent.ToString("0.00");
            string inprogressPercentFormatted = inprogressPercent.ToString("0.00");
            string donePercentFormatted = donePercent.ToString("0.00");

            chart.Series["SeriesProgress"].Points.Clear();

            // Thêm các phần tử vào danh sách
            chart.Series["SeriesProgress"].Points.AddXY("", todoPercent);
            chart.Series["SeriesProgress"].Points.AddXY("", inprogressPercent);
            chart.Series["SeriesProgress"].Points.AddXY("", donePercent);

            // Ẩn nhãn trên biểu đồ tròn
            chart.Series["SeriesProgress"].IsValueShownAsLabel = false;

            chart.Legends.Clear();

            chart.Series["SeriesProgress"].Points[0].Color = Color.FromArgb(214, 40, 40);
            chart.Series["SeriesProgress"].Points[1].Color = Color.FromArgb(0, 255, 0);
            chart.Series["SeriesProgress"].Points[2].Color = Color.FromArgb(67, 97, 238);

            todo.Text = todoPercentFormatted + @"%";
            inprogress.Text = inprogressPercentFormatted + @"%";
            done.Text = donePercentFormatted + @"%";
        }
    }
}
