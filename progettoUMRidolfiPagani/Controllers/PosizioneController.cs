using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using progettoUMRidolfiPagani.Services.Interface;

namespace progettoUMRidolfiPagani.Controllers
{
    public class PosizioneController : Controller
    {
        private readonly IPosizioneService _posizioneService;
        private readonly IArticoloService _articoloService;

        public PosizioneController(IPosizioneService posizioneService, IArticoloService articoloService)
        {
            _posizioneService = posizioneService;
            _articoloService = articoloService;
        }

        // GET: Magazzino
        public async Task<IActionResult> Index()
        {
            var posizioni = await _posizioneService.GetAllPosizioniAsync();
            return View(posizioni);
        }

        // GET: Magazzino/DettagliPosizione/5
        public async Task<IActionResult> DettagliPosizione(int id)
        {
            var posizione = await _posizioneService.GetPosizioneByIdAsync(id);
            if (posizione == null)
            {
                return NotFound();
            }
            return View(posizione);
        }

        // GET: Magazzino/Ricerca
        public IActionResult Ricerca()
        {
            return View();
        }

        // POST: Magazzino/Ricerca
        [HttpPost]
        public async Task<IActionResult> Ricerca(string codiceArticolo)
        {
            var posizione = await _posizioneService.GetPosizioneByCodiceArticoloAsync(codiceArticolo);
            if (posizione == null)
            {
                return NotFound($"Articolo con codice {codiceArticolo} non trovato nel magazzino.");
            }
            return View("DettagliPosizione", posizione);
        }

        // GET: Magazzino/ControlloQuantita/5
        public async Task<IActionResult> ControlloQuantita(int id)
        {
            var posizione = await _posizioneService.GetPosizioneByIdAsync(id);
            if (posizione == null)
            {
                return NotFound();
            }
            return View(posizione);
        }

        // POST: Magazzino/AggiornaQuantita/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AggiornaQuantita(int id, int nuovaQuantita)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _posizioneService.UpdateQuantitaPosizioneAsync(id, nuovaQuantita);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Non è stato possibile aggiornare la quantità.");
                    return View("ControlloQuantita", await _posizioneService.GetPosizioneByIdAsync(id));
                }
                return RedirectToAction(nameof(Index));
            }
            return View("ControlloQuantita", await _posizioneService.GetPosizioneByIdAsync(id));
        }

        // GET: Magazzino/StoricoMovimenti/5
        public async Task<IActionResult> StoricoMovimenti(int id)
        {
            var storico = await _posizioneService.GetStoricoMovimentiPosizioneAsync(id);
            if (storico == null)
            {
                return NotFound();
            }
            return View(storico);
        }
    }
}
