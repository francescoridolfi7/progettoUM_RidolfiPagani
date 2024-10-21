using progettoUMRidolfiPagani.Models;
using progettoUMRidolfiPagani.ViewModels;

namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
        Task<int> GetNumeroTotaleArticoliAsync();
        Task<int> GetNumeroTotaleMovimentiAsync();
        Task<int> GetNumeroPosizioniDisponibiliAsync();
        Task<IEnumerable<Articolo>> GetArticoliInEsaurimentoAsync();
        Task<IEnumerable<Movimento>> GetDatiGraficoMovimentiAsync();
    }
}