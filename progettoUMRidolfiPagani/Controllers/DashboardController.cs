using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Services.Interface;
using System.Threading.Tasks;

namespace progettoUMRidolfiPagani.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMovimentoService _movimentoService;
        private readonly IMagazzinoService _magazzinoService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
           
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                NumeroTotaleArticoli = await _dashboardService.GetDashboardDataAsync().Result.NumeroTotaleArticoli,
                NumeroTotaleMovimenti = await _dashboardService.GetDashboardDataAsync().Result.NumeroTotaleMovimenti,
                NumeroPosizioniDisponibili = await _dashboardService.GetDashboardDataAsync().Result.NumeroPosizioniDisponibili,
                ArticoliInEsaurimento = await _dashboardService.GetDashboardDataAsync().Result.ArticoliInEsaurimento,
                GraficoMovimenti = await _dashboardService.GetDashboardDataAsync().Result.GraficoMovimenti,
                MediaGiorniPermanenza = await _dashboardService.GetDashboardDataAsync().Result.MediaGiorniPermanenza
            };

            return View(dashboardViewModel);
        }

        // GET: Dashboard/ArticoliInEsaurimento
        public async Task<IActionResult> ArticoliInEsaurimento()
        {
            var articoliInEsaurimento = await _dashboardService.GetDashboardDataAsync().Result.ArticoliInEsaurimento;
            return View(articoliInEsaurimento);
        }

        // GET: Dashboard/StatisticheMovimenti
        public async Task<IActionResult> StatisticheMovimenti()
        {
            var statisticheMovimenti = await _movimentoService.GetStatisticheMovimentiAsync();
            return View(statisticheMovimenti);
        }

        // GET: Dashboard/GraficoMovimenti
        public async Task<IActionResult> GraficoMovimenti()
        {
            var datiGrafico = await _dashboardService.GetDashboardDataAsync().Result.GraficoMovimenti;
            return Json(datiGrafico); // Restituisce i dati come JSON per l'integrazione con un grafico frontend
        }
    }
}
