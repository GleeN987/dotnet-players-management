using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System.ComponentModel.DataAnnotations;
using ServiceContracts.Enums;

namespace Services
{
    public class PlayersService : IPlayersService
    {
        private readonly PlayersDbContext _db;
        private readonly ICountriesService _countriesService;

        public PlayersService(PlayersDbContext dbContext, ICountriesService countriesService)
        {
            _db = dbContext;
            _countriesService = countriesService;
        }

        private PlayerResponse PlayerToPlayerResponse(Player player)
        {
            PlayerResponse playerResponse = player.ToPlayerResponse();
            playerResponse.Country = _countriesService.GetCountryByID(player.CountryID)?.CountryName;
            return playerResponse;
        }

        public PlayerResponse AddPlayer(PlayerAddRequest? playerRequest)
        {
            if (playerRequest == null) throw new ArgumentNullException(nameof(playerRequest));
            ValidationHelper.ModelValidation(playerRequest);
            Player playerToAdd = playerRequest.ToPlayer();
            playerToAdd.PlayerID = Guid.NewGuid();
            _db.sp_AddPlayer(playerToAdd);
            return PlayerToPlayerResponse(playerToAdd);
        }

        public List<PlayerResponse> GetAllPlayers()
        {
            return _db.sp_GetAllPlayers().Select(p => PlayerToPlayerResponse(p)).ToList();
        }

        public PlayerResponse? GetPlayerByID(Guid? playerID)
        {
            if (playerID == null) return null;

            Player? player = _db.Players.FirstOrDefault(p => p.PlayerID == playerID);
            if (player == null) return null;

            return PlayerToPlayerResponse(player);
        }

        public List<PlayerResponse> GetFilteredPlayers(string searchBy, string? searchString)
        {
            List<PlayerResponse> playerList = GetAllPlayers();
            List<PlayerResponse> matchingPlayers = playerList;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) return playerList;

            switch (searchBy)
            {

                case nameof(PlayerResponse.Nickname):
                    matchingPlayers = playerList.Where(p => (!string.IsNullOrEmpty(p.Nickname)
                    ? p.Nickname.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PlayerResponse.Mousepad):
                    matchingPlayers = playerList.Where(p => (!string.IsNullOrEmpty(p.Mousepad) 
                    ? p.Mousepad.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PlayerResponse.Mouse):
                    matchingPlayers = playerList.Where(p => (!string.IsNullOrEmpty(p.Mouse)
                    ? p.Mouse.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PlayerResponse.Team):
                    matchingPlayers = playerList.Where(p => (!string.IsNullOrEmpty(p.Team)
                    ? p.Team.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PlayerResponse.DateOfBirth):
                    matchingPlayers = playerList.Where(p => (p.DateOfBirth != null)
                    ? p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PlayerResponse.Country):
                    matchingPlayers = playerList.Where(p => (!string.IsNullOrEmpty(p.Country)
                    ? p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PlayerResponse.Age):
                    matchingPlayers = playerList.Where(p => (p.Age != null)
                    ? p.Age.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                default: matchingPlayers = playerList; 
                    break;
            }
            return matchingPlayers;
            
        }

        public List<PlayerResponse> GetSortedPlayers(List<PlayerResponse> allPlayers, string sortBy, SortOrder sortOrder)
        {
            if(sortBy == null) return allPlayers;
            List<PlayerResponse> sortedPlayers = (sortBy, sortOrder)
            switch
            {
                (nameof(PlayerResponse.Nickname), SortOrder.ASC)
                => allPlayers.OrderBy(p => p.Nickname, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Nickname), SortOrder.DESC)
                => allPlayers.OrderByDescending(p => p.Nickname, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Mouse), SortOrder.ASC)
                => allPlayers.OrderBy(p => p.Mouse, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Mouse), SortOrder.DESC)
                => allPlayers.OrderByDescending(p => p.Mouse, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Mousepad), SortOrder.ASC)
                => allPlayers.OrderBy(p => p.Mousepad, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Mousepad), SortOrder.DESC)
                => allPlayers.OrderByDescending(p => p.Mousepad, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Team), SortOrder.ASC)
                => allPlayers.OrderBy(p => p.Team, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Team), SortOrder.DESC)
                => allPlayers.OrderByDescending(p => p.Team, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Country), SortOrder.ASC)
                => allPlayers.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.Country), SortOrder.DESC)
                => allPlayers.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PlayerResponse.DateOfBirth), SortOrder.ASC)
                => allPlayers.OrderBy(p => p.DateOfBirth).ToList(),
                (nameof(PlayerResponse.DateOfBirth), SortOrder.DESC)
                => allPlayers.OrderByDescending(p => p.DateOfBirth).ToList(),
                (nameof(PlayerResponse.Age), SortOrder.ASC)
                => allPlayers.OrderBy(p => p.Age).ToList(),
                (nameof(PlayerResponse.Age), SortOrder.DESC)
                => allPlayers.OrderByDescending(p => p.Age).ToList(),


                _ => allPlayers
            };
            return sortedPlayers;
        }

        public PlayerResponse UpdatePlayer(PlayerUpdateRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(Player));
            ValidationHelper.ModelValidation(request);

            Player? playerToUpdate = _db.Players.FirstOrDefault(p => p.PlayerID == request.PlayerID);
            if (playerToUpdate == null) throw new ArgumentException("Player with this ID doesn't exist");
            playerToUpdate.Nickname = request.Nickname;
            playerToUpdate.Team = request.Team;
            playerToUpdate.Mouse = request.Mouse.ToString();
            playerToUpdate.Mousepad = request.Mousepad;
            playerToUpdate.CountryID = request.CountryID;
            playerToUpdate.DateOfBirth = request.DateOfBirth;
            _db.SaveChanges();

            return PlayerToPlayerResponse(playerToUpdate);
        }

        public bool DeletePlayer(Guid? playerID)
        {
            if (playerID == null) throw new ArgumentNullException(nameof(playerID));
            Player? playerToDelete = _db.Players.FirstOrDefault(p => p.PlayerID ==  playerID);
            if (playerToDelete == null) return false;
            _db.Players.Remove(playerToDelete);
            _db.SaveChanges();

            return true;
        }
    }
}
