
using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IArticoloService
    {
        Task<IEnumerable<Articolo>> GetAllAsync();
        Task<Articolo> GetByIdAsync(int id);
        Task<Articolo> CreateAsync(Articolo articolo);
        Task<Articolo> UpdateAsync(Articolo articolo);
        Task DeleteAsync(int id);
        Task<Articolo> GetByCodiceAsync(string codice);
        Task<IEnumerable<Movimento>> GetMovimentiByArticoloIdAsync(int id);
        Task<IEnumerable<Articolo>> GetByPosizioneAsync(string posizione);
        Task<int> GetArticoliCountAsync();
        Task<IEnumerable<Articolo>> GetArticoliInEsaurimentoAsync();
        Task<int> GetArticoliDifettosiCountAsync();
        Task<IEnumerable<Posizione>> GetPosizioniLibereAsync();
    }
}



