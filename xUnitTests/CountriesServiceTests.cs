using System;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Microsoft.EntityFrameworkCore;

namespace xUnitTests
{
    public class CountriesServiceTests
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTests()
        {
            _countriesService = new CountriesService(new PlayersDbContext(new DbContextOptionsBuilder<PlayersDbContext>().Options));
        }
        #region AddCountry
        //country request is null -> null exception
        [Fact]
        public void AddCountry_NullCountry()
        {
            CountryAddRequest? request = null;

            Assert.Throws<ArgumentNullException>(() => 
            {  
                _countriesService.AddCountry(request); 
            });
        }

        //country name is null -> argument exception
        [Fact]
        public void AddCountry_NullCountryName()
        {
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };

            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }

        //country name is duplicate -> argument exception
        [Fact]
        public void AddCountry_NullCountryNameDuplicate()
        {
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "usa" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "usa" };

            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //country name is unique -> insert into the list of countries
        [Fact]
        public void AddCountry_ProperRequest()
        {
            CountryAddRequest request = new CountryAddRequest() { CountryName = "japan" };

            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> allCountries = _countriesService.GetAllCountries();

            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, allCountries);
            
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            List<CountryResponse> actualList = _countriesService.GetAllCountries();
            Assert.Empty(actualList);
        }
        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> countryRequestList = new List<CountryAddRequest>() { 
            new CountryAddRequest() { CountryName = "japan" },
            new CountryAddRequest() { CountryName = "poland" },
            new CountryAddRequest() { CountryName = "usa" }
            };

            List<CountryResponse> countryResponseList = new List<CountryResponse>();
            foreach (CountryAddRequest request in countryRequestList)
            {
                countryResponseList.Add(_countriesService.AddCountry(request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();
            
            foreach(CountryResponse response in countryResponseList)
            {
                Assert.Contains(response, actualCountryResponseList);
            }
            
        }

        #endregion

        #region GetCountryById
        //Id < 1
        //Id not in list
        //Id == null
        //Provided not int as id
        [Fact]
        public void GetCountryByID_NullCountryID() 
        {
            //Arrange
            Guid? countryID = null;
            //Act
            CountryResponse? response = _countriesService.GetCountryByID(countryID);
            //Assert
            Assert.Null(response);

        }

        [Fact]
        public void GetCountryByID_ValidCountryID()
        {
            //Arrange
            CountryResponse addCountryResponse = _countriesService.AddCountry(new CountryAddRequest() { CountryName = "poland" });
            Guid countryID = addCountryResponse.CountryID;
            //Act
            CountryResponse? getCountryByIdResponse = _countriesService.GetCountryByID(countryID);
            //Assert
            Assert.Equal(addCountryResponse, getCountryByIdResponse);

        }

        #endregion
    }
}
