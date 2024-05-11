namespace API.Utility
{
    public static class SD
    {
        public const string REQUESTED_BY_CUSTOMER = "Requested by customer";
        public enum UserRoles
        {
            ADMIN,
            CUSTOMER,
            MANAGER
        }

        public enum OrderStatus
        {
            PENDING,
            APPROVED,
            READYTOSHIP,
            COMPLETED,
            CANCELED
        }
    }
}
