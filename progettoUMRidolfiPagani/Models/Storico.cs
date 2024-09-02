public class Storico
{
    public int Id { get; set; }
    public int ArticoloId { get; set; }
    public Articolo Articolo { get; set; }
    public string DescrizioneOperazione { get; set; }  // Descrive l'operazione eseguita
    public DateTime DataOperazione { get; set; }  // Data e ora dell'operazione

}
