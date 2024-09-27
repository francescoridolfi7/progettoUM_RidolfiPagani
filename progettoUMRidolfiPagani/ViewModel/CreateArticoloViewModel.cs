using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.ViewModels
{
    public class CreateArticoloViewModel
    {
        public Articolo Articolo { get; set; }
        public List<Posizione> PosizioniLibere { get; set; }
    }

}
