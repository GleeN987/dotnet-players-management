using System;
using System.Collections.Generic;
using Xunit;
using Entities;
using ServiceContracts;
using Services;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Xunit.Abstractions;

namespace xUnitTests
{
    public class PlayerServiceTests
    {
        private readonly IPlayersService _playersService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _outputHelper;
        public PlayerServiceTests(ITestOutputHelper outputHelper)
        {
            _playersService = new PlayersService(false);
            _countriesService = new CountriesService(false);
            _outputHelper = outputHelper;
        }

        #region AddPlayer
        [Fact]
        public void AddPlayer_NullPlayer()
        {
            //Arrange
            PlayerAddRequest? player = null;
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _playersService.AddPlayer(player);
            });
        }
        [Fact]
        public void AddPlayer_NullPlayerNickname()
        {
            //Arrange
            PlayerAddRequest? player = new PlayerAddRequest() { Nickname = null };
            //Act
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _playersService.AddPlayer(player);
            });
        }
        [Fact]
        public void AddPlayer_ProperRequest()
        {
            //Arrange
            PlayerAddRequest? playerAddRequest = new PlayerAddRequest()
            {
                Nickname = "abc",
                Team = "xyz",
                Mouse = MouseEnum.Pulsar,
                Mousepad = "essa",
                CountryID = Guid.NewGuid(),
                DateOfBirth = DateTime.Parse("2000-11-11")
            };

            //Act
            PlayerResponse playerResponseFromAdd = _playersService.AddPlayer(playerAddRequest);
            List<PlayerResponse> playerList = _playersService.GetAllPlayers();

            //Assert
            Assert.True(playerResponseFromAdd.PlayerID != Guid.Empty);
            Assert.Contains(playerResponseFromAdd, playerList);


        }
        #endregion

        #region GetAllPlayers
        [Fact]
        public void GetAllPlayers_EmptyList()
        {
            List<PlayerResponse> playerList = _playersService.GetAllPlayers();
            Assert.Empty(playerList);
        }
        [Fact]
        public void GetAllPlayers_AddFewPlayers()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Poland" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            List<PlayerAddRequest> playerRequestsList = new List<PlayerAddRequest>()
            {
                new PlayerAddRequest() {
                    Nickname = "xyz",
                    Team = "A",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sda",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2000-11-11")
                },
                new PlayerAddRequest() {
                    Nickname = "xcz",
                    Team = "Aas",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sa",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2001-11-11")
                }
            };

            List<PlayerResponse> playersListActual = new List<PlayerResponse>();
            _outputHelper.WriteLine("Expected:");
            foreach (PlayerAddRequest playerAddRequest in playerRequestsList)
            {
                PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);
                playersListActual.Add(playerResponse);
                _outputHelper.WriteLine(playerResponse.ToString());
            }

            _outputHelper.WriteLine("Actual:");
            List<PlayerResponse> playerListFromGetAll = _playersService.GetAllPlayers();
            foreach (PlayerResponse playerFromGetAll in playerListFromGetAll)
            {
                _outputHelper.WriteLine(playerFromGetAll.ToString());
            }
            foreach (PlayerResponse playerResponse in playersListActual)
            {
                Assert.Contains(playerResponse, playerListFromGetAll);
            }

        }
        #endregion

        #region GetPlayerByID
        [Fact]
        public void GetPlayerByID_NullID()
        {
            Guid nullID = Guid.Empty;
            PlayerResponse? player = _playersService.GetPlayerByID(nullID);

            Assert.Null(player);
        }
        [Fact]
        public void GetPlayerByID_ValidID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "China"};
            CountryResponse country = _countriesService.AddCountry(countryAddRequest); 
            PlayerAddRequest playerAddRequest = new PlayerAddRequest() { Nickname = "Zywoo", CountryID = country.CountryID };
            PlayerResponse addedPlayer = _playersService.AddPlayer(playerAddRequest);
            PlayerResponse? playerFromGetByID = _playersService.GetPlayerByID(addedPlayer.PlayerID);

            Assert.Equal(addedPlayer, playerFromGetByID);
        }
        #endregion

        #region GetFilteredPlayers
        [Fact]
        //providing empty search string should return all players
        public void GetFilteredPlayers_EmptySearchString()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Poland" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            List<PlayerAddRequest> playerRequestsList = new List<PlayerAddRequest>()
            {
                new PlayerAddRequest() {
                    Nickname = "xyz",
                    Team = "A",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sda",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2000-11-11")
                },
                new PlayerAddRequest() {
                    Nickname = "xcz",
                    Team = "Aas",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sa",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2001-11-11")
                }
            };

            List<PlayerResponse> playersListActual = new List<PlayerResponse>();
            _outputHelper.WriteLine("Expected:");
            foreach (PlayerAddRequest playerAddRequest in playerRequestsList)
            {
                PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);
                playersListActual.Add(playerResponse);
                _outputHelper.WriteLine(playerResponse.ToString());
            }

            _outputHelper.WriteLine("Actual:");
            List<PlayerResponse> playerListFiltered = _playersService.GetFilteredPlayers(nameof(Player.Nickname), "");
            foreach (PlayerResponse playerFromGetAll in playerListFiltered)
            {
                _outputHelper.WriteLine(playerFromGetAll.ToString());
            }
            foreach (PlayerResponse playerResponse in playersListActual)
            {
                Assert.Contains(playerResponse, playerListFiltered);
            }

        }

        [Fact]
        //providing empty search string should return all players
        public void GetFilteredPlayers_SearchByNickname()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Poland" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            List<PlayerAddRequest> playerRequestsList = new List<PlayerAddRequest>()
            {
                new PlayerAddRequest() {
                    Nickname = "Zywoo",
                    Team = "A",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sda",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2000-11-11")
                },
                new PlayerAddRequest() {
                    Nickname = "phzy",
                    Team = "Aas",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sa",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2001-11-11")
                }
            };

            List<PlayerResponse> playersListActual = new List<PlayerResponse>();
            _outputHelper.WriteLine("Expected:");
            foreach (PlayerAddRequest playerAddRequest in playerRequestsList)
            {
                PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);
                playersListActual.Add(playerResponse);
                _outputHelper.WriteLine(playerResponse.ToString());
            }

            _outputHelper.WriteLine("Actual:");
            List<PlayerResponse> playerListFiltered = _playersService.GetFilteredPlayers(nameof(Player.Nickname), "zy");
            foreach (PlayerResponse playerFiltered in playerListFiltered)
            {
                _outputHelper.WriteLine(playerFiltered.ToString());
            }
            foreach (PlayerResponse playerResponse in playersListActual)
            {
                if (playerResponse.Nickname != null) 
                { 
                    if(playerResponse.Nickname.Contains("zy", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(playerResponse, playerListFiltered);
                    }
                }
            }

        }
        #endregion

        #region GetSortedPlayers
        [Fact]
        public void GetSortedPlayers_SortByNicknameDescending()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Poland" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            List<PlayerAddRequest> playerRequestsList = new List<PlayerAddRequest>()
            {
                new PlayerAddRequest() {
                    Nickname = "Zywoo",
                    Team = "A",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sda",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2000-11-11")
                },
                new PlayerAddRequest() {
                    Nickname = "aleksib",
                    Team = "Aas",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sa",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2001-11-11")
                },
                new PlayerAddRequest() {
                    Nickname = "phzy",
                    Team = "dssd",
                    Mouse = MouseEnum.Pulsar,
                    Mousepad = "s2a",
                    CountryID = countryResponse.CountryID,
                    DateOfBirth = DateTime.Parse("2001-06-11")
                }
            };

            List<PlayerResponse> playersListExpected = new List<PlayerResponse>();
            foreach (PlayerAddRequest playerAddRequest in playerRequestsList)
            {
                PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);
                playersListExpected.Add(playerResponse);
            }
            List<PlayerResponse> playersSortedExpected = playersListExpected.OrderByDescending(p => p.Nickname).ToList();
            _outputHelper.WriteLine("Expected:");
            foreach (PlayerResponse playerListExpected in playersSortedExpected)
            {
                _outputHelper.WriteLine(playerListExpected.ToString());
            }

            List<PlayerResponse> allPlayers = _playersService.GetAllPlayers();
            _outputHelper.WriteLine("Actual:");
            List<PlayerResponse> playersSortedActual = _playersService.GetSortedPlayers(allPlayers, nameof(Player.Nickname), SortOrder.DESC);
            foreach (PlayerResponse playerSortedActual in playersSortedActual)
            {
                _outputHelper.WriteLine(playerSortedActual.ToString());
            }

            for (int i = 0; i < playersSortedExpected.Count; i++)
            {
                Assert.Equal(playersSortedExpected[i], playersSortedActual[i]);
            }

        }
        #endregion

        #region UpdatePlayer
        [Fact]
        public void UpdatePlayer_NullPlayer()
        {
            PlayerUpdateRequest? playerUpdateRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _playersService.UpdatePlayer(playerUpdateRequest);
            });
            
        }

        [Fact]
        public void UpdatePlayer_InvalidPlayerID()
        {
            PlayerUpdateRequest? playerUpdateRequest = new PlayerUpdateRequest() { PlayerID = Guid.NewGuid() };

            Assert.Throws<ArgumentException>(() =>
            {
                _playersService.UpdatePlayer(playerUpdateRequest);
            });

        }

        [Fact]
        public void UpdatePlayer_PlayerNicknameIsNull()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Poland" };
            CountryResponse country = _countriesService.AddCountry(countryAddRequest);
            PlayerAddRequest playerAddRequest = new PlayerAddRequest() { Nickname = "ropz", CountryID = country.CountryID };
            PlayerResponse player = _playersService.AddPlayer(playerAddRequest);
            PlayerUpdateRequest playerUpdateRequest = player.ToPlayerUpdateRequest();
            playerUpdateRequest.Nickname = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _playersService.UpdatePlayer(playerUpdateRequest);
            });

        }

        [Fact]
        public void UpdatePlayer_PlayerCorrectlyUpdated()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Poland" };
            CountryResponse country = _countriesService.AddCountry(countryAddRequest);
            PlayerAddRequest playerAddRequest = new PlayerAddRequest() { Nickname = "ropz", CountryID = country.CountryID, Mouse = MouseEnum.Logitech, Mousepad = "artisan" };
            PlayerResponse player = _playersService.AddPlayer(playerAddRequest);
            PlayerUpdateRequest playerUpdateRequest = player.ToPlayerUpdateRequest();
            playerUpdateRequest.Nickname = "mezii";
            playerUpdateRequest.Mouse = MouseEnum.Pulsar;

            PlayerResponse updatedPlayer = _playersService.UpdatePlayer(playerUpdateRequest);
            PlayerResponse? playerFromGetByID = _playersService.GetPlayerByID(updatedPlayer.PlayerID);

            Assert.Equal(playerFromGetByID, updatedPlayer);

        }
        #endregion

        #region DeletePlayer
        [Fact]
        public void DeletePlayer_InvalidPlayerID()
        {
            bool isDeleted = _playersService.DeletePlayer(Guid.NewGuid());
            Assert.False(isDeleted);
        }

        [Fact]
        public void DeletePlayer_ValidPlayerID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Poland" };
            CountryResponse country = _countriesService.AddCountry(countryAddRequest);
            PlayerAddRequest playerAddRequest = new PlayerAddRequest() { CountryID = country.CountryID, Nickname = "Niko" };
            PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);

            bool isDeleted = _playersService.DeletePlayer(playerResponse.PlayerID);
            Assert.True(isDeleted);
        }

        #endregion
    }
}
