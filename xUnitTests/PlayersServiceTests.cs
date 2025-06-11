using System;
using System.Collections.Generic;
using Xunit;
using Entities;
using ServiceContracts;
using Services;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace xUnitTests
{
    public class PlayerServiceTests
    {
        private readonly IPlayersService _playersService;
        public PlayerServiceTests()
        {
            _playersService = new PlayersService();
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
            List<PlayerAddRequest> playerRequestsList = new List<PlayerAddRequest>()
            {
                new PlayerAddRequest() {
                    Nickname = "xyz",
                    Team = "A",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sda",
                    CountryID = Guid.NewGuid(),
                    DateOfBirth = DateTime.Parse("2000-11-11")
                },
                new PlayerAddRequest() {
                    Nickname = "xcz",
                    Team = "Aas",
                    Mouse = MouseEnum.Razer,
                    Mousepad = "sa",
                    CountryID = Guid.NewGuid(),
                    DateOfBirth = DateTime.Parse("2001-11-11")
                }
            };

            List<PlayerResponse> playersListActual = new List<PlayerResponse>();
            foreach (PlayerAddRequest playerAddRequest in playerRequestsList)
            {
                PlayerResponse playerResponse = _playersService.AddPlayer(playerAddRequest);
                playersListActual.Add(playerResponse);
            }
            List<PlayerResponse> playerListFromGetAll = _playersService.GetAllPlayers();
            foreach (PlayerResponse playerResponse in playersListActual)
            {
                Assert.Contains(playerResponse, playerListFromGetAll);
            }

            #endregion
        }
    }
}
