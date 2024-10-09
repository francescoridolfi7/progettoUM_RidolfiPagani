using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.ViewModels
{
    public class ArticoliViewModel
    {
        public int TotalArticoli { get; set; }
        public int ArticoliDifettosi { get; set; }
        public IEnumerable<Articolo>? ArticoliInEsaurimento { get; set; }
        public IEnumerable<Articolo>? Articoli { get; set; }
    }

}
