
namespace progettoUMRidolfiPagani.Services {
    public class MovimentoService : IMovimentoService
    {
        private readonly ApplicationDbContext _context;

        public MovimentoService(ApplicationDbContext context)
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

        public async Task<Movimento> SpostaArticoloAsync(int articoloId, int posizioneFinaleId)
        {
            var articolo = await _context.Articoli.Include(a => a.Movimenti).FirstOrDefaultAsync(a => a.Id == articoloId);
            if (articolo == null)
            {
                throw new ArgumentException("Articolo non trovato");
            }

            var posizioneIniziale = articolo.Movimenti.OrderByDescending(m => m.DataMovimento).FirstOrDefault()?.PosizioneFinale;
            var posizioneFinale = await _context.Posizioni.FindAsync(posizioneFinaleId);
            if (posizioneFinale == null)
            {
                throw new ArgumentException("Posizione finale non trovata");
            }

            var nuovoMovimento = new Movimento
            {
                ArticoloId = articoloId,
                PosizioneInizialeId = posizioneIniziale?.Id,
                PosizioneFinaleId = posizioneFinaleId,
                DataMovimento = DateTime.Now,
                Quantita = articolo.QuantitaCorrente,  // Assumendo che la quantità venga trasferita interamente
                TipoMovimento = TipoMovimento.Spostamento
            };

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
    }
}
