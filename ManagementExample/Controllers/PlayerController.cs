using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ManagementExample.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayersService _playersService;
        private readonly ICountriesService _countriesService;
        public PlayerController(IPlayersService playersService, ICountriesService countriesService)
        {
            _playersService = playersService;
            _countriesService = countriesService;
        }
        [HttpGet]
        [Route("/players/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PlayerResponse.Nickname), SortOrder sortOrder = SortOrder.DESC)
        {
            //Search
            ViewBag.Fields = new Dictionary<string, string>() {
                { nameof(PlayerResponse.Nickname), "Nickname" },
                { nameof(PlayerResponse.Team), "Team" },
                { nameof(PlayerResponse.Mouse), "Mouse" },
                { nameof(PlayerResponse.Mousepad), "Mousepad" },
                { nameof(PlayerResponse.Country), "Country" },
                { nameof(PlayerResponse.DateOfBirth), "Date of birth" },
                { nameof(PlayerResponse.Age), "Age" },
            };

            List<PlayerResponse> players = _playersService.GetFilteredPlayers(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PlayerResponse> playersSorted = _playersService.GetSortedPlayers(players, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(playersSorted);

            
        }
        [HttpGet]
        [Route("players/add")]
        public IActionResult AddPlayer()
        {
            ViewBag.Countries = _countriesService.GetAllCountries();
            ViewBag.MouseEnum = new SelectList(Enum.GetValues(typeof(MouseEnum)).Cast<MouseEnum>());
            return View();
        }

        [HttpPost]
        [Route("players/add")]
        public IActionResult AddPlayer(PlayerAddRequest playerAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _countriesService.GetAllCountries();
                ViewBag.MouseEnum = new SelectList(Enum.GetValues(typeof(MouseEnum)).Cast<MouseEnum>());
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            
            PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);
            return RedirectToAction("Index", "Player");
        }
    }
}
