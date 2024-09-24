namespace progettoUMRidolfiPagani.Models
{
    public class Posizione
    {
        public int Id { get; set; } // Identificativo unico della posizione

        public string CodicePosizione { get; set; } // Codice univoco della posizione nel magazzino

        public bool Occupata { get; set; } // Flag che indica se la posizione è occupata

        public int Quantita { get; set; } // Quantità di articoli nella posizione

        public ICollection<Articolo> Articoli { get; set; } // Lista degli articoli in questa posizione

    }
}
