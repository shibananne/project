using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrBacksideModels
{
    public class PrBacksideCRUD
    {
        public int ID { get; set; }
        public string? BacksideName { get; set; }

        public static implicit operator PrBacksideCRUD(PrBackside _PrBackside)
        {
            return new PrBacksideCRUD
            {
                ID = _PrBackside.ID,
                BacksideName = _PrBackside.BacksideName,
              
            };
        }

        public static implicit operator PrBackside(PrBacksideCRUD vm)
        {
            return new PrBackside
            {
                ID = vm.ID,
                BacksideName = vm.BacksideName,
            };
        }
    }
}
