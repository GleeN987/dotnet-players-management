using ServiceContracts.DTO;
using ServiceContracts.Enums;


namespace ServiceContracts
{
    public interface IPlayersService
    {
        PlayerResponse AddPlayer(PlayerAddRequest? request);
        List<PlayerResponse> GetAllPlayers();
        PlayerResponse? GetPlayerByID(Guid? playerID);
        List<PlayerResponse> GetFilteredPlayers(string searchBy, string? searchString);
        List<PlayerResponse> GetSortedPlayers(List<PlayerResponse> allPlayers, string sortBy, SortOrder sortOrder);
        PlayerResponse UpdatePlayer(PlayerUpdateRequest? request);
        bool DeletePlayer(Guid? playerID);
    }
}
