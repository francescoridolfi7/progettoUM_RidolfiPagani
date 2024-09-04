using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Services;
using progettoUMRidolfiPagani.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using progettoUMRidolfiPagani.Services.Interface;

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
            var articoli = await _articoloService.GetAllAsync();
            return View(articoli);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articoli/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codice,Descrizione,Quantita,Stato")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
                await _articoloService.CreateAsync(articolo);
                return RedirectToAction(nameof(Index));
            }
            return View(articolo);
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
                catch (Exception ex)
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

        // POST: Articoli/Search
        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var results = await _articoloService.SearchAsync(searchTerm);
            return View("Index", results);
        }
    }
}
