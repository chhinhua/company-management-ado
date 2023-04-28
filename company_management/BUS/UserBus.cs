﻿using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;

using company_management.DTO;
using company_management.DAO;
using company_management.View;

namespace company_management.BUS
{
    public class UserBus
    {
        public UserBus() { }

        public string GetUserPosition()
        {
            int positionId = UserSession.LoggedInUser.IdPosition;
            switch (positionId)
            {
                case 1:
                    return "Manager";
                case 2:
                    return "Leader";
                default:
                    return "Employee";
            }
        }

        public bool IsEmployee()
        {
            int positionId = UserSession.LoggedInUser.IdPosition;
            return positionId != 1 && positionId != 2;
        }

        public bool IsManager()
        {
            int positionId = UserSession.LoggedInUser.IdPosition;
            return positionId != 2 && positionId != 3;
        }
    }
}