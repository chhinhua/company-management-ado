using System;

namespace company_management.DTO
{
    public class CheckinCheckout
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set; }
        public double TotalHours { get; set; }
        public DateTime Date { get; set; }
        
        public CheckinCheckout() { }

        public CheckinCheckout(int idUser, DateTime checkinTime, DateTime date)
        {
            IdUser = idUser;
            CheckinTime = checkinTime;
            Date = date;
        }

        public CheckinCheckout(int idUser, DateTime checkinTime, DateTime checkoutTime, double totalHours, DateTime date)
        {
            IdUser = idUser;
            CheckinTime = checkinTime;
            CheckoutTime = checkoutTime;
            TotalHours = totalHours;
            Date = date;
        }

        public double CalculateTotalHours()
        {
            TotalHours = (CheckoutTime - CheckinTime).TotalHours;
            return Math.Round(TotalHours, 2);
        }
    }
}
