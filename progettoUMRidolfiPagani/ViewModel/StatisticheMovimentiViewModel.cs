namespace progettoUMRidolfiPagani.ViewModels
{
    public class StatisticheMovimentiViewModel
    {
        public int NumeroTotaleMovimenti { get; set; }
        public double MediaGiorniPermanenza { get; set; }
        public int MovimentiRecenti { get; set; }
        public IDictionary<string, int> MovimentiPerTipo { get; set; }

        public IEnumerable<MovimentoConteggio> ConteggiPerTipoMovimento { get; set; }

        public class MovimentoConteggio
        {
            public string TipoMovimento { get; set; }
            public int Conteggio { get; set; }
        }
    }
}
