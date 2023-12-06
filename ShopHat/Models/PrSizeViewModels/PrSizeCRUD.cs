using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrSizeViewModels
{
    public class PrSizeCRUD
    {
        public int ID { get; set; }
        public string? NameSize { get; set; }

        public static implicit operator PrSizeCRUD(PrSize _PrModel)
        {
            return new PrSizeCRUD
            {
                ID = _PrModel.ID,
                NameSize = _PrModel.NameSize,
         
              
            };
        }

        public static implicit operator PrSize(PrSizeCRUD vm)
        {
            return new PrSize
            {
                ID = vm.ID,
                NameSize = vm.NameSize,
               
            };
        }
    }
}
