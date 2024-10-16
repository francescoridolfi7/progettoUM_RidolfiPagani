using progettoUMRidolfiPagani.Models;
using System.Collections.Generic;

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
        public IDictionary<string, int> MovimentiPerTipo { get; set; }
        public IEnumerable<MovimentoConteggio> ConteggiPerTipoMovimento { get; set; }

        public class MovimentoConteggio
        {
            public string TipoMovimento { get; set; }
            public int Conteggio { get; set; }
        }
    }
}
