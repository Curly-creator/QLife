using QLifeC_Datatool;
using Xunit;

namespace Unittest
{
    public class API_Request_Unittest
    {

        [Fact]
        public void InvalidURL_Empty_Test()
        {
            //Sends an Request to API with an Empthy URL
            API_Request Request = new API_Request("", 1);
            Request.GetCityScores();

            Assert.True(Request.UrlFormatError);
        }

        [Fact]
        public void InvalidURL_WrongFormat_Test()
        {
            //Sends an Request to API with a wrong URL format
            API_Request Request = new API_Request("THIS IS NOT AN URL", 1);
            Request.GetCityScores();

            Assert.True(Request.UrlFormatError);
        }

        [Fact]

        public void Connection_ServerNotFound_Test()
        {
            //Sends an Request to API with an wrong URL
            API_Request Request = new API_Request("https://wrongurl.org/this_is_not_correct", 1);
            Request.GetCityScores();

            Assert.True(Request.ConnectionError);
        }

        [Fact]
        public void NumberOfCities_Null_Test()
        {
            //Sends an Request to API with 0 cities
            API_Request Request = new API_Request("https://api.teleport.org/api/urban_areas", 0);
            Request.GetCityScores();

            Assert.True(Request.NumberOfCitiesError);
        }

        [Fact]
        public void NumberOfCities_Negativ_Test()
        {
            //Sends an Request to API with -1 cities
            API_Request Request = new API_Request("https://api.teleport.org/api/urban_areas", -1);
            Request.GetCityScores();

            Assert.True(Request.NumberOfCitiesError);
        }

        [Fact]
        public void NumberOfCities_TooBig_Test()
        {
            //Sends an Request to API with too many cities
            API_Request Request = new API_Request("https://api.teleport.org/api/urban_areas", 666);
            Request.GetCityScores();

            Assert.True(Request.NumberOfCitiesError);
        }
    }
}
