namespace CrsSoft.Entities.Enums
{
    public class EnumOrderStatus
    {
        internal class EnumTextAttribute : Attribute
        {
            private string v;

            public EnumTextAttribute(string v)
            {
                this.v = v;
            }
        }
        public enum OrderStatus
        {
            [EnumText("Sevkiyat Oluştu")]
            Created,

            [EnumText("Toplanıyor")]
            Picking,

            [EnumText("Kargolandı / Gönderildi")]
            Shipped,

            [EnumText("İptal Edildi")]
            Cancelled,

            [EnumText("Teslim Edildi")]
            Delivered,

            [EnumText("Paket Açıldı")]
            Unpacked,

            [EnumText("Tedarik Edilemedi")]
            UnSupplied,
        }
    }
}