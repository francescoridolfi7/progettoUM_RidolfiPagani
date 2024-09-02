using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IStoricoService
    {
        Task<IEnumerable<Movimento>> GetMovimentiByArticoloIdAsync(int articoloId);
        Task<IEnumerable<Movimento>> GetMovimentiByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<TimeSpan> CalcolaTempoPermanenzaAsync(int articoloId);
        Task<decimal> CalcolaMediaGiorniPermanenzaAsync();
        Task<IEnumerable<Articolo>> GetArticoliPiuVecchiAsync(int count);
        Task<IEnumerable<Movimento>> GetMovimentiRecentiAsync(int days);
    }
}

