using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.TermsPrViewModels
{
    public class TermsPrCRUD
    {
        public int ID { get; set; }
        public string? Terms { get; set; }
        public string? AboutMain { get; set; }
        public string? AboutPage { get; set; }
        public string? RefunPr { get; set; }
        public string? Faq { get; set; }



        public static implicit operator TermsPrCRUD(TermsPr _PrModel)
        {
            return new TermsPrCRUD
            {
                ID = _PrModel.ID,
                Terms = _PrModel.Terms,
                AboutMain = _PrModel.AboutMain,
                AboutPage = _PrModel.AboutPage,
                RefunPr = _PrModel.RefunPr,
                Faq = _PrModel.Faq,
              
            };
        }

        public static implicit operator TermsPr(TermsPrCRUD vm)
        {
            return new TermsPr
            {
                ID = vm.ID,
                Terms = vm.Terms,
                AboutMain = vm.AboutMain,
                AboutPage = vm.AboutPage,
                RefunPr = vm.RefunPr,
                Faq = vm.Faq,
            };
        }
    }
}
