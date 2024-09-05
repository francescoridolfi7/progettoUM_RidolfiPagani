using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.ViewModels
{
    public class DashboardViewModel
    {
        public int ArticoliTotali { get; set; }
        public int ArticoliInMagazzino { get; set; }
        public IEnumerable<Movimento> MovimentiRecenti { get; set; }
        public decimal MediaGiorniPermanenza { get; set; }
        public int ArticoliDifettosi { get; set; }
    }

}
