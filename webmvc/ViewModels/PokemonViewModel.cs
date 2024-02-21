namespace webmvc.ViewModels
{
    public class PokemonViewModel
    {
        public PokeApiNet.NamedApiResourceList<PokeApiNet.Pokemon> AvailablePokemons { get; set; }
        public IEnumerable<PokeApiNet.Pokemon> FavoritePokemons { get; set; }
        public bool IsAuthenticated { get; set; }

    }
}
