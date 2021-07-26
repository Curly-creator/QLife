using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using QLifeC_Datatool;
using System.Xml.Schema;
using System.Xml;

namespace UnitTest
{
    public class ImportFeatureTesting
    {
        [Fact]
        public void Should_Deserialize_XMLFile()
        {
            //Arrange: setting up Path of the xml file as the passing parameter for the method DeserializeXML.
            AdapterFileImport xmlSchemaValidator = new AdapterFileImport();
            string path = "C:\\Users\\Tanvi\\source\\repos\\qlifec_datatool\\QLifeC_Datatool\\bin\\Debug\\netcoreapp3.1\\ExportedXML_Test.xml";

            //Act: Performing the actual test of running the method DeserializeXML.
            xmlSchemaValidator.DeserializeXML(path);

            //Assert: This should be true in accordance with the MethodStatus which should also be true.
            Assert.True(xmlSchemaValidator.MethodStatus, xmlSchemaValidator.StatusNotification);
        }

        [Fact]
        public void Should_Validate_XMLFile_AgainstXSD()
        {
            AdapterFileImport xmlSchemaValidator = new AdapterFileImport();

            //Arrange: setting up Path of the xml file as the passing parameter for the method ValidateXML.
            //string path = "C:\\Users\\Tanvi\\Documents\\File Import testing\\NewExport_1.xml";
            string path = "C:\\Users\\Tanvi\\source\\repos\\qlifec_datatool\\QLifeC_Datatool\\bin\\Debug\\netcoreapp3.1\\ExportedXML_Test.xml";

            //Act: Performing the actual test of running the method ValidateXML.
            xmlSchemaValidator.ValidateXML(path);

            //Assert: This should be true in accordance with the MethodStatus which should also be true.
            Assert.True(xmlSchemaValidator.MethodStatus, xmlSchemaValidator.StatusNotification);
        }

        [Fact]
        public void Should_Parse_CSVFile()
        {
            AdapterFileImport xmlSchemaValidator = new AdapterFileImport();

            //Arrange: setting up Path of the csv file as the passing parameter for the method ReadParseCSV.
            string path = "C:\\Users\\Tanvi\\source\\repos\\qlifec_datatool\\QLifeC_Datatool\\bin\\Debug\\netcoreapp3.1\\Export_for_Oral_Exam_Final.csv";

            //Act: Performing the actual test of running the method ReadParseCSV.
            xmlSchemaValidator.ReadParseCSV(path);

            //Assert: This should be true in accordance with the MethodStatus which should also be true.
            Assert.True(xmlSchemaValidator.MethodStatus, xmlSchemaValidator.StatusNotification);
        }

        [Fact]
        public void Should_CheckIfValidFile()
        {
            AdapterFileImport xmlSchemaValidator = new AdapterFileImport();

            //Arrange: setting up Paths and File Extension for the method CallImportAdapter.
            xmlSchemaValidator.ImportFilePath = "C:\\Users\\Tanvi\\source\\repos\\qlifec_datatool\\QLifeC_Datatool\\bin\\Debug\\netcoreapp3.1\\Export_for_Oral_Exam_Final.csv";
            //string pathxml = "C:\\Users\\Tanvi\\source\\repos\\qlifec_datatool\\QLifeC_Datatool\\bin\\Debug\\netcoreapp3.1\\ExportedXML_Test.xml";
            string extension = ".csv";

            //Act: Performing the actual test of running the method ReadParseCSV.
            xmlSchemaValidator.CallImportAdapter(extension);

            //Assert: This should be true in accordance with the MethodStatus which should also be true.
            Assert.True(xmlSchemaValidator.MethodStatus, xmlSchemaValidator.StatusNotification);
        }

        //public void OpenFileDialog_ShouldReturn_FilePath()
        //{
        //    //Arrange: With this action, you prepare all the required data and preconditions.
        //    string expected = ".xml";

        //    //Act: This action performs the actual test.

        //    string actual = "";

        //    //Assert: This final action checks if the expected result has occurred.
        //    Assert.Equal(expected, actual);
        //}
    }
}
