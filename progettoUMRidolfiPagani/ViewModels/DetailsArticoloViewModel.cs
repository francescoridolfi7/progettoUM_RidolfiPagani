namespace progettoUMRidolfiPagani.ViewModels
{
    public class DetailsArticoloViewModel
    {
        public int Id { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        public int Quantita { get; set; }
        public string Stato { get; set; }
        public List<MovimentoViewModel> Movimenti { get; set; } = new List<MovimentoViewModel>();
    }

    public class MovimentoViewModel
    {
        public DateTime DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public int Quantita { get; set; }
        public string PosizioneIniziale { get; set; }
        public string PosizioneFinale { get; set; }
    }
}
