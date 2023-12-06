namespace ShopHat.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string? Address { get; set; }
        public string? UserID { get; set; }
        public int? ProductId { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone_Number { get; set; }
        public bool? IsDone { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
