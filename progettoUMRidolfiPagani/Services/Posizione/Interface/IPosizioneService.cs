
using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IPosizioneService
    {
        Task<IEnumerable<Posizione>> GetAllPosizioniAsync();
        Task<Posizione> GetPosizioneByIdAsync(int id);
        Task<bool> CheckDisponibilitaPosizioneAsync(int posizioneId);
        Task<Posizione> CreatePosizioneAsync(Posizione posizione);
        Task<Posizione> UpdatePosizioneAsync(Posizione posizione);
        Task DeletePosizioneAsync(int id);
        Task<bool> SpostaArticoloAsync(int articoloId, int nuovaPosizioneId);
        Task<IEnumerable<Articolo>> GetArticoliInPosizioneAsync(int posizioneId);
        Task<int> GetPosizioniDisponibiliCountAsync();
        Task<Posizione> GetPosizioneByCodiceArticoloAsync(string codiceArticolo);
        Task<int> GetArticoliInMagazzinoCountAsync();
        Task UpdateQuantitaPosizioneAsync(int id, int nuovaQuantita);
        Task<IEnumerable<Movimento>> GetStoricoMovimentiPosizioneAsync(int id);
        Task<int> GetArticoliInPosizioneCountAsync();

    }

}
