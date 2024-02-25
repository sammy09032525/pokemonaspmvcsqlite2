using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeApiNet;
using webmvc.Infrastructure;
using webmvc.ViewModels;

namespace webmvc.Controllers
{
    public class PokemonsController : Controller
    {
        const int PAGESIZE = 10;
        private readonly PokeApiClient _apiClient;
        private readonly MyDbContext _dbContext;
        private readonly ILogger<PokemonsController> _logger;
        public PokemonsController(PokeApiClient apiClient, MyDbContext dbContext, ILogger<PokemonsController> logger)
        {
            _apiClient = apiClient;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int offset = 0)
        {
            _dbContext.Pokemons.Load();
            var page = await _apiClient.GetNamedResourcePageAsync<Pokemon>(PAGESIZE, offset);
            page.Previous = !string.IsNullOrEmpty(page.Previous) ? ExtractOffset(page.Previous) : null;
            page.Next = !string.IsNullOrEmpty(page.Next) ? ExtractOffset(page.Next) : null;
            foreach(var r in page.Results)
            {
                string id = ExtractLastParameterFromUrl(r.Url);
                r.Url = id;
            }
            var tables = new PokemonViewModel
            {
                AvailablePokemons = page,
                FavoritePokemons = _dbContext.Pokemons.ToArray(),
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated
            };
            return View("Index", tables);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            await _dbContext.Pokemons.LoadAsync();
            var FavoritePokemons = _dbContext.Pokemons.ToArray();
            return View(FavoritePokemons);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction("", "Pokemons");

            var pokemon = await _apiClient.GetResourceAsync<Pokemon>((int)id);
            if (pokemon == null)
            {
                return NotFound();
            }

            return View(pokemon);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Favor(int? id)
        {
            if (id == null)
                return RedirectToAction("", "Pokemons");

            var pokemon = await _apiClient.GetResourceAsync<Pokemon>((int)id);
            if (pokemon == null)
            {
                return NotFound();
            }
            var pokemonRecord = new webmvc.Models.Pokemon { Id = pokemon.Id, Name= pokemon.Name };
            _dbContext.Add(pokemonRecord);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("", "Pokemons");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UnFavor(int? id)
        {
            if (id == null)
                return RedirectToAction("", "Favorites");

            var pokemonRecord = _dbContext.Pokemons.First(c => c.Id == id.Value); 
//            var pokemonRecord = new webmvc.Models.Pokemon { Id = id.Value};
            _dbContext.Remove(pokemonRecord);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Favorites");
        }

        private static string? ExtractOffset(string url)
        {
            if (int.TryParse(url, out var offset))
                return url;     //this is case when result is from cache

            Uri uri = new(url);
            string queryString = uri.Query;
            string[] queryParams = queryString.TrimStart('?').Split('&');
            foreach (string param in queryParams)
            {
                string[] keyValue = param.Split('=');
                if (keyValue[0] == "offset") 
                    return keyValue[1];
            }
            return null;
        }

        private static string ExtractLastParameterFromUrl(string url)
        {
            // Split the URL by '/'
            string[] segments = url.Trim('/').Split('/');
            // Get the last segment
            string lastSegment = segments[segments.Length - 1];
            return lastSegment;
        }
    }
}
