using System;
using Xunit;

namespace Unittest
{
    public class UnitTest
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            double expected = 13.0;

            //Act
            double actual = 13.0;

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
