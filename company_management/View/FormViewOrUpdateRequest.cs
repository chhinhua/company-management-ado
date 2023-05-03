using System;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;
// ReSharper disable All

namespace company_management.View
{
    public partial class FormViewOrUpdateRequest : Form
    {
        private int _requestId;
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<ImageDao> _imageDao;
        private readonly Lazy<LeaveRequestDao> _requestDao;
        private readonly Lazy<UserBus> _userBus;
        private readonly Lazy<Utils> _utils;

        public FormViewOrUpdateRequest()
        {
            InitializeComponent();
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _imageDao = new Lazy<ImageDao>(() => new ImageDao());
            _requestDao = new Lazy<LeaveRequestDao>(() => new LeaveRequestDao());
            _userBus = new Lazy<UserBus>(() => new UserBus());
            _utils = new Lazy<Utils>(() => new Utils());
        }

        private void FormViewOrUpdateRequest_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var requestDao = _requestDao.Value;
            LeaveRequest request = requestDao.GetRequestById(_requestId);
            BindingRequestToFields();
            CheckControlStatus(request);
            CheckControlStatusForCancelledRequest(request);
        }

        private void BindingRequestToFields()
        {
            var requestDao = _requestDao.Value;
            LeaveRequest request = requestDao.GetRequestById(_requestId);

            var userDao = _userDao.Value;
            User writer = userDao.GetUserById(request.IdUser);
            label_writer.Text = writer.FullName;
            
            var imageDao = _imageDao.Value;
            imageDao.ShowImageInPictureBox(writer.Avatar, picturebox_writer);
            
            if (request.Status == "Approved" || request.Status == "Rejected")
            {
                combobox_status.SelectedIndex = request.Status == "Approved" ? 0 : 1;
            }
            else if (request.Status == "Cancelled")
            {
                label_approver.Text = "";
            }

            User approver = userDao.GetUserById(request.IdApprover);
            if (approver != null)
            {
                label_approver.Text = approver.FullName;
                imageDao.ShowImageInPictureBox(approver.Avatar, picturebox_approver);
            }

            label_numberDay.Text = request.NumberDay + @" ngày";
            label_status.Text = request.Status;
            txtbox_content.Text = request.Content;
            datetime_requestDate.Value = request.RequestDate;
            datetime_startDate.Value = request.StartDate;
            datetime_endDate.Value = request.EndDate;
        }
        
        private void CheckControlStatus(LeaveRequest request)
        {
            var util = _utils.Value;
            util.CheckCancelMyRequestStatus(button_Cancel, request.Status, request.IdUser);
            util.CheckManagerIsVisibleStatus(combobox_status);
            util.CheckManagerIsVisibleStatus(label_approved);
            util.CheckManagerIsReadOnlyStatus(txtbox_content);
            util.CheckManagerNotEnableStatus(datetime_requestDate);
            util.CheckManagerNotEnableStatus(datetime_startDate);
            util.CheckManagerNotEnableStatus(datetime_endDate);
        }

        private void CheckControlStatusForCancelledRequest(LeaveRequest request)
        {
            if (request.Status == "Cancelled")
            {
                txtbox_content.ReadOnly = true;
                datetime_requestDate.Enabled = false;
                datetime_startDate.Enabled = false;
                datetime_endDate.Enabled = false;
                button_save.Enabled = false;
                combobox_status.Enabled = false;
            }
        }
        
        public void SetRequestId(int id) => _requestId = id;

        private LeaveRequest GetRequestForUpdate()
        {
            var requestDao = _requestDao.Value;
            var util = _utils.Value;

            LeaveRequest request = requestDao.GetRequestById(_requestId);
            request.RequestDate = datetime_requestDate.Value;
            request.Content = util.EscapeSqlString(txtbox_content.Text);
            request.StartDate = datetime_startDate.Value;
            request.EndDate = datetime_endDate.Value;
            request.NumberDay = (int)(request.EndDate - request.StartDate).TotalDays;

            if (request.Status != "Canceled")
            {
                request.Status = GetStatusFromComboboxStatus();
                if (request.Status != "Pending")
                {
                    request.IdApprover = UserSession.LoggedInUser.Id;
                }
            }

            return request;
        }

        private string GetStatusFromComboboxStatus()
        {
            string status = "Pending";
            int selectedStatus = combobox_status.SelectedIndex;
            switch (selectedStatus)
            {
                case 0:
                    status = "Approved";
                    break;
                case 1:
                    status = "Rejected";
                    break;
            }

            return status;
        }

        private bool CheckDataInput()
        {
            if (string.IsNullOrEmpty(txtbox_content.Text))
            {
                MessageBox.Show(@"Nội dung nghỉ phép không được bỏ trống. Vui lòng điền đầy đủ thông tin!");
                return false;
            }
            return true;
        }
        
        private void button_save_Click(object sender, EventArgs e)
        {
            if (CheckDataInput())
            {
                var requestDao = _requestDao.Value;
                var userBus = _userBus.Value;
                if (userBus.IsManager())
                {
                    requestDao.UpdateManaRequest(GetRequestForUpdate());
                }
                else
                {
                    requestDao.UpdateRequest(GetRequestForUpdate());
                }
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            var requestDao = _requestDao.Value;
            LeaveRequest request = requestDao.GetRequestById(_requestId);

            request.Status = "Cancelled";

            requestDao.UpdateRequest(request);
        }

        private void combobox_status_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedStatus = combobox_status.SelectedIndex;
            switch (selectedStatus)
            {
                case 0:
                case 1:
                    var userDao = _userDao.Value;
                    var imageDao = _imageDao.Value;

                    var approver = userDao.GetUserById(UserSession.LoggedInUser.Id);
                    imageDao.ShowImageInPictureBox(approver.Avatar, picturebox_approver);
                    label_approver.Text = approver.FullName;

                    break;
            }
        }
    }
}