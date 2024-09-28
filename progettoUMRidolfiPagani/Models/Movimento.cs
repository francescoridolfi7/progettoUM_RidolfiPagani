namespace progettoUMRidolfiPagani.Models
{
    public class Movimento
    {
        public int Id { get; set; }

        // Relazione con Articolo
        public int ArticoloId { get; set; } // Chiave esterna per Articolo
        public required Articolo Articolo { get; set; }  // Navigazione

        public TipoMovimento TipoMovimento { get; set; }  // Enum per tipo di movimento
        public int? PosizioneInizialeId { get; set; } // Chiave esterna per la Posizione Iniziale
        public Posizione? PosizioneIniziale { get; set; }
        public int? PosizioneFinaleId { get; set; } // Chiave esterna per la Posizione Finale
        public Posizione? PosizioneFinale { get; set; } 

        public DateTime DataMovimento { get; set; } // Data del movimento

        public int Quantita { get; set; }  // Quantità movimentata
    }

    // Enum per i tipi di movimento
    public enum TipoMovimento
    {
        Ingresso,  // 0
        Uscita,    // 1
        Spostamento  // 2
    }
}
