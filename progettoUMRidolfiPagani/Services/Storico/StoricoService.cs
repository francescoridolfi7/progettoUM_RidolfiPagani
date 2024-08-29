public class StoricoService : IStoricoService
{
    private readonly ApplicationDbContext _context;

    public StoricoService(ApplicationDbContext context)
    {
        _context = context;
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

    public async Task<IEnumerable<Movimento>> GetMovimentiByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Movimenti
            .Where(m => m.DataMovimento >= startDate && m.DataMovimento <= endDate)
            .Include(m => m.Articolo)
            .Include(m => m.PosizioneIniziale)
            .Include(m => m.PosizioneFinale)
            .OrderBy(m => m.DataMovimento)
            .ToListAsync();
    }

    public async Task<TimeSpan> CalcolaTempoPermanenzaAsync(int articoloId)
    {
        var movimenti = await GetMovimentiByArticoloIdAsync(articoloId);

        if (movimenti == null || !movimenti.Any())
        {
            throw new InvalidOperationException("Non ci sono movimenti per questo articolo.");
        }

        var primoMovimento = movimenti.First().DataMovimento;
        var ultimoMovimento = movimenti.Last().DataMovimento;

        return ultimoMovimento - primoMovimento;
    }

    public async Task<decimal> CalcolaMediaGiorniPermanenzaAsync()
    {
        var articoli = await _context.Articoli.ToListAsync();
        var totalDays = 0;
        var count = 0;

        foreach (var articolo in articoli)
        {
            var tempoPermanenza = await CalcolaTempoPermanenzaAsync(articolo.Id);
            totalDays += (int)tempoPermanenza.TotalDays;
            count++;
        }

        return count > 0 ? (decimal)totalDays / count : 0;
    }

    public async Task<IEnumerable<Articolo>> GetArticoliPiuVecchiAsync(int count)
    {
        return await _context.Articoli
            .Include(a => a.Movimenti)
            .OrderBy(a => a.Movimenti.Min(m => m.DataMovimento))
            .Take(count)
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
