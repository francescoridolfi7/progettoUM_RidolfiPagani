
using Microsoft.EntityFrameworkCore;
using progettoUMRidolfiPagani.Models;
using progettoUMRidolfiPagani.Repository;
using progettoUMRidolfiPagani.Services.Interface;

namespace progettoUMRidolfiPagani.Services
{
    public class PosizioneService : IPosizioneService
    {
        private readonly MagazzinoDbContext _context;

        public PosizioneService(MagazzinoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Posizione>> GetAllPosizioniAsync()
        {
            return await _context.Posizioni.ToListAsync();
        }


        public async Task<bool> CheckDisponibilitaPosizioneAsync(int posizioneId)
        {
            var posizione = await _context.Posizioni.Include(p => p.Articoli).FirstOrDefaultAsync(p => p.Id == posizioneId);
            return posizione != null && !posizione.Articoli.Any();
        }

        public async Task<Posizione> CreatePosizioneAsync(Posizione posizione)
        {
            _context.Posizioni.Add(posizione);
            await _context.SaveChangesAsync();
            return posizione;
        }

        public async Task<Posizione> UpdatePosizioneAsync(Posizione posizione)
        {
            _context.Posizioni.Update(posizione);
            await _context.SaveChangesAsync();
            return posizione;
        }

        public async Task DeletePosizioneAsync(int id)
        {
            var posizione = await _context.Posizioni.FindAsync(id);
            if (posizione != null)
            {
                _context.Posizioni.Remove(posizione);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SpostaArticoloAsync(int articoloId, int nuovaPosizioneId)
        {
            var articolo = await _context.Articoli.Include(a => a.Movimenti).FirstOrDefaultAsync(a => a.Id == articoloId);
            if (articolo == null)
            {
                throw new ArgumentException("Articolo non trovato");
            }

            var posizioneFinale = await _context.Posizioni.FindAsync(nuovaPosizioneId);
            if (posizioneFinale == null)
            {
                throw new ArgumentException("Posizione finale non trovata");
            }

            if (await CheckDisponibilitaPosizioneAsync(nuovaPosizioneId))
            {
                //Crea un nuovo movimento per registrare lo spostamento
                var nuovoMovimento = new Movimento
                {
                    Articolo = new Articolo { Id = articoloId },
                    PosizioneIniziale = articolo.Movimenti.OrderByDescending(m => m.DataMovimento)
                                        .FirstOrDefault()?.PosizioneFinale,
                    PosizioneFinale = new Posizione { Id = nuovaPosizioneId },
                    DataMovimento = DateTime.Now,
                    TipoMovimento = TipoMovimento.Spostamento
                };


                _context.Movimenti.Add(nuovoMovimento);

                //Aggiorna la posizione corrente dell'articolo
                articolo.PosizioneId = nuovaPosizioneId;

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Articolo>> GetArticoliInPosizioneAsync(int posizioneId)
        {
            return await _context.Articoli
                .Where(a => a.PosizioneId == posizioneId)
                .Include(a => a.Movimenti)
                .ToListAsync();
        }

        public async Task<int> GetPosizioniDisponibiliCountAsync()
        {
            return await _context.Posizioni.CountAsync(p => p.Occupata == false);
        }

        public async Task<Posizione> GetPosizioneByIdAsync(int id)
        {
            return await _context.Posizioni.FindAsync(id);
        }

        public async Task<Posizione> GetPosizioneByCodiceArticoloAsync(string codiceArticolo)
        {
            return await _context.Posizioni.FirstOrDefaultAsync(p => p.Articoli.Any(a => a.Codice == codiceArticolo));
        }

        public async Task UpdateQuantitaPosizioneAsync(int id, int nuovaQuantita)
        {
            var posizione = await _context.Posizioni.FindAsync(id);
            if (posizione != null)
            {
                posizione.Quantita = nuovaQuantita;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Movimento>> GetStoricoMovimentiPosizioneAsync(int id)
        {
            return await _context.Movimenti
                .Where(m => m.PosizioneIniziale.Id == id || m.PosizioneFinale.Id == id)
                .ToListAsync();
        }


        public async Task<int> GetArticoliInMagazzinoCountAsync()
        {
            return await _context.Articoli.SumAsync(a => a.Quantita);
        }

    }
}


