public class ArticoloDifettoso
{
    public int Id { get; set; }
    public int ArticoloId { get; set; }
    public Articolo Articolo { get; set; }
    public string DescrizioneDifetto { get; set; }
    public DateTime DataSegnalazione { get; set; }
    public string Stato { get; set; }  // "In Attesa", "Riparato", "Scartato"
}
