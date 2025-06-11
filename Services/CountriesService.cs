using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null) 
            { 
                throw new ArgumentNullException(nameof(CountryAddRequest)); 
            }
            if(countryAddRequest.CountryName == null) 
            { 
                throw new ArgumentException(nameof(CountryAddRequest.CountryName)); 
            }
            if (_countries.Exists(c => c.CountryName == countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country with this name already exists."); 
            }
          
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }
            Country? country = _countries.FirstOrDefault(c => c.CountryID == countryID);
            if (country == null) 
            {
                return null;
            }
            return country.ToCountryResponse();
        }
    }
}
