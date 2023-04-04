﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company_management.DTO
{
    public class CheckinCheckoutDTO
    {
        private int id;
        private int idUser;
        private DateTime checkinTime;
        private DateTime checkoutTime;
        private double totalHours;
        private DateTime date;

        public CheckinCheckoutDTO() { }

        public CheckinCheckoutDTO(int idUser, DateTime checkinTime, DateTime date)
        {
            IdUser = idUser;
            CheckinTime = checkinTime;
            Date = date;
        }

        public CheckinCheckoutDTO(int idUser, DateTime checkinTime, 
                                  DateTime checkoutTime, double totalHours, DateTime date)
        {
            IdUser = idUser;
            CheckinTime = checkinTime;
            CheckoutTime = checkoutTime;
            TotalHours = totalHours;
            Date = date;
        }

/*        public double overtimeHours()
        {
            
        }*/

        public int Id { get => id; set => id = value; }
        public int IdUser { get => idUser; set => idUser = value; }
        public DateTime CheckinTime { get => checkinTime; set => checkinTime = value; }
        public DateTime CheckoutTime { get => checkoutTime; set => checkoutTime = value; }
        public double TotalHours { get => totalHours; set => totalHours = value; }
        public DateTime Date { get => date; set => date = value; }

        public void CalculateTotalHours() => TotalHours = (checkoutTime - checkinTime).TotalHours;

        public override string ToString() 
            => $"IdUser: {IdUser}\nCheckinTime: {CheckinTime}" +
               $"\nCheckoutTime: {CheckoutTime}\nTotalHours: {TotalHours}\nDate: {Date}%";
    }
}
