using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using progettoUMRidolfiPagani.Services.Interface;
using progettoUMRidolfiPagani.ViewModels;

namespace progettoUMRidolfiPagani.Controllers
{
    public class ArticoliController : Controller
    {
        private readonly IArticoloService _articoloService;

        public ArticoliController(IArticoloService articoloService)
        {
            _articoloService = articoloService;
        }

        // GET: Articoli
        public async Task<IActionResult> Index()
        {
            // Ottieni il conteggio totale degli articoli
            var totalArticoli = await _articoloService.GetArticoliCountAsync();

            // Ottieni il conteggio degli articoli difettosi
            var articoliDifettosi = await _articoloService.GetArticoliDifettosiCountAsync();

            // Ottieni gli articoli in esaurimento
            var articoliInEsaurimento = await _articoloService.GetArticoliInEsaurimentoAsync();

            // Ottieni tutti gli articoli
            var articoli = await _articoloService.GetAllAsync();

            // Crea un ViewModel per passare i dati alla vista
            var viewModel = new ArticoliViewModel
            {
                TotalArticoli = totalArticoli,
                ArticoliDifettosi = articoliDifettosi,
                ArticoliInEsaurimento = articoliInEsaurimento,
                Articoli = articoli.ToList() 
            };

            return View(viewModel);
        }


        // GET: Articoli/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var articolo = await _articoloService.GetByIdAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }
            return View(articolo);
        }

        // GET: Articoli/Create
        public async Task<IActionResult> Create()
        {
            // Ottieni le posizioni libere
            var posizioniLibere = await _articoloService.GetPosizioniLibereAsync();

            // Crea un ViewModel per passare i dati alla vista Create
            var createViewModel = new CreateArticoloViewModel
            {
                PosizioniLibere = posizioniLibere.ToList()  // Convertiamo in lista
            };

            return View(createViewModel);
        }


        // POST: Articoli/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateArticoloViewModel model)
        {
            ModelState.Remove("PosizioniLibere");

            if (ModelState.IsValid)
            {
                var articolo = new Articolo
                {
                    Codice = model.Codice,
                    Descrizione = model.Descrizione,
                    Quantita = model.Quantita,
                    Stato = model.Stato,
                    PosizioneId = model.PosizioneId
                };

                await _articoloService.CreateAsync(articolo);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }





        // GET: Articoli/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var articolo = await _articoloService.GetByIdAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }
            return View(articolo);
        }

        // POST: Articoli/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codice,Descrizione,Quantita,Stato")] Articolo articolo)
        {
            if (id != articolo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _articoloService.UpdateAsync(articolo);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Non è stato possibile aggiornare l'articolo.");
                    return View(articolo);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(articolo);
        }

        // GET: Articoli/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var articolo = await _articoloService.GetByIdAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }
            return View(articolo);
        }

        // POST: Articoli/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _articoloService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Articoli/Search
        public IActionResult Search()
        {
            return View();
        }

        // GET: Articoli/GetByPosizione
        [HttpGet] 
        public async Task<IActionResult> GetByPosizione(string posizione)
        {
            var articolo = await _articoloService.GetByPosizioneAsync(posizione);
            return Json(articolo); // Restituisce i risultati come JSON per il frontend
        }


        // GET: Articoli/GetByCodice
        [HttpGet]
        public async Task<IActionResult> GetByCodice(string codice)
        {
            var articolo = await _articoloService.GetByCodiceAsync(codice);
            if (articolo == null)
            {
                return NotFound();
            }
            return Json(articolo);
        }

        // GET: Articoli/GetArticoliCount
        [HttpGet]
        public async Task<IActionResult> GetArticoliCount()
        {
            var count = await _articoloService.GetArticoliCountAsync();
            return Json(count);
        }

        // GET: Articoli/GetArticoliDifettosiCount
        [HttpGet]
        public async Task<IActionResult> GetArticoliDifettosiCount()
        {
            var count = await _articoloService.GetArticoliDifettosiCountAsync();
            return Json(count);
        }

        // GET: Articoli/GetArticoliInEsaurimento
        [HttpGet]
        public async Task<IActionResult> GetArticoliInEsaurimento()
        {
            var articoli = await _articoloService.GetArticoliInEsaurimentoAsync();
            return Json(articoli);
        }

        // GET: Articoli/GetAllArticoli
        [HttpGet]
        public async Task<IActionResult> GetAllArticoli()
        {
            var articoli = await _articoloService.GetAllAsync();
            return Ok(articoli);
        }

        // GET: Articoli/GetPosizioniLibere
        [HttpGet]
        public async Task<IActionResult> GetPosizioniLibere()
        {
            var posizioniLibere = await _articoloService.GetPosizioniLibereAsync();
            return Ok(posizioniLibere);
        }




    }
}
