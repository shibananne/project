using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.SubCategoryViewModels
{
    public class SubCategoryCRUD
    {
        public int ID { get; set; }
        public string? NameSub { get; set; }
        public int? CateId { get; set; }
        public string? ImgSubCate { get; set; }
        public string? Slug { get; set; }

        public IFormFile? PrPath { get; set; }


        public static implicit operator SubCategoryCRUD(SubCategory _PrModel)
        {
            return new SubCategoryCRUD
            {
                ID = _PrModel.ID,
                NameSub = _PrModel.NameSub,
                CateId = _PrModel.CateId,
                ImgSubCate = _PrModel.ImgSubCate,
                Slug = _PrModel.Slug,
              
            };
        }

        public static implicit operator SubCategory(SubCategoryCRUD vm)
        {
            return new SubCategory
            {
                ID = vm.ID,
                NameSub = vm.NameSub,
                CateId = vm.CateId,
                ImgSubCate = vm.ImgSubCate,
                Slug = vm.Slug,
            };
        }
    }
}
