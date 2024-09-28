using Microsoft.EntityFrameworkCore;
using progettoUMRidolfiPagani.Models;
using progettoUMRidolfiPagani.Repository;
using progettoUMRidolfiPagani.Services.Interface;

namespace progettoUMRidolfiPagani.Services
{
    public class ArticoloService : IArticoloService
    {
        private readonly MagazzinoDbContext _context;

        public ArticoloService(MagazzinoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Articolo>> GetAllAsync()
        {
           return await  _context.Articoli.Include(a => a.Posizione).ToListAsync();
        }

        public async Task<Articolo> GetByIdAsync(int id)
        {
            return await _context.Articoli
                .Include(a => a.Movimenti)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Articolo> CreateAsync(Articolo articolo)
        {
            // Aggiungi l'articolo al database
            _context.Articoli.Add(articolo);
            await _context.SaveChangesAsync();

            // Crea un nuovo movimento associato all'articolo
            var movimento = new Movimento
            {
                Articolo = articolo,
                ArticoloId = articolo.Id, // Associa l'articolo al movimento
                TipoMovimento = 0, // Movimento in entrata (0)
                PosizioneInizialeId = null, // Nessuna posizione iniziale
                PosizioneFinaleId = articolo.PosizioneId, // La posizione selezionata dall'utente
                DataMovimento = articolo.DataArrivo, // Uguale alla DataArrivo dell'articolo
                Quantita = articolo.Quantita // Quantità selezionata dall'utente
            };

            // Aggiungi il movimento al database
            _context.Movimenti.Add(movimento);
            await _context.SaveChangesAsync();

            return articolo;
        }


        public async Task<Articolo> UpdateAsync(Articolo articolo)
        {
            _context.Articoli.Update(articolo);
            await _context.SaveChangesAsync();
            return articolo;
        }

        public async Task DeleteAsync(int id)
        {
            var articolo = await _context.Articoli.FindAsync(id);
            if (articolo != null)
            {
                _context.Articoli.Remove(articolo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Articolo> GetByCodiceAsync(string codice)
        {
            return await _context.Articoli
                .Include(a => a.Posizione)
                .FirstOrDefaultAsync(a => a.Codice == codice);
        }

        public async Task<IEnumerable<Movimento>> GetMovimentiByArticoloIdAsync(int id)
        {
            var articolo = await _context.Articoli
                .Include(a => a.Movimenti)
                .FirstOrDefaultAsync(a => a.Id == id);

            return articolo?.Movimenti ?? new List<Movimento>();
        }

        public async Task<IEnumerable<Articolo>> GetByPosizioneAsync(string posizione)
        {
            return await _context.Articoli
                .Include(a => a.Posizione) 
                .Where(a => a.Posizione.CodicePosizione.Equals(posizione))
                .ToListAsync();
        }


        public async Task<int> GetArticoliCountAsync()
        {
            return await _context.Articoli.CountAsync();
        }

        public async Task<IEnumerable<Articolo>> GetArticoliInEsaurimentoAsync()
        {
            return await _context.Articoli
                .Include(a => a.Posizione)
                .Where(a => a.Quantita <= 5)
                .ToListAsync();
        }

        public async Task<int> GetArticoliDifettosiCountAsync()
        {
            return await _context.Articoli.CountAsync(a => a.Stato == "Difettoso");
        }

        public async Task<IEnumerable<Posizione>> GetPosizioniLibereAsync()
        {
            return await _context.Posizioni.Where(p => p.Occupata == false).ToListAsync();
        }


    }
}


