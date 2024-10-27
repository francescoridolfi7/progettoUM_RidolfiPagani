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
                .Include(a => a.Posizione)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Articolo> CreateAsync(Articolo articolo)
        {
            _context.Articoli.Add(articolo);
            await _context.SaveChangesAsync();

            //Crea un nuovo movimento associato all'articolo
            var movimento = new Movimento
            {
                Articolo = articolo,
                ArticoloId = articolo.Id, 
                TipoMovimento = 0, 
                PosizioneInizialeId = null, 
                PosizioneFinaleId = articolo.PosizioneId, 
                DataMovimento = articolo.DataArrivo, 
                Quantita = articolo.Quantita 
            };

            _context.Movimenti.Add(movimento);
            await _context.SaveChangesAsync();

            if (articolo.PosizioneId.HasValue)
            {
                var posizione = await _context.Posizioni.FindAsync(articolo.PosizioneId.Value);
                if (posizione != null)
                {
                    posizione.Occupata = true; //Imposta la posizione come occupata
                    posizione.Quantita = articolo.Quantita;
                    _context.Posizioni.Update(posizione);
                    await _context.SaveChangesAsync();
                }
            }

            return articolo;
        }


        public async Task<Articolo> UpdateAsync(Articolo articolo,int? nuovaPosizioneId,int? posizioneIdCorrente)
        {
            var articoloDb = await _context.Articoli
                .Include(a => a.Posizione)
                .FirstOrDefaultAsync(a => a.Id == articolo.Id);

            if (articoloDb == null)
                throw new Exception("Articolo non trovato");

            articoloDb.PosizioneId = nuovaPosizioneId;
            

            _context.Articoli.Update(articoloDb);

            //Aggiorna la nuova posizione
            var nuovaPosizione = await _context.Posizioni.FirstOrDefaultAsync(p => p.Id == nuovaPosizioneId);
            if (nuovaPosizione != null)
            {
                nuovaPosizione.Quantita += articolo.Quantita;
                nuovaPosizione.Occupata = true;
                _context.Posizioni.Update(nuovaPosizione);
            }

                var vecchiaPosizione = await _context.Posizioni.FirstOrDefaultAsync(p => p.Id == posizioneIdCorrente);
                if (vecchiaPosizione != null)
                {
                    vecchiaPosizione.Quantita = 0;
                    vecchiaPosizione.Occupata = false;
                    _context.Posizioni.Update(vecchiaPosizione);
                }

            var movimento = new Movimento
            {
                Articolo = articolo,
                ArticoloId = articoloDb.Id,  
                TipoMovimento = (TipoMovimento)1, 
                PosizioneInizialeId = posizioneIdCorrente,  
                PosizioneFinaleId = nuovaPosizioneId, 
                Quantita = articoloDb.Quantita,  
                DataMovimento = DateTime.Now  
            };

            _context.Movimenti.Add(movimento);

            await _context.SaveChangesAsync();  
            return articolo;
        }

        public async Task DeleteAsync(int articoloId, int quantitaDaUscire)
        {
            var articolo = await _context.Articoli
                .Include(a => a.Posizione)  
                .FirstOrDefaultAsync(a => a.Id == articoloId);

            if (articolo == null)
                throw new Exception("Articolo non trovato");

            var posizione = articolo.Posizione;

            
            var movimento = new Movimento
            {
                Articolo = articolo,
                ArticoloId = articolo.Id,
                TipoMovimento = (TipoMovimento)2,  //Uscita
                PosizioneInizialeId = posizione?.Id,
                PosizioneFinaleId = null,
                Quantita = quantitaDaUscire,
                DataMovimento = DateTime.Now
            };

            _context.Movimenti.Add(movimento);
            await _context.SaveChangesAsync();

            if (quantitaDaUscire >= articolo.Quantita)
            {
                if (posizione != null)
                {
                    posizione.Quantita = 0;
                    posizione.Occupata = false;
                    _context.Posizioni.Update(posizione);
                }

                _context.Articoli.Remove(articolo);
            }
            else
            {
                articolo.Quantita -= quantitaDaUscire;
                _context.Articoli.Update(articolo);

                if (posizione != null)
                {
                    posizione.Quantita -= quantitaDaUscire;
                    _context.Posizioni.Update(posizione);
                }
            }

            await _context.SaveChangesAsync();  
        }

        public async Task<Articolo> GetByCodiceAsync(string codice)
        {
            return await _context.Articoli
                .Include(a => a.Posizione)
                .FirstOrDefaultAsync(a => a.Codice == codice);
        }

        public async Task<List<Movimento>> GetMovimentiByArticoloIdAsync(int articoloId)
        {
            return await _context.Movimenti
                .Include(m => m.PosizioneIniziale)
                .Include(m => m.PosizioneFinale)
                .Where(m => m.ArticoloId == articoloId)
                .ToListAsync();
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
        public async Task<Articolo> GetArticoloPiuVecchioAsync()
        {
            return await _context.Articoli
                .Include(a => a.Posizione)
                .OrderBy(a => a.DataArrivo)  
                .FirstOrDefaultAsync();  
        }

        public async Task RiparaArticoloAsync(int id)
        {
            var articolo = await _context.Articoli.FirstOrDefaultAsync(a => a.Id == id);
            if (articolo == null)
            {
                throw new Exception("Articolo non trovato");
            }

            //Imposta lo stato dell'articolo a "In Magazzino"
            articolo.Stato = "In Magazzino";

            _context.Articoli.Update(articolo);
            await _context.SaveChangesAsync();
        }





    }
}


