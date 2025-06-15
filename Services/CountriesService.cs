using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>() 
                {
                    new Country() { CountryID = Guid.Parse("A52C23B2-777A-4D43-9E5A-2E1C6D730EBB"), CountryName = "Poland" },
                    new Country() { CountryID = Guid.Parse("E4AA3C20-D609-4179-A744-81572EFEA56E"), CountryName = "China" },
                    new Country() { CountryID = Guid.Parse("E517F036-6F16-4A79-89C7-D2FA5D40D289"), CountryName = "Brazil" },
                    new Country() { CountryID = Guid.Parse("FA7648A4-6A0B-4B96-BCC7-D7AA1D0D3B9E"), CountryName = "France" },
                });
            }
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
