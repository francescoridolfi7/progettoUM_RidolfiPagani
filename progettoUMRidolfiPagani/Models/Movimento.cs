namespace progettoUMRidolfiPagani.Models
{
    public class Movimento
    {
        public int Id { get; set; }
        public int ArticoloId { get; set; }
        public Articolo Articolo { get; set; }  // Relazione con l'articolo

        public TipoMovimento TipoMovimento { get; set; }  // "Ingresso", "Uscita", "Spostamento"

        // Relazione con la Posizione Iniziale
        public int? PosizioneInizialeId { get; set; }
        public Posizione PosizioneIniziale { get; set; }

        // Relazione con la Posizione Finale
        public int? PosizioneFinaleId { get; set; }
        public Posizione PosizioneFinale { get; set; }

        public DateTime DataMovimento { get; set; }
        public TimeSpan? TempoPermanenza { get; set; }  // Calcolato per permanenze lunghe
        public int Quantita { get; set; }  // Quantità di articoli movimentati
    }

    public enum TipoMovimento
    {
        Ingresso,
        Uscita,
        Spostamento
    }

}