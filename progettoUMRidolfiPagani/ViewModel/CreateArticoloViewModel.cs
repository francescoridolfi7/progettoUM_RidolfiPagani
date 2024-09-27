using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.ViewModels
{
    public class CreateArticoloViewModel
    {
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        public int Quantita { get; set; }
        public string Stato { get; set; }
        public int? PosizioneId { get; set; }

        public List<Posizione> PosizioniLibere { get; set; }
    }


}
