using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using QLifeC_Datatool;

namespace UnitTest
{
    public class ImportFeatureTesting
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
