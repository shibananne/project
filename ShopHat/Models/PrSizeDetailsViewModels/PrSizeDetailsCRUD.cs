using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrSizeDetailsViewModels
{
    public class PrRimCRUD
    {
        public int ID { get; set; }
        public int? NameSizeId { get; set; }
        public int? ProductId { get; set; }
        public bool? OutOF { get; set; }

        public static implicit operator PrRimCRUD(PrSizeDetails _PrModel)
        {
            return new PrRimCRUD
            {
                ID = _PrModel.ID,
                NameSizeId = _PrModel.NameSizeId,
                ProductId = _PrModel.ProductId,
                OutOF = _PrModel.OutOF,
              
            };
        }

        public static implicit operator PrSizeDetails(PrRimCRUD vm)
        {
            return new PrSizeDetails
            {
                ID = vm.ID,
                NameSizeId = vm.NameSizeId,
                ProductId = vm.ProductId,
                OutOF = vm.OutOF,
            };
        }
    }
}
