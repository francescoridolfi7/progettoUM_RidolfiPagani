using Microsoft.AspNetCore.Mvc;
using progettoUMRidolfiPagani.Models;
using progettoUMRidolfiPagani.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using progettoUMRidolfiPagani.Repository;
using Microsoft.EntityFrameworkCore;

namespace progettoUMRidolfiPagani.Controllers
{
    public class AccountController : Controller
    {
        private readonly MagazzinoDbContext _context;

        public AccountController(MagazzinoDbContext context)
        {
            _context = context;
        }

        // Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // Registrazione
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var fieldKey = state.Key;
                    var fieldErrors = state.Value.Errors;

                    Console.WriteLine($"Campo: {fieldKey}");

                    foreach (var error in fieldErrors)
                    {
                        Console.WriteLine($"Errore: {error.ErrorMessage}");
                    }
                }
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var nuovoUtente = new Utente
            {
                Nome = model.Nome,
                Cognome = model.Cognome,
                Email = model.Email,
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password) // Assicurati di utilizzare un hash sicuro
            };

            _context.Utenti.Add(nuovoUtente);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
            
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Cerca l'utente nel database tramite lo username
            var user = await _context.Utenti.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return BadRequest(new { errors = new[] { "Username o password non validi." } });
            }

            // Se il login ha successo, salva l'utente nella sessione
            HttpContext.Session.SetString("Username", user.Username);

            return Ok(new { message = "Login avvenuto con successo!", redirectUrl = Url.Action("Index", "Articoli") });
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Rimuove tutti i dati della sessione
            return RedirectToAction("Login", "Account");
        }


    }
}
