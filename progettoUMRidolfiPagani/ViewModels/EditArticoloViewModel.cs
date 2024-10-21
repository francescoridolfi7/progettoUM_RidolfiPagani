using progettoUMRidolfiPagani.Models;
using System.ComponentModel.DataAnnotations;

namespace progettoUMRidolfiPagani.ViewModels
{
    public class EditArticoloViewModel
    {
        public int Id { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        
        [Required]
        public int Quantita { get; set; }
        public string Stato { get; set; }

        public int? PosizioneIdCorrente { get; set; }

        //La nuova posizione selezionata dall'utente
        [Required] 
        public int? PosizioneId { get; set; }

        //Lista delle posizioni libere per la selezione
        public List<Posizione> PosizioniLibere { get; set; }
    }

}
