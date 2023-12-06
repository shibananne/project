using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.CokieUserViewModels
{
    public class CokieUserCRUD
    {
        public int ID { get; set; }
        public string? CokieUserMain { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? FullAddress { get; set; }
        public int? City { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }
        public string? DetailsAddress { get; set; }
        public string? Phone { get; set; }


        public List<string>? ListProduct { get; set; }

        public static implicit operator CokieUserCRUD(CokieUser _PrModel)
        {
            return new CokieUserCRUD
            {
                ID = _PrModel.ID,
                CokieUserMain = _PrModel.CokieUserMain,
                FullName = _PrModel.FullName,
                Email = _PrModel.Email,
                FullAddress = _PrModel.FullAddress,
                City = _PrModel.City,
                District = _PrModel.District,
                Ward = _PrModel.Ward,
                DetailsAddress = _PrModel.DetailsAddress,
               
              
            };
        }

        public static implicit operator CokieUser(CokieUserCRUD vm)
        {
            return new CokieUser
            {
                ID = vm.ID,
                CokieUserMain = vm.CokieUserMain,
                FullName = vm.FullName,
                Email = vm.Email,
                FullAddress = vm.FullAddress,
                City = vm.City,
                District = vm.District,
                Ward = vm.Ward,
                DetailsAddress = vm.DetailsAddress,

            };
        }
    }
}
