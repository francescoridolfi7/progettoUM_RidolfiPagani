
using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IMovimentoService
    {
        Task<IEnumerable<Movimento>> GetAllAsync();
        Task<Movimento> GetByIdAsync(int id);
        Task<Movimento> CreateAsync(Movimento movimento);
        Task<Movimento> UpdateAsync(Movimento movimento);
        Task DeleteAsync(int id);
        Task<Movimento> SpostaArticoloAsync(int articoloId, int posizioneFinaleId);
        Task<IEnumerable<Movimento>> GetMovimentiByArticoloIdAsync(int articoloId);
        Task<IEnumerable<Movimento>> GetMovimentiRecentiAsync(int days);
    }
}
  
