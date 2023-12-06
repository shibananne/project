namespace ShopHat.Models
{
    public class Products
    {
        public int ID { get; set; }
        public string? NameProduct { get; set; }
        public string? Slug { get; set; }
        public int? BeforePrice { get; set; }
        public int? DisPrice { get; set; }
        public int? CateId { get; set; }
        public string? SizeId { get; set; }
        public int? SubId { get; set; }
        public int? ProductsCount { get; set; }
        public int? Quantity { get; set; }
        public string? ProductChildId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? BrandId { get; set; }
        public int? LeagueID { get; set; }
        public int? CLBID { get; set; }
        public int? ColorID { get; set; }
        public string? Material { get; set; }
        public string? ProductCare { get; set; }
        public int? AgeID { get; set; }
        public string? BacksideId { get; set; }
        public int? ModelID { get; set; }
        public int? RimID { get; set; }
        public string? ProductNumber { get; set; }
        public string? ImgProducts { get; set; }
        public string? ImgFull { get; set; }
        public bool? IsExclusives { get; set; }

    }
}
