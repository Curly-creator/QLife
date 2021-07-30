using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using QLifeC_Datatool;
using System.Xml.Schema;
using System.Xml;
using System.IO;

namespace UnitTest
{
    public class ImportFeatureTesting
    {
        //Initializing both the adapters to test the methods in them.
        AdapterXML xmlAdapter = new AdapterXML();
        AdapterCSV TestCSV = new AdapterCSV();

        [Fact]
        public void Should_Deserialize_XMLFile_ReturnsTrue()
        {
            //Arrange: setting up Path of the xml file as the passing parameter for the method DeserializeXML.
            string pathstart = Directory.GetCurrentDirectory();
            //This is the full path with the name of the file in the current directory.
            string path = pathstart + "\\Unit_Test_Files\\CityList_Backup_XML.xml";

            //Act: Performing the actual test of running the method DeserializeXML.
            xmlAdapter.DeserializeXML(path);

            //Assert: This should be true in accordance with the MethodStatus which should also be true.
            Assert.True(xmlAdapter.MethodStatus, xmlAdapter.StatusNotification);
        }

        [Fact]
        public void Should_Deserialize_XMLFile_ReturnsFalse()
        {
            //Arrange: setting up Path of the xml file as the passing parameter for the method DeserializeXML.
            string pathstart = System.IO.Directory.GetCurrentDirectory();
            //This is the full path with the name of the file in the current directory.
            string path = pathstart + "\\Unit_Test_Files\\CityList_Backup_CSV.csv";

            //Act: Performing the actual test of running the method DeserializeXML.
            xmlAdapter.DeserializeXML(path);

            //Assert: This should be false in accordance with the MethodStatus which should also be false.
            Assert.False(xmlAdapter.MethodStatus, xmlAdapter.StatusNotification);
        }

        [Fact]
        public void Should_Validate_XMLFile_AgainstXSD_ReturnsTrue()
        {
            //Arrange: setting up Path of the xml file as the passing parameter for the method ValidateXML.
            string pathstart = System.IO.Directory.GetCurrentDirectory();
            //This is the full path with the name of the file in the current directory.
            string path = pathstart + "\\Unit_Test_Files\\CityList_Backup_XML.xml";

            //Act: Performing the actual test of running the method ValidateXML.
            xmlAdapter.ValidateXML(path);

            //Assert: This should be true in accordance with the MethodStatus which should also be true.
            Assert.True(xmlAdapter.MethodStatus, xmlAdapter.StatusNotification);
        }

        [Fact]
        public void Should_Validate_XMLFile_AgainstXSD_ReturnsFalse()
        {
            //Arrange: setting up Path of the xml file as the passing parameter for the method ValidateXML.
            string pathstart = System.IO.Directory.GetCurrentDirectory();
            //This is the full path with the name of the file in the current directory.
            string path = pathstart + "\\Unit_Test_Files\\CityList_Backup_CSV.csv";

            //Act: Performing the actual test of running the method ValidateXML.
            xmlAdapter.ValidateXML(path);

            //Assert: This should be false in accordance with the MethodStatus which should also be false.
            Assert.False(xmlAdapter.MethodStatus, xmlAdapter.StatusNotification);
        }

        [Fact]
        public void Should_Parse_CSVFile_ReturnsTrue()
        {
            //Arrange: setting up Path of the csv file as the passing parameter for the method ReadParseCSV.
            string pathstart = System.IO.Directory.GetCurrentDirectory();
            //This is the full path with the name of the file in the current directory.
            string path = pathstart + "\\Unit_Test_Files\\CityList_Backup_CSV.csv";

            //Act: Performing the actual test of running the method ReadParseCSV.
            TestCSV.ReadParseCSV(path);

            //Assert: This should be true in accordance with the MethodStatus which should also be true.
            Assert.True(TestCSV.MethodStatus, TestCSV.StatusNotification);
        }

        //[Fact]
        //public void Should_CheckFileExtandCallAdapter_ReturnsTrue()
        //{
        //    MainWindow window = new MainWindow();
        //    //Arrange:
        //    //Arrange: Setting up Path of the csv file as the passing parameter for the method ReadParseCSV.
        //    string pathstart = System.IO.Directory.GetCurrentDirectory();
        //    //This is the full path with the name of the file in the current directory.
        //    string path = pathstart + "\\Export_for_Oral_Exam_Final.xml";
        //    string ext = ".xml";
            
        //    window.CheckFileExtandImport(ext, path);

        //    Assert.True(window.MethodStatus, window.ErrorNotification);
        //}


    }
}
