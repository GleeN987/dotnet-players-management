using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PlayerResponse
    {
        public Guid PlayerID { get; set; }
        public string? Nickname { get; set; }
        public string? Team { get; set; }
        public string? Mouse { get; set; }
        public string? Mousepad { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PlayerResponse)) return false; 

            PlayerResponse player = (PlayerResponse)obj;
            return player.PlayerID == PlayerID && player.Nickname == Nickname && 
                player.Team == Team && player.Mouse == Mouse &&
                player.Mousepad == Mousepad && player.CountryID == CountryID && 
                player.Country == Country && player.DateOfBirth == DateOfBirth &&
                player.Age == Age;

        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public static class PlayerExtensions
    {
        public static PlayerResponse ToPlayerResponse(this Player player)
        {
            return new PlayerResponse()
            {
                PlayerID = player.PlayerID,
                Nickname = player.Nickname,
                Team = player.Team,
                Mouse = player.Mouse,
                Mousepad = player.Mousepad,
                CountryID = player.CountryID,
                DateOfBirth = player.DateOfBirth,
                Age = (player.DateOfBirth != null) ? Math.Round((DateTime.Now - player.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
        
    }
}
