namespace API.Utility
{
    public static class SD
    {
        public enum UserRoles
        {
            ADMIN = 1,
            CUSTOMER = 2,
            MANAGER = 3
        }

        public enum OrderStatus
        {
            PENDING = 1,
            APPROVED = 2,
            READYTOSHIP = 3,
            COMPLETED = 4,
            CANCELED = 5
        }
    }
}
