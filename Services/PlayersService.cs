using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class PlayersService : IPlayersService
    {
        private readonly List<Player> _players;
        private readonly ICountriesService _countriesService;

        public PlayersService()
        {
            _players = new List<Player>();
            _countriesService = new CountriesService();
        }

        private PlayerResponse PlayerToPlayerResponse(Player player)
        {
            PlayerResponse playerResponse = player.ToPlayerResponse();
            CountryResponse? countryResponse = _countriesService.GetCountryByID(player.CountryID);
            if (countryResponse != null)
            {
                playerResponse.Country = countryResponse.CountryName;
            }
            return playerResponse;
        }

        public PlayerResponse AddPlayer(PlayerAddRequest? playerRequest)
        {
            if (playerRequest == null) throw new ArgumentNullException(nameof(playerRequest));
            ValidationHelper.ModelValidation(playerRequest);
            Player playerToAdd = playerRequest.ToPlayer();
            playerToAdd.PlayerID = Guid.NewGuid();
            _players.Add(playerToAdd);
            return PlayerToPlayerResponse(playerToAdd);
        }

        public List<PlayerResponse> GetAllPlayers()
        {
            return _players.Select(p => p.ToPlayerResponse()).ToList();
        }
    }
}
