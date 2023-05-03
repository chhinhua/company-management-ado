using company_management.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using company_management.View.UC;

namespace company_management.DAO
{
    public sealed class CheckinCheckoutDao : IDisposable
    {
        private bool _disposed;
        private readonly DBConnection _dBConnection;

        public CheckinCheckoutDao()
        {
            _dBConnection = new DBConnection();
        }

        public void AddCheckinCo(CheckinCheckout cico)
        {
            string query = string.Format("INSERT INTO checkin_checkout(idUser, checkinTime, date) VALUES ('{0}', '{1}', '{2}'); SELECT SCOPE_IDENTITY();",
                                      cico.IdUser, cico.CheckinTime, cico.Date);
            int id = Convert.ToInt32(_dBConnection.ExecuteScalar(query)); // lấy giá trị ID từ cơ sở dữ liệu
            UcTimeKeeping.LastCheckinCheckoutId = id;
        }

        public void UpdateCheckinCo(CheckinCheckout cico)
        {
            string sqlStr = string.Format("UPDATE checkin_checkout SET checkoutTime = '{0}', totalHours = '{1}' WHERE id = '{2}'",
                   cico.CheckoutTime, cico.CalculateTotalHours(), cico.Id);
            _dBConnection.ExecuteQuery(sqlStr);
        }

        public void DeleteCheckinCo(int id)
        {
            string sqlStr = $"DELETE FROM task WHERE id = '{id}'";
            _dBConnection.ExecuteQuery(sqlStr);
        }

        public CheckinCheckout GetCheckinCheckoutById(int id)
        {
            string query = $"SELECT * FROM checkin_checkout WHERE id = {id}";
            return _dBConnection.GetObjectByQuery<CheckinCheckout>(query);
        }

        public List<CheckinCheckout> GetAllCheckinCheckouts()
        {
            string query = "SELECT * FROM checkin_checkout";
            return _dBConnection.GetListObjectsByQuery<CheckinCheckout>(query);
        }

        public List<CheckinCheckout> GetMyCheckinCoCheckouts(int idUser)
        {
            return GetAllCheckinCheckouts().Where(c => c.IdUser == idUser).ToList();
        }

        // Checkin mà chưa checkout
        public List<CheckinCheckout> GetIncompleteCheckinCheckouts(int idUser)
        {
            DateTime defaultDateTime = new DateTime();
            return GetAllCheckinCheckouts()     
                .Where(c => c.IdUser == idUser && c.CheckoutTime == defaultDateTime)
                .ToList();
        }

        // Hoàn thành checkin checkout
        public List<CheckinCheckout> GetCompleteCheckinCheckouts(int idUser)
        {
            DateTime defaultDateTime = new DateTime();
            return GetAllCheckinCheckouts()
                .Where(c => c.IdUser == idUser && c.CheckoutTime != defaultDateTime)
                .ToList();
        }

        // Lọc theo ngày
        public List<CheckinCheckout> GetCheckinCheckoutByDate(DateTime selectedDate)
        {
            return GetAllCheckinCheckouts()
                .Where(c => c.Date.Date == selectedDate.Date)
                .ToList();
        }
        
        public CheckinCheckout GetCheckinById(int id)
        {
            string query = string.Format("SELECT * FROM checkin_checkout WHERE id = {0}", id);
            return _dBConnection.GetObjectByQuery<CheckinCheckout>(query);
        }

        // tổng số giờ làm việc của mỗi nhân viên trong một ngày
        public double GetTotalHours(int idUser, DateTime fromDate, DateTime toDate, SqlConnection connection)
        {
            string query = "SELECT SUM(totalHours) AS totalHours " +
                           "FROM checkin_checkout WHERE idUser = @idUser " +
                           "AND date BETWEEN @fromDate AND @toDate";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idUser", idUser);
                command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                command.Parameters.AddWithValue("@toDate", toDate.Date);

                object result = command.ExecuteScalar();
                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDouble(result);
                }
            }
        }

        // số giờ tăng ca của mỗi nhân viên trong một ngày
        public double GetOvertimeHours(int idUser, DateTime fromDate, DateTime toDate, SqlConnection connection)
        {
            string query = "SELECT SUM(CASE WHEN totalHours > 8 THEN totalHours - 8 ELSE 0 END) AS overtimeHours FROM checkin_checkout WHERE idUser = @idUser AND date BETWEEN @fromDate AND @toDate";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idUser", idUser);
                command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                command.Parameters.AddWithValue("@toDate", toDate.Date);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        return Convert.ToDouble(reader[0]);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        //  số giờ nghỉ phép của mỗi nhân viên trong một ngày
        public double GetLeaveHours(int idUser, DateTime fromDate, DateTime toDate, SqlConnection connection)
        {
            string query = "SELECT SUM(CASE WHEN totalHours < 8 THEN (8 - totalHours) ELSE 0 END) " +
                           "AS leaveHours " +
                           "FROM checkin_checkout WHERE idUser = @idUser " +
                           "AND date BETWEEN @fromDate AND @toDate";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idUser", idUser);
                command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                command.Parameters.AddWithValue("@toDate", toDate.Date);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        return Convert.ToDouble(reader[0]);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _dBConnection.Dispose();
            }

            _disposed = true;
        }

        ~CheckinCheckoutDao()
        {
            Dispose(false);
        }
    }
}