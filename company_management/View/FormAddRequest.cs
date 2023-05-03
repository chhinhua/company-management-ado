using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;

namespace company_management.View
{
    public partial class FormAddRequest : Form
    {
        private readonly Lazy<LeaveRequestBus> _requestBus;
        private readonly Lazy<LeaveRequestDao> _requestDao;

        public FormAddRequest()
        {
            _requestBus = new Lazy<LeaveRequestBus>(() => new LeaveRequestBus());
            _requestDao = new Lazy<LeaveRequestDao>(() => new LeaveRequestDao());
            InitializeComponent();
        }
        
        private bool CheckDataInput()
        {
            if (string.IsNullOrEmpty(txtbox_content.Text))
            {
                MessageBox.Show("Các trường bắt buộc chưa được điền. Vui lòng điền đầy đủ thông tin!");
                return false;
            }
            return true;
        }
        
        private void button_apply_Click(object sender, EventArgs e)
        {
            var requestBus = _requestBus.Value;
            LeaveRequest request = requestBus.GetRequestFromTextBox(datetime_startDate, datetime_endDate, datetime_requestDate, txtbox_content);

            if (CheckDataInput())
            {
                var requestDao = _requestDao.Value;
                requestDao.AddRequest(request);
            }
            
            ClearFields();
        }
        
        private void ClearFields()
        {
            txtbox_content.Clear();
        }
        
        private void LoadTimeNow()
        {
            datetime_startDate.Value = DateTime.Now.Date;
            datetime_requestDate.Value = DateTime.Now.Date;
            datetime_endDate.Value = DateTime.Now.AddDays(1);
        }

        private void FormAddRequest_Load(object sender, EventArgs e)
        {
            LoadTimeNow();
        }
    }
}
