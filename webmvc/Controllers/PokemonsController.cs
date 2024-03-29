﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeApiNet;
using System.Xml.Linq;
using webmvc.Infrastructure;
using webmvc.Models;
using webmvc.ViewModels;

namespace webmvc.Controllers
{
    public class PokemonsController : Controller
    {
        const int PAGESIZE = 10;
        private readonly PokeApiClient _apiClient;
        private readonly MyDbContext _dbContext;
        private readonly ILogger<PokemonsController> _logger;
        private readonly UserManager<User> _userManager;
        public PokemonsController(PokeApiClient apiClient, MyDbContext dbContext, UserManager<User> userManager, ILogger<PokemonsController> logger)
        {
            _apiClient = apiClient;
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int offset = 0)
        {
            Models.Pokemon[] pokemons = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                pokemons = _dbContext.Pokemons.Where(e => e.UserId == user.Id).ToArray();
            }
            else
            {
                pokemons = _dbContext.Pokemons.ToArray();
            }
            var page = await _apiClient.GetNamedResourcePageAsync<PokeApiNet.Pokemon>(PAGESIZE, offset);
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
                FavoritePokemons = pokemons,
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated
            };
            return View("Index", tables);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var pokemons = _dbContext.Pokemons.Where(e => e.UserId == user.Id).ToArray();
            var FavoritePokemons = pokemons;
            return View(FavoritePokemons);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction("", "Pokemons");

            var pokemon = await _apiClient.GetResourceAsync<PokeApiNet.Pokemon>((int)id);
            if (pokemon == null)
            {
                return NotFound();
            }

            return View(pokemon);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Favor(int? id, string? name)
        {
            if (id == null)
                return RedirectToAction("", "Pokemons");

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var pokemonRecord = new webmvc.Models.Pokemon { Id = id.Value, Name= name, UserId = user.Id };
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

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var pokemonRecord = await _dbContext.Pokemons.FirstOrDefaultAsync(c => c.Id == id.Value); 
            if (pokemonRecord == null)
                return NotFound();
            _dbContext.Remove(pokemonRecord);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Favorites");
        }

        [HttpGet]
        public async Task<IActionResult> PokemonPartialView(int id)
        {

            var pokemon = await _apiClient.GetResourceAsync<PokeApiNet.Pokemon>((int)id);
            return PartialView("_DetailsPartial", pokemon);
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
