
using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IArticoloService
    {
        Task<IEnumerable<Articolo>> GetAllAsync();
        Task<Articolo> GetByIdAsync(int id);
        Task<Articolo> CreateAsync(Articolo articolo);
        Task<Articolo> UpdateAsync(Articolo articolo, int? nuovaPosizioneId, int? posizioneIdCorrente);
        Task DeleteAsync(int articoloId, int quantitaDaUscire);
        Task<Articolo> GetByCodiceAsync(string codice);
        Task<List<Movimento>> GetMovimentiByArticoloIdAsync(int articoloId);
        Task<IEnumerable<Articolo>> GetByPosizioneAsync(string posizione);
        Task<int> GetArticoliCountAsync();
        Task<IEnumerable<Articolo>> GetArticoliInEsaurimentoAsync();
        Task<int> GetArticoliDifettosiCountAsync();
        Task<IEnumerable<Posizione>> GetPosizioniLibereAsync();
        Task<Posizione> GetPosizioneByIdAsync(int id);
        Task<Articolo> GetArticoloPiuVecchioAsync();
        Task RiparaArticoloAsync(int id);
    }
}



