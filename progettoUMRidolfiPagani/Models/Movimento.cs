namespace progettoUMRidolfiPagani.Models
{
    public class Movimento
    {
        public int Id { get; set; }
        public required Articolo Articolo { get; set; }  // Relazione con l'articolo

        public TipoMovimento TipoMovimento { get; set; }  // "Ingresso", "Uscita", "Spostamento"

        public required Posizione PosizioneIniziale { get; set; }
        public Posizione? PosizioneFinale { get; set; }

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