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

            // Aggiorna la posizione associata all'articolo
            if (articolo.PosizioneId.HasValue)
            {
                var posizione = await _context.Posizioni.FindAsync(articolo.PosizioneId.Value);
                if (posizione != null)
                {
                    posizione.Occupata = true; // Imposta la posizione come occupata
                    posizione.Quantita = articolo.Quantita;
                    _context.Posizioni.Update(posizione);
                    await _context.SaveChangesAsync(); // Salva le modifiche nel database
                }
            }

            return articolo;
        }


        public async Task<Articolo> UpdateAsync(Articolo articolo, int nuovaQuantita, int? nuovaPosizioneId, int quantitaOriginale, int? posizioneIdCorrente)
        {
            var articoloDb = await _context.Articoli
                .Include(a => a.Posizione)
                .FirstOrDefaultAsync(a => a.Id == articolo.Id);

            if (articoloDb == null)
                throw new Exception("Articolo non trovato");

            // Verifica se viene spostata tutta la quantità o solo una parte
            if (nuovaQuantita < quantitaOriginale)
            {
                articoloDb.Quantita -= nuovaQuantita;

                // Aggiorna la quantità nella vecchia posizione
                var vecchiaPosizione = await _context.Posizioni.FirstOrDefaultAsync(p => p.Id == posizioneIdCorrente);
                if (vecchiaPosizione != null)
                {
                    vecchiaPosizione.Quantita -= nuovaQuantita;  // Sottrae la quantità spostata
                    _context.Posizioni.Update(vecchiaPosizione);
                }

                // Crea un nuovo articolo per la quantità spostata
                var nuovoArticolo = new Articolo
                {
                    Codice = articoloDb.Codice,
                    Descrizione = articoloDb.Descrizione,
                    Stato = articoloDb.Stato,
                    Quantita = nuovaQuantita,
                    PosizioneId = nuovaPosizioneId
                };

                _context.Articoli.Add(nuovoArticolo);
            }
            else
            {
                articoloDb.PosizioneId = nuovaPosizioneId;
            }

            _context.Articoli.Update(articoloDb);

            // Aggiorna la nuova posizione
            var nuovaPosizione = await _context.Posizioni.FirstOrDefaultAsync(p => p.Id == nuovaPosizioneId);
            if (nuovaPosizione != null)
            {
                nuovaPosizione.Quantita += nuovaQuantita;
                nuovaPosizione.Occupata = true;
                _context.Posizioni.Update(nuovaPosizione);
            }

            // Se viene spostata l'intera quantità, libera la vecchia posizione
            if (nuovaQuantita == quantitaOriginale)
            {
                var vecchiaPosizione = await _context.Posizioni.FirstOrDefaultAsync(p => p.Id == posizioneIdCorrente);
                if (vecchiaPosizione != null)
                {
                    vecchiaPosizione.Quantita = 0;
                    vecchiaPosizione.Occupata = false;
                    _context.Posizioni.Update(vecchiaPosizione);
                }
            }

            // Crea un nuovo record nella tabella Movimenti
            var movimento = new Movimento
            {
                Articolo = articolo,
                ArticoloId = articoloDb.Id,  // L'articolo che è stato spostato
                TipoMovimento = (TipoMovimento)1, // Spostamento (1)
                PosizioneInizialeId = posizioneIdCorrente,  // Posizione originale
                PosizioneFinaleId = nuovaPosizioneId,  // Posizione finale
                Quantita = nuovaQuantita,  // Quantità spostata
                DataMovimento = DateTime.Now  // Imposta la data del movimento
            };

            _context.Movimenti.Add(movimento);

            await _context.SaveChangesAsync();  // Salva tutte le modifiche nel database
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
        public async Task<Posizione> GetPosizioneByIdAsync(int id)
        {
            return await _context.Posizioni.FindAsync(id);
        }



    }
}


