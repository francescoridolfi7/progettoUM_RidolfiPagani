
using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.ViewModels
{
    public class DashboardViewModel
    {
        public int NumeroTotaleArticoli { get; set; }
        public int NumeroTotaleMovimenti { get; set; }
        public int NumeroPosizioniDisponibili { get; set; }
        public IEnumerable<Articolo> ArticoliInEsaurimento { get; set; }
        public IEnumerable<Movimento> MovimentiRecenti { get; set; }
        public double MediaGiorniPermanenza { get; set; }
        public object GraficoMovimenti { get; set; }
        public int ArticoliInMagazzino { get; set; }
        public int ArticoliDifettosi { get; set; }
    }
}
