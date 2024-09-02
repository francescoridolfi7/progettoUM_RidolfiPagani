namespace progettoUMRidolfiPagani.Services 
{
    public class ArticoloService : IArticoloService
    {
        private readonly ApplicationDbContext _context;

        public ArticoloService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Articolo>> GetAllAsync()
        {
            return await _context.Articoli.ToListAsync();
        }

        public async Task<Articolo> GetByIdAsync(int id)
        {
            return await _context.Articoli
                .Include(a => a.Movimenti)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Articolo> CreateAsync(Articolo articolo)
        {
            _context.Articoli.Add(articolo);
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
                .Include(a => a.Movimenti)
                .FirstOrDefaultAsync(a => a.Codice == codice);
        }

        public async Task<IEnumerable<Movimento>> GetMovimentiByArticoloIdAsync(int id)
        {
            var articolo = await _context.Articoli
                .Include(a => a.Movimenti)
                .FirstOrDefaultAsync(a => a.Id == id);

            return articolo?.Movimenti ?? new List<Movimento>();
        }

        public async Task<IEnumerable<Articolo>> SearchAsync(string searchString)
        {
            return await _context.Articoli
                .Where(a => a.Codice.Contains(searchString) || a.Descrizione.Contains(searchString))
                .ToListAsync();
        }

    }
}

