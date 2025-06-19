using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PlayersDbContext _db;

        public CountriesService(PlayersDbContext dbContext)
        {
            _db = dbContext;
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
            if (_db.Countries.Count(c => c.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Country with this name already exists."); 
            }
          
            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();
            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }
            Country? country = _db.Countries.FirstOrDefault(c => c.CountryID == countryID);
            if (country == null) 
            {
                return null;
            }
            return country.ToCountryResponse();
        }
    }
}
