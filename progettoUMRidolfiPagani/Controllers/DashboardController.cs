using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Services.Interface;
using progettoUMRidolfiPagani.ViewModels;
using System.Threading.Tasks;

namespace progettoUMRidolfiPagani.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                NumeroTotaleArticoli = await _dashboardService.GetNumeroTotaleArticoliAsync(),
                NumeroTotaleMovimenti = await _dashboardService.GetNumeroTotaleMovimentiAsync(),
                NumeroPosizioniDisponibili = await _dashboardService.GetNumeroPosizioniDisponibiliAsync(),
                ArticoliInEsaurimento = await _dashboardService.GetArticoliInEsaurimentoAsync(),
                GraficoMovimenti = await _dashboardService.GetDatiGraficoMovimentiAsync(),
            };

            return View(dashboardViewModel);
        }

        // Nuova azione per ottenere i dati della dashboard
        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();
            return Json(dashboardData);
        }
    }
}
