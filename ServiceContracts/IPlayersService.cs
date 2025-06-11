using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPlayersService
    {
        PlayerResponse AddPlayer(PlayerAddRequest? request);
        List<PlayerResponse> GetAllPlayers();
    }
}
