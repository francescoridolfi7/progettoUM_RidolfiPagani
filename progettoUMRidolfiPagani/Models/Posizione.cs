namespace progettoUMRidolfiPagani.Models
{
    public class Posizione
    {
        public int Id { get; set; } // Identificativo unico della posizione

        public string Codice { get; set; } // Codice univoco della posizione nel magazzino

        public string Descrizione { get; set; } // Descrizione della posizione

        public bool Occupata { get; set; } // Flag che indica se la posizione è occupata

        public int Quantita { get; set; } // Quantità di articoli nella posizione

        public ICollection<Articolo> Articoli { get; set; } // Lista degli articoli in questa posizione

        public DateTime DataCreazione { get; set; } // Data di creazione della posizione

        public DateTime? DataUltimaModifica { get; set; } // Data dell'ultima modifica della posizione
    }
}
