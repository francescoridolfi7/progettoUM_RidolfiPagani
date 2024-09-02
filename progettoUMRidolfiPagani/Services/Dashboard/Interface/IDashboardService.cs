namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}