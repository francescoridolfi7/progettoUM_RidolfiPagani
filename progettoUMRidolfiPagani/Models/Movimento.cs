namespace progettoUMRidolfiPagani.Models
{
    public class Movimento
    {
        public int Id { get; set; }
        public int ArticoloId { get; set; }
        public Articolo Articolo { get; set; }  // Relazione con l'articolo
        public string TipoMovimento { get; set; }  // "Ingresso", "Uscita", "Spostamento"
        public string PosizioneIniziale { get; set; }  // Posizione iniziale nel magazzino (es. "B56")
        public string PosizioneFinale { get; set; }  // Posizione finale nel magazzino (es. "H41")
        public DateTime DataMovimento { get; set; }
        public TimeSpan? TempoPermanenza { get; set; }  // Calcolato per permanenze lunghe
    }

}