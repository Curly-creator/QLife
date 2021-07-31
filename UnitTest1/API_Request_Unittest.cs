using QLifeC_Datatool;
using Xunit;

namespace Unittest
{
    public class API_Request_Unittest
    {

        [Fact]
        public void InvalidURL_Empty_Test()
        {
            API_Request Request = new API_Request("", 20);
            Request.GetCityScores();

            Assert.True(Request.UrlFormatError);
        }

        [Fact]
        public void InvalidURL_WrongFormat_Test()
        {
            API_Request Request = new API_Request("asdfvdsfhfdsgfv", 20);
            Request.GetCityScores();

            Assert.True(Request.UrlFormatError);
        }

        [Fact]

        public void Connection_ServerNotFound_Test()
        {
            API_Request Request = new API_Request("https://api.t.org/api/urban_areas", 20);
            Request.GetCityScores();

            Assert.True(Request.ConnectionError);
        }

        [Fact]
        public void IntervallError_Null_Test()
        {
            API_Request Request = new API_Request("https://api.teleport.org/api/urban_areas", 0);
            Request.GetCityScores();

            Assert.True(Request.IntervallError);
        }

        [Fact]
        public void IntervallError_Negativ_Test()
        {
            API_Request Request = new API_Request("https://api.teleport.org/api/urban_areas", -1);
            Request.GetCityScores();

            Assert.True(Request.IntervallError);
        }
    }
}
