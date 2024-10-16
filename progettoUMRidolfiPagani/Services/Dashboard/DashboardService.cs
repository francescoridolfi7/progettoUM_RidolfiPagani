using progettoUMRidolfiPagani.Models;
using progettoUMRidolfiPagani.Services.Interface;
using progettoUMRidolfiPagani.ViewModels;
using progettoUMRidolfiPagani.Repository; // Aggiunto per l'uso di MagazzinoDbContext
using Microsoft.EntityFrameworkCore; // Aggiunto per l'uso di ToListAsync e altre funzioni EF

namespace progettoUMRidolfiPagani.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IArticoloService _articoloService;
        private readonly IMovimentoService _movimentoService;
        private readonly IPosizioneService _posizioneService;
        private readonly MagazzinoDbContext _context; // Aggiunta del contesto DbContext

        public DashboardService(
            IArticoloService articoloService,
            IMovimentoService movimentoService,
            IPosizioneService posizioneService,
            MagazzinoDbContext context)
        {
            _articoloService = articoloService;
            _movimentoService = movimentoService;
            _posizioneService = posizioneService;
            _context = context; // Inizializzazione del contesto
        }

        public async Task<int> GetNumeroTotaleArticoliAsync()
        {
            return await _articoloService.GetArticoliCountAsync();
        }

        public async Task<int> GetNumeroTotaleMovimentiAsync()
        {
            return await _movimentoService.GetMovimentiCountAsync();
        }

        public async Task<int> GetNumeroPosizioniDisponibiliAsync()
        {
            return await _posizioneService.GetPosizioniDisponibiliCountAsync();
        }

        public async Task<IEnumerable<Articolo>> GetArticoliInEsaurimentoAsync()
        {
            return await _articoloService.GetArticoliInEsaurimentoAsync();
        }

        public async Task<IEnumerable<Movimento>> GetDatiGraficoMovimentiAsync()
        {
            return await _movimentoService.GetDatiGraficoMovimentiAsync();
        }

        public async Task<double> CalcolaMediaGiorniPermanenzaAsync()
        {
            var ingressi = await _context.Movimenti
                .Where(m => m.TipoMovimento == TipoMovimento.Ingresso)
                .ToListAsync();

            var uscite = await _context.Movimenti
                .Where(m => m.TipoMovimento == TipoMovimento.Uscita)
                .ToListAsync();

            if (!ingressi.Any()) return 0;

            double sommaGiorni = 0;
            int conteggioPermanenza = 0;

            foreach (var ingresso in ingressi)
            {
                // Trova la prima uscita per lo stesso articolo dopo l'ingresso
                var uscitaCorrispondente = uscite
                    .FirstOrDefault(u => u.ArticoloId == ingresso.ArticoloId && u.DataMovimento > ingresso.DataMovimento);

                double giorniDiPermanenza;

                if (uscitaCorrispondente != null)
                {
                    // Calcola i giorni di permanenza tra l'ingresso e l'uscita
                    giorniDiPermanenza = (uscitaCorrispondente.DataMovimento - ingresso.DataMovimento).TotalDays;
                }
                else
                {
                    // Se non c'è uscita, calcola i giorni di permanenza fino a oggi
                    giorniDiPermanenza = (DateTime.Now - ingresso.DataMovimento).TotalDays;
                }

                // Aggiungi alla somma totale dei giorni e incrementa il conteggio
                sommaGiorni += giorniDiPermanenza;
                conteggioPermanenza++;
            }

            // Calcola la media dei giorni di permanenza
            return conteggioPermanenza > 0 ? sommaGiorni / conteggioPermanenza : 0;
        }



        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var articoliTotali = await _articoloService.GetArticoliCountAsync();
            var articoliInMagazzino = await _posizioneService.GetArticoliInMagazzinoCountAsync();
            var articoliDifettosi = await _articoloService.GetArticoliDifettosiCountAsync();
            var numeroTotaleMovimenti = await _movimentoService.GetMovimentiCountAsync();
            var numeroPosizioniDisponibili = await _posizioneService.GetPosizioniDisponibiliCountAsync();
            var movimentiPerTipo = await _movimentoService.GetMovimentiPerTipoAsync();
            var mediaPermanenza = await CalcolaMediaGiorniPermanenzaAsync();

            return new DashboardViewModel
            {
                NumeroTotaleArticoli = articoliTotali,
                ArticoliInMagazzino = articoliInMagazzino,
                ArticoliDifettosi = articoliDifettosi,
                NumeroTotaleMovimenti = numeroTotaleMovimenti,
                NumeroPosizioniDisponibili = numeroPosizioniDisponibili,
                MovimentiPerTipo = movimentiPerTipo,
                MediaGiorniPermanenza = mediaPermanenza,
            };
        }
    }
}
