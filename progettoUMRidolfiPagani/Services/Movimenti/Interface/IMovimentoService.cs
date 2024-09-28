
using progettoUMRidolfiPagani.Models;
using progettoUMRidolfiPagani.ViewModels;

namespace progettoUMRidolfiPagani.Services.Interface
{
    public interface IMovimentoService
    {
        Task<IEnumerable<Movimento>> GetAllAsync();
        Task<Movimento> GetByIdAsync(int id);
        Task<Movimento> CreateAsync(Movimento movimento);
        Task<Movimento> UpdateAsync(Movimento movimento);
        Task DeleteAsync(int id);
        Task<IEnumerable<Movimento>> GetMovimentiByArticoloIdAsync(int articoloId);
        Task<IEnumerable<Movimento>> GetMovimentiRecentiAsync(int days);
        Task<int> GetMovimentiCountAsync();
        Task<IEnumerable<Movimento>> GetDatiGraficoMovimentiAsync();
        //Task<double> GetMediaGiorniPermanenzaAsync();
        Task<StatisticheMovimentiViewModel> GetStatisticheMovimentiAsync();
        Task RegistraIngressoAsync(int articoloId, int posizione, int quantita);
        Task RegistraUscitaAsync(int articoloId, int quantita, int posizione);
        Task<Movimento> SpostaArticoloAsync(int articoloId, int posizioneInizialeId, int posizioneFinaleId);
        Task<IEnumerable<Movimento>> GetStoricoMovimentiAsync(int articoloId);



    }
}


