using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using progettoUMRidolfiPagani.Services.Interface;

namespace progettoUMRidolfiPagani.Controllers
{
    public class MovimentiController : Controller
    {
        private readonly IMovimentoService _movimentoService;
        private readonly IArticoloService _articoloService;

        public MovimentiController(IMovimentoService movimentoService, IArticoloService articoloService)
        {
            _movimentoService = movimentoService;
            _articoloService = articoloService;
        }

        // GET: Movimenti
        public async Task<IActionResult> Index()
        {
            var movimenti = await _movimentoService.GetAllMovimentiAsync();
            return View(movimenti);
        }

        // GET: Movimenti/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var movimento = await _movimentoService.GetMovimentoByIdAsync(id);
            if (movimento == null)
            {
                return NotFound();
            }
            return View(movimento);
        }

        // GET: Movimenti/Ingresso
        public async Task<IActionResult> Ingresso()
        {
            var articoli = await _articoloService.GetAllArticoliAsync();
            ViewBag.Articoli = articoli;
            return View();
        }

        // POST: Movimenti/Ingresso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ingresso([Bind("ArticoloId,Posizione,Quantita")] Movimento movimento)
        {
            if (ModelState.IsValid)
            {
                await _movimentoService.RegistraIngressoAsync(movimento.ArticoloId, movimento.Posizione, movimento.Quantita);
                return RedirectToAction(nameof(Index));
            }
            return View(movimento);
        }

        // GET: Movimenti/Uscita
        public async Task<IActionResult> Uscita()
        {
            var articoli = await _articoloService.GetAllArticoliAsync();
            ViewBag.Articoli = articoli;
            return View();
        }

        // POST: Movimenti/Uscita
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Uscita([Bind("ArticoloId,Quantita")] Movimento movimento)
        {
            if (ModelState.IsValid)
            {
                await _movimentoService.RegistraUscitaAsync(movimento.ArticoloId, movimento.Quantita);
                return RedirectToAction(nameof(Index));
            }
            return View(movimento);
        }

        // GET: Movimenti/Spostamento
        public async Task<IActionResult> Spostamento()
        {
            var articoli = await _articoloService.GetAllArticoliAsync();
            ViewBag.Articoli = articoli;
            return View();
        }

        // POST: Movimenti/Spostamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Spostamento([Bind("ArticoloId,Posizione,NuovaPosizione")] Movimento movimento)
        {
            if (ModelState.IsValid)
            {
                await _movimentoService.SpostaArticoloAsync(movimento.ArticoloId, movimento.Posizione, movimento.NuovaPosizione);
                return RedirectToAction(nameof(Index));
            }
            return View(movimento);
        }

        // GET: Movimenti/Storico/5
        public async Task<IActionResult> Storico(int id)
        {
            var storico = await _movimentoService.GetStoricoMovimentiAsync(id);
            if (storico == null)
            {
                return NotFound();
            }
            return View(storico);
        }
    }
}
