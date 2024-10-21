using System.ComponentModel.DataAnnotations;

namespace progettoUMRidolfiPagani.ViewModels
{
    public class DeleteArticoloViewModel
    {
        public int Id { get; set; }

        public string Codice { get; set; }

        public string Descrizione { get; set; }

        public int Quantita { get; set; }

        public string Stato { get; set; }

        public int? PosizioneIdCorrente { get; set; }

        public string CodicePosizioneCorrente { get; set; }

        [Required]
        public bool ConfermaUscita { get; set; }
    }
}
