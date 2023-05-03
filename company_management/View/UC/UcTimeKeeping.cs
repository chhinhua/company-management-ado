using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DAO;
using company_management.DTO;

namespace company_management.View.UC
{
    public partial class UcTimeKeeping : UserControl
    {
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<CheckinCheckoutDao> _cicoDao;
        private readonly Lazy<CheckinCheckoutBus> _cicoBus;
        public static int LastCheckinCheckoutId;

        public UcTimeKeeping()
        {
            InitializeComponent();
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _cicoDao = new Lazy<CheckinCheckoutDao>(() => new CheckinCheckoutDao());
            _cicoBus = new Lazy<CheckinCheckoutBus>(() => new CheckinCheckoutBus());
        }

        private void UCTimeKeeping_Load(object sender, EventArgs e)
        {
            LoadDataGridview();
            LoadTimeNow();
        }

        private void Alert(string msg, FormAlert.enmType type)
        {
            FormAlert frm = new FormAlert();
            frm.showAlert(msg, type);
        }

        private void LoadTimeNow()
        {
            datetime_date.Value = DateTime.Now.Date;
            datetime_Checkin.Value = DateTime.Now;
            datetime_Checkout.Value = DateTime.Now;

            toggle_checkout.Checked = false;
        }

        private void LoadDataGridview()
        {
            var cicoBus = _cicoBus.Value;
            List<CheckinCheckout> data = cicoBus.GetListCheckinCheckoutsByPosition();
            cicoBus.LoadDataGridview(data, datagridview_timeKeeping);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            /*string keyword = txtSearch.Text;

            // Tạo một chuỗi điều kiện để lọc dữ liệu
            StringBuilder filterExpression = new StringBuilder();
            foreach (DataGridViewColumn column in datagridview_timeKeeping.Columns)
            {
                // Chỉ áp dụng lọc cho các cột chứa dữ liệu
                if (column.Visible && column.Name == "Employee")
                {
                    if (filterExpression.Length > 0)
                    {
                        filterExpression.Append(" OR ");
                    }
                    filterExpression.Append($"[{column.DataPropertyName}] LIKE '%{keyword}%'");
                }
            }

            // Áp dụng chuỗi điều kiện lọc dữ liệu vào DataGridView
            ((DataTable)datagridview_timeKeeping.DataSource).DefaultView.RowFilter = filterExpression.ToString();*/
        }

        private void ClearAll()
        {
            LastCheckinCheckoutId = 0;
            txtBox_fullName.Clear();
            txtBox_totalHours.Clear();
            toggle_checkin.Checked = false;
            toggle_checkout.Checked = false;
            toggle_checkin.Enabled = true;
            toggle_checkout.Enabled = true;
            LoadTimeNow();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void toggle_checkin_Click(object sender, EventArgs e)
        {
            if (toggle_checkin.Checked)
            {
                DialogResult result = MessageBox.Show("Dữ liệu sẽ được tạo sau khi bạn bấm yes.", "Xác nhận Checkin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var cicoBus = _cicoBus.Value;
                    DateTime checkinTime = datetime_Checkin.Value;
                    DateTime date = datetime_date.Value.Date;
                    cicoBus.Checkin(checkinTime, date);
                    this.Alert("Checkin successful", FormAlert.enmType.Success);
                    LoadDataGridview();
                    toggle_checkin.Enabled = false;
                }
                else
                {
                    toggle_checkin.Checked = false;
                }
            }
        }

        private void toggle_checkout_Click(object sender, EventArgs e)
        {

            // Kiểm tra xem toggle switch "checkin" đã được chọn hay chưa
            if (toggle_checkin.Checked == false)
            {
                this.Alert("Failed to checkout", FormAlert.enmType.Warning);
                MessageBox.Show("Bạn phải checkin trước khi checkout.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                toggle_checkout.Checked = false;
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Dữ liệu sẽ được lưu sau khi bạn bấm yes.", "Xác nhận Checkout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var cicoBus = _cicoBus.Value;
                    DateTime checkoutTime = datetime_Checkout.Value;
                    cicoBus.Checkout(checkoutTime);
                    this.Alert("Checkout successful", FormAlert.enmType.Success);
                    LoadDataGridview();

                    toggle_checkout.Enabled = false;
                }
                else
                {
                    toggle_checkout.Checked = false;
                }
            }
        }

        private void datagridview_timeKeeping_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = datagridview_timeKeeping.Rows[e.RowIndex];
                LastCheckinCheckoutId = (int)selectedRow.Cells[0].Value;

                var cicoDao = _cicoDao.Value;
                CheckinCheckout cico = cicoDao.GetCheckinCheckoutById(LastCheckinCheckoutId);

                var userDao = _userDao.Value;
                txtBox_fullName.Text = userDao.GetUserById(cico.IdUser).FullName;
                txtBox_totalHours.Text = cico.TotalHours.ToString() + " h";
                datetime_date.Value = cico.Date;
                datetime_Checkin.Value = cico.CheckinTime;

                // Bấm vào một hàng là mặc định giờ checkin được chọn
                toggle_checkin.Checked = true;
                toggle_checkin.Enabled = false;
                                
                // Check xem có checkout hay chưa (giá trị mặc định là DateTime.MinVal)
                if (cico.CheckoutTime == DateTime.MinValue)
                {
                    toggle_checkout.Checked = false;
                    toggle_checkout.Enabled = true;
                }
                else
                {
                    datetime_Checkout.Value = cico.CheckoutTime;
                    toggle_checkout.Checked = true;
                    toggle_checkout.Enabled = false;                
                }
            }
        }
    }
}
