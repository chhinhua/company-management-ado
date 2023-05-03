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
    }
}
