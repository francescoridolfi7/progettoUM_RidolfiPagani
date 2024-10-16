namespace progettoUMRidolfiPagani.Models
{
    public class Utente
    {
        public int Id { get; set; }  
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } 
    }


}
