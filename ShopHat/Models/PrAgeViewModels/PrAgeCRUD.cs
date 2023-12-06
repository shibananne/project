using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrAgeViewModels
{
    public class PrAgeCRUD
    {
        public int ID { get; set; }
        public string? NameAge { get; set; }

        public static implicit operator PrAgeCRUD(PrAge _Age)
        {
            return new PrAgeCRUD
            {
                ID = _Age.ID,
                NameAge = _Age.NameAge,
              
            };
        }

        public static implicit operator PrAge(PrAgeCRUD vm)
        {
            return new PrAge
            {
                ID = vm.ID,
                NameAge = vm.NameAge,
            };
        }
    }
}
