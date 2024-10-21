namespace progettoUMRidolfiPagani.Models
{
    public class Movimento
    {
        public int Id { get; set; }

        // Relazione con Articolo
        public int ArticoloId { get; set; } 
        public required Articolo Articolo { get; set; }  

        public TipoMovimento TipoMovimento { get; set; } 
        public int? PosizioneInizialeId { get; set; } 
        public Posizione? PosizioneIniziale { get; set; }
        public int? PosizioneFinaleId { get; set; } 
        public Posizione? PosizioneFinale { get; set; } 

        public DateTime DataMovimento { get; set; } = DateTime.Now;

        public int Quantita { get; set; }
    }

    public enum TipoMovimento
    {
        Ingresso,  // 0
        Spostamento,    // 1
        Uscita  // 2
    }
}
