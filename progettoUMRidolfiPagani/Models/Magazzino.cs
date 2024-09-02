namespace progettoUMRidolfiPagani.Models
{
    public class Magazzino
    {
        public int Id { get; set; }
        public string CodicePosizione { get; set; }  // Identificativo della posizione (es. "B56")
        public string Descrizione { get; set; }  // Descrizione della posizione
        public int CapacitaMassima { get; set; }  // Capacità massima 
        public ICollection<Articolo> Articoli { get; set; }  // Articoli attualmente in questa posizione
    }

}
