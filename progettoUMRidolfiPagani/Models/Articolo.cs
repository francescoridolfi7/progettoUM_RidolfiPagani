namespace progettoUMRidolfiPagani.Models
{
    public class Articolo
    {
        public int Id { get; set; }
        public string Codice { get; set; }  // Codice a barre (es. "N51")
        public string Descrizione { get; set; }
        public DateTime DataArrivo { get; set; }
        public DateTime? DataUscita { get; set; }
        public string Stato { get; set; }  // "In Magazzino","Difettoso"
        public int Quantita { get; set; }  // Quantità di articoli 
        public Posizione Posizione { get; set; }
        public int PosizioneId { get; set; }
        public ICollection<Movimento> Movimenti { get; set; }  // Storico dei movimenti dell'articolo
    }

}