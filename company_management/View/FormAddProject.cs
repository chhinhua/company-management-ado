using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using company_management.DAO;
using company_management.DTO;
using company_management.BUS;
using company_management.Utilities;
using System.Data.SqlClient;

namespace company_management.View
{
    public partial class FormAddProject : Form
    {
        private readonly Lazy<ProjectBus> _projectBus;
        private readonly Lazy<ProjectDao> _projectDao;

        public FormAddProject()
        {
            InitializeComponent();
            var utils = new Utils();
            _projectBus = new Lazy<ProjectBus>(() => new ProjectBus());
            _projectDao = new Lazy<ProjectDao>(() => new ProjectDao());
            utils.SetFormShadow(this);
        }

        private void Alert(string msg, FormAlert.enmType type)
        {
            FormAlert frm = new FormAlert();
            frm.showAlert(msg, type);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckDataInput())
            {
                var projectBus = _projectBus.Value;
                var projectDao = _projectDao.Value;

                Project project = projectBus.GetProjectFromTextBox(txtbox_projectName.Text, txtbox_Desciption.Text,
                                  dateTime_startDate, dateTime_endDate, combbox_AssigneeTeam, 0, textBox_Bonus.Text);
                projectDao.AddProject(project);
                ClearFields();
            }
        }

        private bool CheckDataInput()
        {
            if (string.IsNullOrEmpty(txtbox_projectName.Text) || string.IsNullOrEmpty(txtbox_Desciption.Text))
            {
                MessageBox.Show("Các trường bắt buộc chưa được điền. Vui lòng điền đầy đủ thông tin!");
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void LoadData()
        {
            var projectDao = _projectDao.Value;
            projectDao.LoadTeamToCombobox(combbox_AssigneeTeam);
        }

        private void ClearFields()
        {
            txtbox_projectName.Clear();
            txtbox_Desciption.Clear();
        }

        private void Form_AddProject_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
