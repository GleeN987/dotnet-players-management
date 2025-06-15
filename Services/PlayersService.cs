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
        private readonly List<Player> _players;
        private readonly ICountriesService _countriesService;

        public PlayersService(bool initialize = true)
        {
            _players = new List<Player>();
            _countriesService = new CountriesService();
            if (initialize)
            {
                _players.AddRange(new List<Player>() 
                {
                    new Player() { PlayerID=Guid.Parse("01A4C2DD-2E65-4A19-9973-A8F513ED9D4D"), Nickname="Snax", Team="G2", Mouse="Zowie", Mousepad="Logitech G640", DateOfBirth=DateTime.Parse("1995-01-04"), CountryID=Guid.Parse("FA7648A4-6A0B-4B96-BCC7-D7AA1D0D3B9E") },
                    new Player() { PlayerID=Guid.Parse("E4AA3C20-D609-4179-A744-81572EFEA56E"), Nickname="EmiliaQAQ", Team="Lynn Vision", Mouse="Logitech", Mousepad="Qck Heavy", DateOfBirth=DateTime.Parse("2002-03-07"), CountryID=Guid.Parse("A52C23B2-777A-4D43-9E5A-2E1C6D730EBB") },
                    new Player() { PlayerID=Guid.Parse("24C003C1-84A3-4907-BBF7-039508CA689F"), Nickname="dumau", Team="Legacy", Mouse="Zowie", Mousepad="Artisan Zero", DateOfBirth=DateTime.Parse("2004-12-05"), CountryID=Guid.Parse("E517F036-6F16-4A79-89C7-D2FA5D40D289") },
                    new Player() { PlayerID=Guid.Parse("E7EBD193-8574-4A2C-8D2B-C3AE5F41AA02"), Nickname="Zywoo", Team="Vitality", Mouse="Pulsar", Mousepad="The Chosen One", DateOfBirth=DateTime.Parse("2000-11-11"), CountryID=Guid.Parse("A52C23B2-777A-4D43-9E5A-2E1C6D730EBB") },

                 });

          
            }
           
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
            _players.Add(playerToAdd);
            return PlayerToPlayerResponse(playerToAdd);
        }

        public List<PlayerResponse> GetAllPlayers()
        {
            List<PlayerResponse> players = _players.Select(p => PlayerToPlayerResponse(p)).ToList();
            return players;
        }

        public PlayerResponse? GetPlayerByID(Guid? playerID)
        {
            if (playerID == null) return null;

            Player? player = _players.FirstOrDefault(p => p.PlayerID == playerID);
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

            Player? playerToUpdate = _players.FirstOrDefault(p => p.PlayerID == request.PlayerID);
            if (playerToUpdate == null) throw new ArgumentException("Player with this ID doesn't exist");
            playerToUpdate.Nickname = request.Nickname;
            playerToUpdate.Team = request.Team;
            playerToUpdate.Mouse = request.Mouse.ToString();
            playerToUpdate.Mousepad = request.Mousepad;
            playerToUpdate.CountryID = request.CountryID;
            playerToUpdate.DateOfBirth = request.DateOfBirth;

            return PlayerToPlayerResponse(playerToUpdate);
        }

        public bool DeletePlayer(Guid? playerID)
        {
            if (playerID == null) throw new ArgumentNullException(nameof(playerID));
            Player? playerToDelete = _players.FirstOrDefault(p => p.PlayerID ==  playerID);
            if (playerToDelete == null) return false;
            _players.Remove(playerToDelete);
            return true;
        }
    }
}
