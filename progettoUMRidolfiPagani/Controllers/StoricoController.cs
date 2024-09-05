using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using System.Threading.Tasks;
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
