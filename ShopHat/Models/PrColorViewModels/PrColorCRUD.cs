using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrColorViewModels
{
    public class PrColorCRUD
    {
        public int ID { get; set; }
        public string? NameColor { get; set; }
        public string? ImgColor { get; set; }
        public IFormFile? PrPath { get; set; }

        public static implicit operator PrColorCRUD(PrColor _PrModel)
        {
            return new PrColorCRUD
            {
                ID = _PrModel.ID,
                NameColor = _PrModel.NameColor,
                ImgColor = _PrModel.ImgColor,
         
         
              
            };
        }

        public static implicit operator PrColor(PrColorCRUD vm)
        {
            return new PrColor
            {
                ID = vm.ID,
                NameColor = vm.NameColor,
                ImgColor = vm.ImgColor,

            };
        }
    }
}
