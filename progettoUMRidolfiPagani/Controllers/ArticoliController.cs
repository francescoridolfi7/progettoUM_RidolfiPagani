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

        //GET: Articoli
        public async Task<IActionResult> Index()
        {          
            var totalArticoli = await _articoloService.GetArticoliCountAsync();
          
            var articoliDifettosi = await _articoloService.GetArticoliDifettosiCountAsync();
           
            var articoliInEsaurimento = await _articoloService.GetArticoliInEsaurimentoAsync();
           
            var articoli = await _articoloService.GetAllAsync();

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

            var movimenti = await _articoloService.GetMovimentiByArticoloIdAsync(id);

            var detailsViewModel = new DetailsArticoloViewModel
            {
                Id = articolo.Id,
                Codice = articolo.Codice,
                Descrizione = articolo.Descrizione,
                Quantita = articolo.Quantita,
                Stato = articolo.Stato,
                Movimenti = movimenti.Select(m => new MovimentoViewModel
                {
                    DataMovimento = m.DataMovimento,
                    TipoMovimento = m.TipoMovimento == TipoMovimento.Ingresso ? "Entrata" :
                    m.TipoMovimento == TipoMovimento.Spostamento ? "Spostamento" : "Uscita",
                    Quantita = m.Quantita,
                    PosizioneIniziale = m.PosizioneIniziale?.CodicePosizione,
                    PosizioneFinale = m.PosizioneFinale?.CodicePosizione
                }).ToList()
            };

            return View(detailsViewModel);
        }


        // GET: Articoli/Create
        public async Task<IActionResult> Create()
        {
            var posizioniLibere = await _articoloService.GetPosizioniLibereAsync();

            var createViewModel = new CreateArticoloViewModel
            {
                PosizioniLibere = posizioniLibere.ToList() 
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

            //Verifica che l'articolo abbia una posizione associata
            string codicePosizioneCorrente = articolo.Posizione?.CodicePosizione ?? "Nessuna posizione";

            //Ottieni le posizioni libere
            var posizioniLibere = await _articoloService.GetPosizioniLibereAsync();

            //Crea il ViewModel per la vista di Edit
            var editViewModel = new EditArticoloViewModel
            {
                Id = articolo.Id,
                Codice = articolo.Codice,
                Descrizione = articolo.Descrizione,
                Quantita = articolo.Quantita,
                Stato = articolo.Stato,
                PosizioneIdCorrente = articolo.PosizioneId, //Posizione corrente
                PosizioniLibere = posizioniLibere.ToList()
            };

            return View(editViewModel);
        }



        // POST: Articoli/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] EditArticoloViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            ModelState.Remove("PosizioniLibere");
            ModelState.Remove("Codice");
            ModelState.Remove("Descrizione");
            ModelState.Remove("Stato");
            ModelState.Remove("Quantita");

            Console.WriteLine($"Quantità ricevuta: {model.Quantita}");
            Console.WriteLine($"PosizioneId ricevuta: {model.PosizioneId}");
            Console.WriteLine($"Posizione originale ricevuta: {model.PosizioneIdCorrente}");

            if (ModelState.IsValid)
            {
                try
                {
                    var articolo = await _articoloService.GetByIdAsync(id);
                    if (articolo == null)
                    {
                        return NotFound();
                    }

                    await _articoloService.UpdateAsync(
                        articolo,
                        model.PosizioneId,
                        model.PosizioneIdCorrente
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errore: " + ex.Message);
                    ModelState.AddModelError("", "Non è stato possibile aggiornare l'articolo.");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            model.PosizioniLibere = (List<Posizione>)await _articoloService.GetPosizioniLibereAsync();
            return View(model);
        }





        // GET: Articoli/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var articolo = await _articoloService.GetByIdAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }

            var deleteViewModel = new DeleteArticoloViewModel
            {
                Id = articolo.Id,
                Codice = articolo.Codice,
                Descrizione = articolo.Descrizione,
                Quantita = articolo.Quantita,
                Stato = articolo.Stato,
                PosizioneIdCorrente = articolo.PosizioneId,
                CodicePosizioneCorrente = articolo.Posizione?.CodicePosizione ?? "Nessuna posizione"
            };

            return View(deleteViewModel);
        }

        // POST: Articoli/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteArticoloViewModel model)
        {
            try
            {
                await _articoloService.DeleteAsync(id, model.Quantita);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Errore nell'uscita dell'articolo: {ex.Message}");
                return View(model);
            }
        }

        // GET: Articoli/Search
        public async Task<IActionResult> Research()
        {
            var totalArticoli = await _articoloService.GetArticoliCountAsync();

            var articoliDifettosi = await _articoloService.GetArticoliDifettosiCountAsync();

            var articoliInEsaurimento = await _articoloService.GetArticoliInEsaurimentoAsync();

            var articoli = await _articoloService.GetAllAsync();

            var viewModel = new ArticoliViewModel
            {
                TotalArticoli = totalArticoli,
                ArticoliDifettosi = articoliDifettosi,
                ArticoliInEsaurimento = articoliInEsaurimento,
                Articoli = articoli.ToList()
            };

            return View(viewModel);
        }

        // GET: Articoli/GetByPosizione
        [HttpGet] 
        public async Task<IActionResult> GetByPosizione(string posizione)
        {
            var articolo = await _articoloService.GetByPosizioneAsync(posizione);
            return Json(articolo); 
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
        // GET: Articoli/GetArticoloById/5
        [HttpGet]
        public async Task<IActionResult> GetArticoloById(int id)
        {
            var articolo = await _articoloService.GetByIdAsync(id);

            if (articolo == null)
            {
                return NotFound();
            }

            var movimenti = await _articoloService.GetMovimentiByArticoloIdAsync(id);

            var response = new
            {
                id = articolo.Id,
                codice = articolo.Codice,
                descrizione = articolo.Descrizione,
                quantita = articolo.Quantita,
                stato = articolo.Stato,
                posizioneId = articolo.PosizioneId,
                codicePosizioneCorrente = articolo.Posizione?.CodicePosizione,
                movimenti = movimenti.Select(m => new
                {
                    id = m.Id,
                    dataMovimento = m.DataMovimento,
                    tipoMovimento = m.TipoMovimento.ToString(),
                    quantita = m.Quantita,
                    posizioneIniziale = m.PosizioneIniziale?.CodicePosizione,
                    posizioneFinale = m.PosizioneFinale?.CodicePosizione
                }).ToList()
            };

            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetArticoloPiuVecchio()
        {
            var articoloPiuVecchio = await _articoloService.GetArticoloPiuVecchioAsync();
            if (articoloPiuVecchio == null)
            {
                return NotFound();
            }

            var response = new
            {
                id = articoloPiuVecchio.Id,
                codice = articoloPiuVecchio.Codice,
                descrizione = articoloPiuVecchio.Descrizione,
                quantita = articoloPiuVecchio.Quantita,
                codicePosizione = articoloPiuVecchio.Posizione?.CodicePosizione ?? "Articolo consegnato direttamente al reparto"
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Ripara(int id)
        {
            try
            {
                await _articoloService.RiparaArticoloAsync(id); 
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }




    }
}
