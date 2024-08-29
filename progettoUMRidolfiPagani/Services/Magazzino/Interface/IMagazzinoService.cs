public interface IMagazzinoService
{
    Task<IEnumerable<Posizione>> GetAllPosizioniAsync();
    Task<Posizione> GetPosizioneByIdAsync(int id);
    Task<bool> CheckDisponibilitaPosizioneAsync(int posizioneId);
    Task<Posizione> CreatePosizioneAsync(Posizione posizione);
    Task<Posizione> UpdatePosizioneAsync(Posizione posizione);
    Task DeletePosizioneAsync(int id);
    Task<bool> SpostaArticoloAsync(int articoloId, int nuovaPosizioneId);
    Task<IEnumerable<Articolo>> GetArticoliInPosizioneAsync(int posizioneId);
}
