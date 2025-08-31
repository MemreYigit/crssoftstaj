namespace CrsSoft.Entities.Enums
{
    public class EnumOrderStatus
    {
        public enum OrderStatus
        {
            Created,
            Picking,
            Shipped,
            Cancelled,
            Delivered,
            Unpacked,
            UnSupplied,
        }
    }
}