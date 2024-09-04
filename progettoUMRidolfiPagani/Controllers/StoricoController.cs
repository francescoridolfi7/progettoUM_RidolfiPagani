using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using System.Threading.Tasks;
using progettoUMRidolfiPagani.ViewModels;
using progettoUMRidolfiPagani.Services.Interface;

namespace progettoUMRidolfiPagani.Controllers
{
    public class StoricoController : Controller
    {
        private readonly IStoricoService _storicoService;

        public StoricoController(IStoricoService storicoService)
        {
            _storicoService = storicoService;
        }

        // GET: Storico
        public async Task<IActionResult> Index()
        {
            var storico = await _storicoService.GetStoricoCompletoAsync();
            return View(storico);
        }

        // GET: Storico/Articolo/5
        public async Task<IActionResult> Articolo(int id)
        {
            var storicoArticolo = await _storicoService.GetStoricoByArticoloIdAsync(id);
            if (storicoArticolo == null)
            {
                return NotFound($"Non sono stati trovati movimenti per l'articolo con ID {id}.");
            }
            return View(storicoArticolo);
        }

        // GET: Storico/Filtra
        public IActionResult Filtra()
        {
            return View();
        }

        // POST: Storico/Filtra
        [HttpPost]
        public async Task<IActionResult> Filtra(StoricoFiltraViewModel filtro)
        {
            if (ModelState.IsValid)
            {
                var storicoFiltrato = await _storicoService.GetStoricoFiltratoAsync(filtro);
                return View("Index", storicoFiltrato);
            }
            return View(filtro);
        }

        // GET: Storico/Dettagli/5
        public async Task<IActionResult> Dettagli(int id)
        {
            var movimentoDettagli = await _storicoService.GetMovimentoDettagliByIdAsync(id);
            if (movimentoDettagli == null)
            {
                return NotFound($"Non sono stati trovati dettagli per il movimento con ID {id}.");
            }
            return View(movimentoDettagli);
        }
    }
}
