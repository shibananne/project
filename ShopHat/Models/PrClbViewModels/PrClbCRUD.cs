using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrClbViewModels
{
    public class PrClbCRUD
    {
        public int ID { get; set; }
        public string? NameCLB { get; set; }
        public string? ImgCLB { get; set; }
        public string? Slug { get; set; }
        public int? IDMain { get; set; }
        public IFormFile? PrPath { get; set; }


        public static implicit operator PrClbCRUD(PrClb _PrClb)
        {
            return new PrClbCRUD
            {
                ID = _PrClb.ID,
                NameCLB = _PrClb.NameCLB,
                ImgCLB = _PrClb.ImgCLB,
                Slug = _PrClb.Slug,
              
            };
        }

        public static implicit operator PrClb(PrClbCRUD vm)
        {
            return new PrClb
            {
                ID = vm.ID,
                NameCLB = vm.NameCLB,
                ImgCLB = vm.ImgCLB,
                Slug = vm.Slug,
            };
        }
    }
}
