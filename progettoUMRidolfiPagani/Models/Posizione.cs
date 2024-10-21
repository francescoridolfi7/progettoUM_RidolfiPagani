namespace progettoUMRidolfiPagani.Models
{
    public class Posizione
    {
        public int Id { get; set; } 

        public string CodicePosizione { get; set; } 

        public bool Occupata { get; set; } 

        public int Quantita { get; set; }

        public ICollection<Articolo> Articoli { get; set; }

    }
}
