using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using company_management.DTO;
using company_management.Utilities;
using company_management.View;
// ReSharper disable All

namespace company_management.DAO
{
    public sealed class LeaveRequestDao : IDisposable
    {
        private DBConnection _dBConnection;
        private readonly Utils _utils;

        public LeaveRequestDao()
        {
            _dBConnection = new DBConnection();
            _utils = new Utils();
        }
        
         public void AddRequest(LeaveRequest request)
        {
            string query = string.Format(
                "INSERT INTO leaveRequest (idUser, requestDate, startDate, endDate, numberDay, content, status)" +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                request.IdUser, request.RequestDate, request.StartDate, request.EndDate, request.NumberDay, _utils.EscapeSqlString(request.Content), request.Status);
            if (_dBConnection.ExecuteQuery(query))
            {
                _utils.Alert("Add successful", FormAlert.enmType.Success);
            }
            else
            {
                _utils.Alert("Add failed", FormAlert.enmType.Error);
            }
        }
         
        public void UpdateRequest(LeaveRequest request)
        {
            string query = string.Format("UPDATE leaveRequest SET idUser='{0}', requestDate='{1}', startDate='{2}', endDate='{3}', numberDay='{4}', content='{5}', status='{6}' WHERE id='{7}'",
                request.IdUser, request.RequestDate, request.StartDate, request.EndDate, request.NumberDay, request.Content, request.Status, request.Id);
            if (_dBConnection.ExecuteQuery(query))
            {
                _utils.Alert("Successful", FormAlert.enmType.Success);
            }
            else
            {
                _utils.Alert("Action failed", FormAlert.enmType.Error);
            }
        }
        
        public void DeleteRequest(int id)
        {
            string query = string.Format("DELETE FROM leaveRequest WHERE id = {0}", id);
            try
            {
                _dBConnection.ExecuteQuery(query);
                _utils.Alert("Deleted successful", FormAlert.enmType.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _utils.Alert("Deleted failed", FormAlert.enmType.Error);
            }
        }
        
        public void UpdateManaRequest(LeaveRequest request)
        {
            string query = string.Format("UPDATE leaveRequest SET idUser='{0}', requestDate='{1}', startDate='{2}', endDate='{3}', numberDay='{4}', content='{5}', status='{6}', idApprover='{7}' WHERE id='{8}'",
                request.IdUser, request.RequestDate, request.StartDate, request.EndDate, request.NumberDay, request.Content, request.Status, request.IdApprover, request.Id);
            if (_dBConnection.ExecuteQuery(query))
            {
                _utils.Alert("Updated successful", FormAlert.enmType.Success);
            }
            else
            {
                _utils.Alert("Update failed", FormAlert.enmType.Error);
            }
        }
        
        public List<LeaveRequest> GetAllLeaveRequests()
        {
            string query = "SELECT * FROM leaveRequest";
            return _dBConnection.GetListObjectsByQuery<LeaveRequest>(query);
        }

        public LeaveRequest GetRequestById(int id)
        {
            string query = $"SELECT * FROM leaveRequest WHERE id = {id}";
            return _dBConnection.GetObjectByQuery<LeaveRequest>(query);
        }
        
        public List<LeaveRequest> GetMyLeaveRequests(int idUser)
        {
            return GetAllLeaveRequests().Where(s => s.IdUser == idUser).ToList();
        }
        
        public double GetTotalLeaveHours(int idUser, DateTime fromDate, DateTime toDate, SqlConnection connection)
        {
            double leaveHours = 0;

            // Tìm tất cả các đơn xin nghỉ được chấp thuận trong khoảng thời gian fromDate - toDate
            string query = "SELECT SUM(numberDay * 8) AS leaveHours " +
                           "FROM leave_request WHERE idUser = @idUser " +
                           "AND startDate <= @toDate AND endDate >= @fromDate AND status = 'Approved'";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idUser", idUser);
                command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                command.Parameters.AddWithValue("@toDate", toDate.Date);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        leaveHours += Convert.ToDouble(reader["leaveHours"]);
                    }
                }
            }

            return Math.Max(0, leaveHours); // Return non-negative leave hours
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Giải phóng các tài nguyên quản lý ở đây.
                if (_dBConnection != null)
                {
                    _dBConnection.Dispose(); // Giải phóng đối tượng DBConnection
                    _dBConnection = null;
                }
            }
        }
    }
}