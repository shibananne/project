using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShopHat.Models.PrLeagueViewModels
{
    public class PrLeagueCRUD
    {
        public int ID { get; set; }
        public string? LeagueName { get; set; }
        public string? ImgLeague { get; set; }
    
    public static implicit operator PrLeagueCRUD(PrLeague _PrLeague)
        {
            return new PrLeagueCRUD
            {
                ID = _PrLeague.ID,
                LeagueName = _PrLeague.LeagueName,
                ImgLeague = _PrLeague.ImgLeague,
              
            };
        }

        public static implicit operator PrLeague(PrLeagueCRUD vm)
        {
            return new PrLeague
            {
                ID = vm.ID,
                LeagueName = vm.LeagueName,
                ImgLeague = vm.ImgLeague,
            };
        }
    }
}
