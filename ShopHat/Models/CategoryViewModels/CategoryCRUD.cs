using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.CategoryViewModels
{
    public class CategoryCRUD
    {
        public int ID { get; set; }
        public string? NameCate { get; set; }
        public string? Slug { get; set; }

        public static implicit operator CategoryCRUD(Category _Cate)
        {
            return new CategoryCRUD
            {
                ID = _Cate.ID,
                NameCate = _Cate.NameCate,
                Slug = _Cate.Slug,
              
            };
        }

        public static implicit operator Category(CategoryCRUD vm)
        {
            return new Category
            {
                ID = vm.ID,
                NameCate = vm.NameCate,
                Slug = vm.Slug,
            };
        }
    }
}
