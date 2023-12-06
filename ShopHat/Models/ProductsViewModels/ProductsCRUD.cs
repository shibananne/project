using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.ProductsViewModels
{
    public class ProductsCRUD
    {
        public int ID { get; set; }
        public string? NameProduct { get; set; }
        public string? Slug { get; set; }
        public string? SlugBrand { get; set; }
        public int? BeforePrice { get; set; }
        public int? DisPrice { get; set; }
        public int? CateId { get; set; }
        public string? SizeId { get; set; }
        public int? SubId { get; set; }
        public int? ProductsCount { get; set; }
        public int? TotalPrices { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityCient { get; set; }
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
        public int? IdDetails { get; set; }
        public string? ProductNumber { get; set; }
        public string? ImgProducts { get; set; }
        public string? CateName { get; set; }
        public string? SubName { get; set; }
        public string? BrandsName { get; set; }
        public string? ClbName { get; set; }
        public string? AgeName { get; set; }
        public string? RimName { get; set; }
        public string? ImgFull { get; set; }
        public string? BrandIcon { get; set; }
        public string? PrPathFullEmpty { get; set; }
        public string? SelectSizeId { get; set; }
        public int? OrderId { get; set; }
        public bool? IsExclusives { get; set; }
        public string? ColorName { get; set; }

        public IFormFile? PrPath { get; set; }
        public List<IFormFile>? PrPathFull { get; set; }
        public List<string>? SizeFull { get; set; }

        public List<string> ImgFullList { get; set; }

        public string? CustomerName { get; set; }
        public string? Phone_Number { get; set; }
        public string? Address { get; set; }
        public DateTime? CreateOrder { get; set; }
        public BrandObj Brand { get; set; }
        public AgeObj Age { get; set; }
        public ClbObj Clb { get; set; }
        public RimObj Rim { get; set; }

        public class BrandObj
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
        }

        public class AgeObj
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
        }

        public class ClbObj
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
        }

        public class RimObj
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
        }
        public static implicit operator ProductsCRUD(Products _Products)
        {
            return new ProductsCRUD
            {
                ID = _Products.ID,
                NameProduct = _Products.NameProduct,
                Slug = _Products.Slug,
                BeforePrice = _Products.BeforePrice,
                DisPrice = _Products.DisPrice,
                CateId = _Products.CateId,
                SizeId = _Products.SizeId,
                SubId = _Products.SubId,
                ProductsCount = _Products.ProductsCount,
                Quantity = _Products.Quantity,
                ProductChildId = _Products.ProductChildId,
                CreateDate = _Products.CreateDate,
                BrandId = _Products.BrandId,
                LeagueID = _Products.LeagueID,
                CLBID = _Products.CLBID,
                ColorID = _Products.ColorID,
                Material = _Products.Material,
                ProductCare = _Products.ProductCare,
                AgeID = _Products.AgeID,
                BacksideId = _Products.BacksideId,
                ModelID = _Products.ModelID,
                RimID = _Products.RimID,
                ProductNumber = _Products.ProductNumber,
                ImgProducts = _Products.ImgProducts,
                IsExclusives = _Products.IsExclusives,

            };
        }

        public static implicit operator Products(ProductsCRUD vm)
        {
            return new Products
            {
                ID = vm.ID,
                NameProduct = vm.NameProduct,
                Slug = vm.Slug,
                BeforePrice = vm.BeforePrice,
                DisPrice = vm.DisPrice,
                CateId = vm.CateId,
                SizeId = vm.SizeId,
                SubId = vm.SubId,
                ProductsCount = vm.ProductsCount,
                Quantity = vm.Quantity,
                ProductChildId = vm.ProductChildId,
                CreateDate = vm.CreateDate,
                BrandId = vm.BrandId,
                LeagueID = vm.LeagueID,
                CLBID = vm.CLBID,
                ColorID = vm.ColorID,
                Material = vm.Material,
                ProductCare = vm.ProductCare,
                AgeID = vm.AgeID,
                BacksideId = vm.BacksideId,
                ModelID = vm.ModelID,
                RimID = vm.RimID,
                ProductNumber = vm.ProductNumber,
                ImgProducts = vm.ImgProducts,
                IsExclusives = vm.IsExclusives,
            };
        }
    }
}
