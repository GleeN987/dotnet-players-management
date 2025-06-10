using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Interface with bussiness logic for manipulating country entities
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds new country object to list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns></returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();
        CountryResponse? GetCountryByID(Guid? countryID);
    }
}
