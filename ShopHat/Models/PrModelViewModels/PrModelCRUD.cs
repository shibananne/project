using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrModelViewModels
{
    public class PrModelCRUD
    {
        public int ID { get; set; }
        public string? ModelName { get; set; }
        public string? ImgModel { get; set; }

        public static implicit operator PrModelCRUD(PrModel _PrModel)
        {
            return new PrModelCRUD
            {
                ID = _PrModel.ID,
                ModelName = _PrModel.ModelName,
                ImgModel = _PrModel.ImgModel,
              
            };
        }

        public static implicit operator PrModel(PrModelCRUD vm)
        {
            return new PrModel
            {
                ID = vm.ID,
                ModelName = vm.ModelName,
                ImgModel = vm.ImgModel,
            };
        }
    }
}
