using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.CookieCartViewModels
{
    public class CookieCartCRUD
    {
        public int ID { get; set; }
        public int? ProductID { get; set; }
        public int? ProductQuantity { get; set; }
        public int? TotalPay { get; set; }
        public int? OrderId { get; set; }
        public int? SizeID { get; set; }

        public List<string>? ListProduct { get; set; }

        public static implicit operator CookieCartCRUD(CookieCart _PrModel)
        {
            return new CookieCartCRUD
            {
                ID = _PrModel.ID,
                ProductID = _PrModel.ProductID,
                ProductQuantity = _PrModel.ProductQuantity,
                TotalPay = _PrModel.TotalPay,
                OrderId = _PrModel.OrderId,
                SizeID = _PrModel.SizeID,
               
              
            };
        }

        public static implicit operator CookieCart(CookieCartCRUD vm)
        {
            return new CookieCart
            {
                ID = vm.ID,
                ProductID = vm.ProductID,
                ProductQuantity = vm.ProductQuantity,
                TotalPay = vm.TotalPay,
                OrderId = vm.OrderId,
                SizeID = vm.SizeID,

            };
        }
    }
}
