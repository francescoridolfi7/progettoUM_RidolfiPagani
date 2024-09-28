using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Services.Interface;
using progettoUMRidolfiPagani.ViewModels;
using System.Threading.Tasks;

namespace progettoUMRidolfiPagani.Controllers
{
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        private readonly IDashboardService _dashboardService = dashboardService;
        private readonly IMovimentoService _movimentoService;

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
                //MediaGiorniPermanenza = await _dashboardService.GetMediaGiorniPermanenzaAsync()
            };

            return View(dashboardViewModel);
        }

        // GET: Dashboard/ArticoliInEsaurimento
        public async Task<IActionResult> ArticoliInEsaurimento()
        {
            var articoliInEsaurimento = await _dashboardService.GetArticoliInEsaurimentoAsync();
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
            var datiGrafico = await _dashboardService.GetDatiGraficoMovimentiAsync();
            return Json(datiGrafico); // Restituisce i dati come JSON per l'integrazione con un grafico frontend
        }
    }
}
