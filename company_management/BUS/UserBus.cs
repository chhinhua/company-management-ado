using company_management.DTO;

namespace company_management.BUS
{
    public class UserBus
    {
        public bool IsEmployee()
        {
            int positionId = UserSession.LoggedInUser.IdPosition;
            return positionId == 3;
        }

        public bool IsManager()
        {
            int positionId = UserSession.LoggedInUser.IdPosition;
            return positionId == 1;
        }
        
        public bool IsHumanResources()
        {
            int positionId = UserSession.LoggedInUser.IdPosition;
            return positionId == 4;
        }
        
        public bool IsLeader()
        {
            int positionId = UserSession.LoggedInUser.IdPosition;
            return positionId == 2;
        }

        public string GetPosition(User user)
        {
            string position = null;
            switch (user.IdPosition)
            {
                case 1:
                    position = "Quản lý";
                    break;
                case 2:
                    position = "Trưởng nhóm";
                    break;
                case 3:
                    position = "Nhân viên";
                    break;
                case 4:
                    position = "Human Resources (HR)";
                    break;
            }
            return position;
        } 
    }
}
