using webmvc.Models;

namespace webmvc.ViewModels
{
    public class PokemonViewModel
    {
        public PokeApiNet.NamedApiResourceList<PokeApiNet.Pokemon> AvailablePokemons { get; set; }
        public IEnumerable<Pokemon> FavoritePokemons { get; set; }
        public bool IsAuthenticated { get; set; }

    }
}
