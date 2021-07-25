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
        //Unit test to check if xml upload works
        //Unit test to validate xml file success
        public void OpenFileDialog_ShouldReturn_FilePath()
        {
            //Arrange: With this action, you prepare all the required data and preconditions.
            string expected = ".xml";

            //Act: This action performs the actual test.
            
            string actual = "";

            //Assert: This final action checks if the expected result has occurred.
            Assert.Equal(expected, actual);
        }
    }
}
