using company_management.View.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;

namespace company_management.View
{
    public partial class FormMain : Form
    {
        private readonly ImageDao _imageDa0;

        public FormMain()
        {
            InitializeComponent();
            _imageDa0 = new ImageDao();
        }

        private void AddUc(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;

            pn_main.Controls.Clear();
            pn_main.Controls.Add(uc);

            uc.BringToFront();
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            this.lb_menu_active.Location = new Point(btnTask.Location.X, btnTask.Location.Y);
            UcTask uCTask = new UcTask();
            AddUc(uCTask);
        }

        private void btnLeaveRequest_Click(object sender, EventArgs e)
        {
            this.lb_menu_active.Location = new Point(btnLeaveRequest.Location.X, btnLeaveRequest.Location.Y);
            UcLeaveRequest request = new UcLeaveRequest();
            AddUc(request);
        }

        private void btnSalary_Click(object sender, EventArgs e)
        {
            this.lb_menu_active.Location = new Point(btnSalary.Location.X, btnSalary.Location.Y);
            UcSalary uCSalary = new UcSalary();
            AddUc(uCSalary);
        }

        private void btnTimekeeping_Click(object sender, EventArgs e)
        {
            this.lb_menu_active.Location = new Point(btnTimekeeping.Location.X, btnTimekeeping.Location.Y);
            UcTimeKeeping timeKeeping = new UcTimeKeeping();
            AddUc(timeKeeping);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            this.lb_menu_active.Location = new Point(btnUser.Location.X, btnUser.Location.Y);
            UcUser userManagementUC = new UcUser();
            AddUc(userManagementUC);
        }


        private void picboxProfile_Click(object sender, EventArgs e)
        {

        }

        private void combobox_profile_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pn_main_Paint(object sender, PaintEventArgs e)
        {

        }

        public void LoadUserControl()
        {
            this.lb_menu_active.Location = new Point(btnUser.Location.X, btnUser.Location.Y);
            UcTask uCTask = new UcTask();
            AddUc(uCTask);
        }

        private void btnTeam_Click(object sender, EventArgs e)
        {
            this.lb_menu_active.Location = new Point(btnTeam.Location.X, btnTeam.Location.Y);
            UcTeam uC_Team = new UcTeam();
            AddUc(uC_Team);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            btnHome.BackColor = Color.Teal;
            this.lb_menu_active.Location = new Point(btnHome.Location.X, btnHome.Location.Y);
            UcHome uCHome = new UcHome();
            AddUc(uCHome);
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            this.lb_menu_active.Location = new Point(btnProject.Location.X, btnProject.Location.Y);
            UcProject uC_Project = new UcProject();
            AddUc(uC_Project);
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            _imageDa0.ShowImageInPictureBox(UserSession.LoggedInUser.Avatar, picturebox_avatar);
            UcHome uCHome = new UcHome();
            AddUc(uCHome);
        }

        private void combobox_user_action_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = combobox_user_action.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                   
                    break;
                case 1:
                   
                    break;
                case 2:
                   
                    break;
                case 3:
                    this.Hide();
                    FormLogin login = new FormLogin();
                    login.Show();
                    UserSession.LogoutUser();
                    break;
            }
        }
    }
}
