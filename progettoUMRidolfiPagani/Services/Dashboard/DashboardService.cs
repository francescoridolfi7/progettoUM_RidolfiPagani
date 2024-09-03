
using progettoUMRidolfiPagani.Services.Interface;

namespace progettoUMRidolfiPagani.Services {
    public class DashboardService : IDashboardService
    {
        private readonly IArticoloService _articoloService;
        private readonly IMovimentoService _movimentoService;
        private readonly IMagazzinoService _magazzinoService;
        private readonly IStoricoService _storicoService;

        public DashboardService(
            IArticoloService articoloService,
            IMovimentoService movimentoService,
            IMagazzinoService magazzinoService,
            IStoricoService storicoService)
        {
            _articoloService = articoloService;
            _movimentoService = movimentoService;
            _magazzinoService = magazzinoService;
            _storicoService = storicoService;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var articoliTotali = await _articoloService.GetArticoliCountAsync();
            var articoliInMagazzino = await _magazzinoService.GetArticoliInMagazzinoCountAsync();
            var movimentiRecenti = await _storicoService.GetMovimentiRecentiAsync(7);
            var mediaPermanenza = await _storicoService.CalcolaMediaGiorniPermanenzaAsync();
            var articoliDifettosi = await _articoloService.GetArticoliDifettosiCountAsync();

            return new DashboardViewModel
            {
                ArticoliTotali = articoliTotali,
                ArticoliInMagazzino = articoliInMagazzino,
                MovimentiRecenti = movimentiRecenti,
                MediaGiorniPermanenza = mediaPermanenza,
                ArticoliDifettosi = articoliDifettosi
            };
        }
    }
}

