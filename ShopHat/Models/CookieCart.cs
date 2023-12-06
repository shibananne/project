namespace ShopHat.Models
{
    public class CookieCart
    {
        public int ID { get; set; }
        public int? ProductID { get; set; }
        public int? ProductQuantity { get; set; }
        public int? TotalPay { get; set; }
        public int? OrderId { get; set; }
        public int? SizeID { get; set; }
    }
}
