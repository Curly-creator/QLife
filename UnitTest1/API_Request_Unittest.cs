using QLifeC_Datatool;
using Xunit;

namespace Unittest
{
    public class API_Request_Unittest
    {
        
        [Fact]
        public void InvalidURL_Empty_Test()
        {
            API_Request Request = new API_Request("");
            Request.GetCityScores(1);

            //Arrange
            bool expected = true;

            //Act
            bool actual = Request.GetUrlFormatError();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InvalidURL_WrongFormat_Test()
        {
            API_Request Request = new API_Request("asdfvdsfhfdsgfv");
            Request.GetCityScores(1);

            //Arrange
            bool expected = true;

            //Act
            bool actual = Request.GetUrlFormatError();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        
        public void Connection_ServerNotFound_Test()
        {
            API_Request Request = new API_Request("https://api.t.org/api/urban_areas");
            Request.GetCityScores(20);

            //Arrange
            bool expected = true;

            //Act
            bool actual = Request.GetConnectionError();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IntervallError_Null_Test()
        {
            API_Request Request = new API_Request("https://api.teleport.org/api/urban_areas");
            Request.GetCityScores(0);

            //Arrange
            bool expected = true;

            //Act
            bool actual = Request.GetIntervallError();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IntervallError_Negativ_Test()
        {
            API_Request Request = new API_Request("https://api.teleport.org/api/urban_areas");
            Request.GetCityScores(-1);

            //Arrange
            bool expected = true;

            //Act
            bool actual = Request.GetIntervallError();

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
