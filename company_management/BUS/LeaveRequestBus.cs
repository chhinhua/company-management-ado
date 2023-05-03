using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;
using Guna.UI2.WinForms;
// ReSharper disable All

namespace company_management.BUS
{
    public class LeaveRequestBus
    {
        private readonly Lazy<Utils> _utils;
        private readonly Lazy<TeamDao> _teamDao;
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<UserBus> _userBus;
        private readonly Lazy<TaskBus> _taskBus;
        private readonly Lazy<LeaveRequestDao> _requestDao;
        private readonly Lazy<LeaveRequestBus> _requestBus;
        private readonly Lazy<List<LeaveRequest>> _listRequest;

        public LeaveRequestBus()
        {
            _utils = new Lazy<Utils>(() => new Utils());
            _teamDao = new Lazy<TeamDao>(() => new TeamDao());
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _userBus = new Lazy<UserBus>(() => new UserBus());
            _taskBus = new Lazy<TaskBus>(() => new TaskBus());
            _requestBus = new Lazy<LeaveRequestBus>(() => new LeaveRequestBus());
            _requestDao = new Lazy<LeaveRequestDao>(() => new LeaveRequestDao());
            _listRequest = new Lazy<List<LeaveRequest>>(() => new List<LeaveRequest>());
        }

        public void LoadDataGridview(List<LeaveRequest> listProject, DataGridView dataGridView)
        {
            var userDao = _userDao.Value;

            dataGridView.ColumnCount = 8;
            dataGridView.Columns[0].Name = "Id";
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Name = "Họ tên";
            dataGridView.Columns[2].Name = "Ngày viết";
            dataGridView.Columns[3].Name = "Nghỉ từ ngày";
            dataGridView.Columns[4].Name = "Đến ngày";
            dataGridView.Columns[5].Name = "Tổng số ngày";
            dataGridView.Columns[6].Name = "Trạng thái";
            dataGridView.Columns[7].Name = "Người phê duyệt";
            dataGridView.Rows.Clear();

            foreach (var rq in listProject)
            {
                var writer = userDao.GetUserById(rq.IdUser).FullName;

                var user = userDao.GetUserById(rq.IdApprover);
                var approver = user != null ? user.FullName : "N/A";

                dataGridView.Rows.Add(rq.Id, writer, rq.RequestDate.ToString("d/M/yyyy"),
                    rq.StartDate.ToString("d/M/yyyy"), rq.EndDate.ToString("d/M/yyyy"), rq.NumberDay, rq.Status,
                    approver);
            }
        }

        public LeaveRequest GetRequestFromTextBox(Guna2DateTimePicker startDate, Guna2DateTimePicker endDate,
            Guna2DateTimePicker requestDate, Guna2TextBox content)
        {
            LeaveRequest request = new LeaveRequest
            {
                IdUser = UserSession.LoggedInUser.Id,
                RequestDate = requestDate.Value,
                StartDate = startDate.Value,
                EndDate = endDate.Value,
                NumberDay = (int)(endDate.Value - startDate.Value).TotalDays,
                Content = content.Text,
                Status = "Pending"
            };

            return request;
        }

        public List<LeaveRequest> GetListRequestByPosition()
        {
            var requests = _listRequest.Value;
            var requestDao = _requestDao.Value;
            var userBus = _userBus.Value;

            ClearListRequest(requests);

            if (userBus.IsManager())
            {requests = requestDao.GetAllLeaveRequests();}
            else
            {requests = requestDao.GetMyLeaveRequests(UserSession.LoggedInUser.Id);}

            return requests;
        }

        public List<LeaveRequest> GetPendingRequests() =>
            GetListRequestByPosition().Where(r => r.Status == "Pending").ToList();

        public List<LeaveRequest> GetApprovedRequests() =>
            GetListRequestByPosition().Where(r => r.Status == "Approved").ToList();

        public List<LeaveRequest> GetRejectedRequests() =>
            GetListRequestByPosition().Where(r => r.Status == "Rejected").ToList();

        public List<LeaveRequest> GetCancelledRequests() =>
            GetListRequestByPosition().Where(r => r.Status == "Cancelled").ToList();

        public RequestStatistics GetRequestsStatistics(List<LeaveRequest> requests)
        {
            RequestStatistics requestStatistics = new RequestStatistics();
            requestStatistics.AllCount = requests.Count;
            requestStatistics.Pending = requests.Where(r => r.Status == "Pending").Count();
            requestStatistics.Approved = requests.Where(r => r.Status == "Approved").Count();
            requestStatistics.Cancelled = requests.Where(r => r.Status == "Cancelled").Count();
            requestStatistics.Rejected = requests.Where(r => r.Status == "Rejected").Count();
            return requestStatistics;
        }

        private void ClearListRequest(List<LeaveRequest> listRequest)
        {
            listRequest.Clear();
            listRequest.TrimExcess();
        }
    }
}