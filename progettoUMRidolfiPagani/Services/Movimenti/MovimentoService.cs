
using progettoUMRidolfiPagani.Models;
using progettoUMRidolfiPagani.Services.Interface;
using progettoUMRidolfiPagani.ViewModels;

namespace progettoUMRidolfiPagani.Services
{
    public class MovimentoService : IMovimentoService
    {
        private readonly MagazzinoDbContext _context;

        public MovimentoService(MagazzinoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movimento>> GetAllAsync()
        {
            return await _context.Movimenti
                .Include(m => m.Articolo)
                .Include(m => m.PosizioneIniziale)
                .Include(m => m.PosizioneFinale)
                .ToListAsync();
        }

        public async Task<Movimento> GetByIdAsync(int id)
        {
            return await _context.Movimenti
                .Include(m => m.Articolo)
                .Include(m => m.PosizioneIniziale)
                .Include(m => m.PosizioneFinale)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movimento> CreateAsync(Movimento movimento)
        {
            _context.Movimenti.Add(movimento);
            await _context.SaveChangesAsync();
            return movimento;
        }

        public async Task<Movimento> UpdateAsync(Movimento movimento)
        {
            _context.Movimenti.Update(movimento);
            await _context.SaveChangesAsync();
            return movimento;
        }

        public async Task DeleteAsync(int id)
        {
            var movimento = await _context.Movimenti.FindAsync(id);
            if (movimento != null)
            {
                _context.Movimenti.Remove(movimento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Movimento> SpostaArticoloAsync(int articoloId, int posizioneInizialeId, int posizioneFinaleId)
        {
            // Trova l'articolo da spostare
            var articolo = await _context.Articoli.Include(a => a.Movimenti).FirstOrDefaultAsync(a => a.Id == articoloId);
            if (articolo == null)
            {
                throw new ArgumentException("Articolo non trovato");
            }

            // Trova la posizione iniziale e finale
            var posizioneIniziale = await _context.Posizioni.FindAsync(posizioneInizialeId);
            var posizioneFinale = await _context.Posizioni.FindAsync(posizioneFinaleId);

            if (posizioneIniziale == null || posizioneFinale == null)
            {
                throw new ArgumentException("Posizione iniziale o finale non trovata");
            }

            // Creare un nuovo movimento per registrare lo spostamento
            var nuovoMovimento = new Movimento
            {
                ArticoloId = articoloId,
                PosizioneInizialeId = posizioneInizialeId,
                PosizioneFinaleId = posizioneFinaleId,
                DataMovimento = DateTime.Now,
                Quantita = articolo.Quantita,  // Usa la quantità associata all'articolo
                TipoMovimento = "Spostamento"
            };

            // Aggiorna la posizione corrente dell'articolo
            articolo.PosizioneId = posizioneFinaleId;

            // Aggiungi il nuovo movimento e salva i cambiamenti nel contesto
            _context.Movimenti.Add(nuovoMovimento);
            await _context.SaveChangesAsync();

            return nuovoMovimento;
        }


        public async Task<IEnumerable<Movimento>> GetMovimentiByArticoloIdAsync(int articoloId)
        {
            return await _context.Movimenti
                .Where(m => m.ArticoloId == articoloId)
                .Include(m => m.PosizioneIniziale)
                .Include(m => m.PosizioneFinale)
                .OrderBy(m => m.DataMovimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movimento>> GetMovimentiRecentiAsync(int days)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);
            return await _context.Movimenti
                .Where(m => m.DataMovimento >= cutoffDate)
                .Include(m => m.Articolo)
                .Include(m => m.PosizioneIniziale)
                .Include(m => m.PosizioneFinale)
                .OrderByDescending(m => m.DataMovimento)
                .ToListAsync();
        }

        public async Task<int> GetMovimentiCountAsync()
        {
            return await _context.Movimenti.CountAsync();
        }

        public async Task<IEnumerable<Movimento>> GetDatiGraficoMovimentiAsync()
        {
            return await _context.Movimenti
                .OrderBy(m => m.DataMovimento)
                .ToListAsync();
        }

        public async Task<double> GetMediaGiorniPermanenzaAsync()
        {
            return await _context.Movimenti.AverageAsync(m => EF.Functions.DateDiffDay(m.DataInizio, m.DataFine));
        }

        public async Task<StatisticheMovimentiViewModel> GetStatisticheMovimentiAsync()
        {
            var totaleMovimenti = await _context.Movimenti.CountAsync();
            var movimentiPerTipo = await _context.Movimenti
                .GroupBy(m => m.TipoMovimento)
                .Select(g => new { TipoMovimento = g.Key, Conteggio = g.Count() })
                .ToListAsync();

            var viewModel = new StatisticheMovimentiViewModel
            {
                NumeroTotaleMovimenti = totaleMovimenti,
                MovimentiPerTipo = movimentiPerTipo.ToDictionary(m => m.TipoMovimento.ToString(), m => m.Conteggio)
            };

            return viewModel;
        }

        public async Task RegistraIngressoAsync(int articoloId, int posizioneId, int quantita)
        {
            var articolo = await _context.Articoli.FindAsync(articoloId);
            if (articolo == null) throw new ArgumentException("Articolo non trovato");

            var posizione = await _context.Posizioni.FindAsync(posizioneId);
            if (posizione == null) throw new ArgumentException("Posizione non trovata");

            var movimento = new Movimento
            {
                ArticoloId = articoloId,
                PosizioneFinaleId = posizioneId,
                Quantita = quantita,
                TipoMovimento = "Ingresso",
                DataMovimento = DateTime.Now
            };

            _context.Movimenti.Add(movimento);
            await _context.SaveChangesAsync();
        }

        public async Task RegistraUscitaAsync(int articoloId, int quantita)
        {
            var articolo = await _context.Articoli.FindAsync(articoloId);
            if (articolo == null) throw new ArgumentException("Articolo non trovato");

            var movimento = new Movimento
            {
                ArticoloId = articoloId,
                PosizioneInizialeId = articolo.PosizioneId,
                Quantita = quantita,
                TipoMovimento = "Uscita",
                DataMovimento = DateTime.Now
            };

            _context.Movimenti.Add(movimento);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Movimento>> GetStoricoMovimentiAsync(int articoloId)
        {
            return await _context.Movimenti
                .Where(m => m.ArticoloId == articoloId)
                .OrderByDescending(m => m.DataMovimento)
                .ToListAsync();
        }


    }
}

