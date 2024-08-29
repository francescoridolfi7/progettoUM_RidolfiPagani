using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.ViewModels;
using System.Threading.Tasks;

namespace progettoUMRidolfiPagani.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IArticoloService _articoloService;
		private readonly IMovimentoService _movimentoService;
		private readonly IMagazzinoService _magazzinoService;

		public DashboardController(IArticoloService articoloService, IMovimentoService movimentoService, IMagazzinoService magazzinoService)
		{
			_articoloService = articoloService;
			_movimentoService = movimentoService;
			_magazzinoService = magazzinoService;
		}

		// GET: Dashboard
		public async Task<IActionResult> Index()
		{
			var dashboardViewModel = new DashboardViewModel
			{
				NumeroTotaleArticoli = await _articoloService.GetNumeroTotaleArticoliAsync(),
				NumeroTotaleMovimenti = await _movimentoService.GetNumeroTotaleMovimentiAsync(),
				NumeroPosizioniDisponibili = await _magazzinoService.GetNumeroPosizioniDisponibiliAsync(),
				ArticoliInEsaurimento = await _articoloService.GetArticoliInEsaurimentoAsync(),
				GraficoMovimenti = await _movimentoService.GetDatiGraficoMovimentiAsync(),
				MediaGiorniPermanenza = await _movimentoService.GetMediaGiorniPermanenzaAsync()
			};

			return View(dashboardViewModel);
		}

		// GET: Dashboard/ArticoliInEsaurimento
		public async Task<IActionResult> ArticoliInEsaurimento()
		{
			var articoliInEsaurimento = await _articoloService.GetArticoliInEsaurimentoAsync();
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
			var datiGrafico = await _movimentoService.GetDatiGraficoMovimentiAsync();
			return Json(datiGrafico); // Restituisce i dati come JSON per l'integrazione con un grafico frontend
		}
	}
}
