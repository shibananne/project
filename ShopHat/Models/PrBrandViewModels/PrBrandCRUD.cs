using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrBrandViewModels
{
    public class PrBrandCRUD
    {
        public int ID { get; set; }
        public string? NameBrand { get; set; }
        public string? ImgBrand { get; set; }
        public string? Slug { get; set; }
        public IFormFile? PrPath { get; set; }


        public static implicit operator PrBrandCRUD(PrBrand _PrBrand)
        {
            return new PrBrandCRUD
            {
                ID = _PrBrand.ID,
                NameBrand = _PrBrand.NameBrand,
                ImgBrand = _PrBrand.ImgBrand,
                Slug = _PrBrand.Slug,
              
            };
        }

        public static implicit operator PrBrand(PrBrandCRUD vm)
        {
            return new PrBrand
            {
                ID = vm.ID,
                NameBrand = vm.NameBrand,
                ImgBrand = vm.ImgBrand,
                Slug = vm.Slug,
            };
        }
    }
}
