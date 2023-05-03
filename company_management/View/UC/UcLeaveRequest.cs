using System;
using System.Collections.Generic;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;

namespace company_management.View.UC
{
    public partial class UcLeaveRequest : UserControl
    {
        private readonly Lazy<LeaveRequestDao> _requestDao;
        private readonly Lazy<LeaveRequestBus> _requestBus;
        private readonly Lazy<List<LeaveRequest>> _listRequest;
        private readonly Lazy<Utils> _utils;
        private int _selectedId;
        
        public UcLeaveRequest()
        {
            _requestBus = new Lazy<LeaveRequestBus>(() => new LeaveRequestBus());
            _requestDao = new Lazy<LeaveRequestDao>(() => new LeaveRequestDao());
            _listRequest = new Lazy<List<LeaveRequest>>(() => new List<LeaveRequest>());
            _utils = new Lazy<Utils>(() => new Utils());
            InitializeComponent();
        }
        
        private void UCLeaveRequest_Load(object sender, EventArgs e)
        {
            LoadData(GetData());
        }

        private void LoadData(List<LeaveRequest> requests)
        {
            LoadDataGridview(requests);
            LoadRequestsStatistics(requests);
            CheckControlStatus();
        }

        private void LoadRequestsStatistics(List<LeaveRequest> requests)
        {
            var requestBus = _requestBus.Value;
            var requestStatistics = requestBus.GetRequestsStatistics(requests);

            label_allCount.Text = requestStatistics.AllCount.ToString();
            label_approved.Text = requestStatistics.Approved.ToString();
            label_rejected.Text = requestStatistics.Rejected.ToString();
            label_pending.Text = requestStatistics.Pending.ToString();
            label_cancelled.Text = requestStatistics.Cancelled.ToString();
        }
        
        private void CheckControlStatus()
        {
            var util = _utils.Value;
            util.CheckManagerNotVisibleStatus(buttonAdd);
        }
        
        private void datagridview_leaveRequest_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var approveColumn = datagridview_leaveRequest.Columns["Duyệt"];
                var rejectColumn = datagridview_leaveRequest.Columns["Từ chối"];

                if (datagridview_leaveRequest.Columns[e.ColumnIndex] == rejectColumn)
                {
                    var result = MessageBox.Show("Lưu thay đổi", "Từ chối yêu cầu xin nghỉ!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        datagridview_leaveRequest.Rows[e.RowIndex].Cells["Duyệt"].Value = false;
                        datagridview_leaveRequest.Rows[e.RowIndex].Cells["Từ chối"].Value = true;
                    }
                    else
                    {
                        datagridview_leaveRequest.Rows[e.RowIndex].Cells["Từ chối"].Value = false;
                    }
                }
                else if (datagridview_leaveRequest.Columns[e.ColumnIndex] == approveColumn)
                {
                    datagridview_leaveRequest.Rows[e.RowIndex].Cells["Từ chối"].Value = false;
                }
            }

        }   

        private void LoadDataGridview(List<LeaveRequest> requests)
        {
            var requestBus = _requestBus.Value;
            requestBus.LoadDataGridview(requests, datagridview_leaveRequest);
        }

        private List<LeaveRequest> GetData()
        {
            var requestBus = _requestBus.Value;
            return requestBus.GetListRequestByPosition();
        }
        
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormAddRequest requestForm = new FormAddRequest();
            requestForm.Show();
        }

        private void btnViewOrUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                 FormViewOrUpdateRequest formRequest = new FormViewOrUpdateRequest();
                 formRequest.SetRequestId(_selectedId);
                 formRequest.Show();
            }
            else MessageBox.Show("Bạn chưa chọn đơn nào! Vui lòng chọn một đơn để xem", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void datagridview_leaveRequest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                object value = datagridview_leaveRequest.Rows[e.RowIndex].Cells[0].Value;
                if (value != DBNull.Value)
                {
                    _selectedId = Convert.ToInt32(value);
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                DialogResult result = MessageBox.Show(@"Bạn chắc chắn muốn xóa đơn?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var requestDao = _requestDao.Value;
                    requestDao.DeleteRequest(_selectedId);
                    LoadData(GetData());
                }
            }
            else MessageBox.Show(@"Vui lòng chọn đơn trước!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData(GetData());
        }

        private void combobox_requestStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var requestBus = _requestBus.Value;
            var requests = _listRequest.Value;
            requests.Clear();
            
            int selectedIndex = combobox_requestStatusFilter.SelectedIndex;
            switch (selectedIndex)
            {
                case 1:
                    requests = requestBus.GetApprovedRequests();
                    break;
                case 2:
                    requests = requestBus.GetPendingRequests();
                    break;
                case 3:
                    requests = requestBus.GetRejectedRequests();
                    break;
                case 4:
                    requests = requestBus.GetCancelledRequests();
                    break;
                default:
                    requests = requestBus.GetListRequestByPosition();
                    break;
            }

            LoadDataGridview(requests);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (_selectedId != 0)
            {
                FormViewOrUpdateRequest formRequest = new FormViewOrUpdateRequest();
                formRequest.SetRequestId(_selectedId);
                formRequest.Show();
            }
            else MessageBox.Show("Bạn chưa chọn đơn nào! Vui lòng chọn một đơn để xem", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
    }
}
