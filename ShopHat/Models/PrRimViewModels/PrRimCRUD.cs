using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrRimViewModels
{
    public class PrRimCRUD
    {
        public int ID { get; set; }
        public string? RimName { get; set; }

        public static implicit operator PrRimCRUD(PrRim _PrModel)
        {
            return new PrRimCRUD
            {
                ID = _PrModel.ID,
                RimName = _PrModel.RimName,
              
            };
        }

        public static implicit operator PrRim(PrRimCRUD vm)
        {
            return new PrRim
            {
                ID = vm.ID,
                RimName = vm.RimName,
            };
        }
    }
}
