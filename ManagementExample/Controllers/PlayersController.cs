using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ManagementExample.Controllers
{
    [Route("[controller]")]
    public class PlayersController : Controller
    {
        private readonly IPlayersService _playersService;
        private readonly ICountriesService _countriesService;
        public PlayersController(IPlayersService playersService, ICountriesService countriesService)
        {
            _playersService = playersService;
            _countriesService = countriesService;
        }
        [HttpGet]
        [Route("[action]")]
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
        [Route("[action]")]
        public IActionResult Add()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Add(PlayerAddRequest playerAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            
            PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{playerID}")]
        public IActionResult Edit(Guid playerID)
        {
            PlayerResponse? player = _playersService.GetPlayerByID(playerID);
            if (player == null)
            {
                return RedirectToAction("Index");
            }
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            PlayerUpdateRequest playerUpdateRequest = player.ToPlayerUpdateRequest();

            return View(playerUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{playerID}")]
        public IActionResult Edit(PlayerUpdateRequest playerUpdateRequest)
        {
            PlayerResponse? playerResponse = _playersService.GetPlayerByID(playerUpdateRequest.PlayerID);
            if (playerResponse == null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                _playersService.UpdatePlayer(playerUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(playerResponse.ToPlayerUpdateRequest());
            }
        }

        [HttpGet]
        [Route("[action]/{playerID}")]
        public IActionResult Delete(Guid playerID)
        {
            PlayerResponse? playerResponse = _playersService.GetPlayerByID(playerID);
            if (playerResponse == null)
            {
                return RedirectToAction("Index");
            }
            return View(playerResponse);
        }

        [HttpPost]
        [Route("[action]/{playerID}")]
        public IActionResult Delete(PlayerUpdateRequest playerUpdateRequest)
        {
            PlayerResponse? playerResponse = _playersService.GetPlayerByID(playerUpdateRequest.PlayerID);
            if (playerResponse == null)
            {
                return RedirectToAction("Index");
            }
            _playersService.DeletePlayer(playerResponse.PlayerID);

            return RedirectToAction("Index");
        }
    }
}
