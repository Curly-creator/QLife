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
        //[Fact]
        //Unit test to check if xml upload works
        //Unit test to validate xml file success

        //public void OpenFileDialog_ShouldReturn_FilePath()
        //{
        //    //Arrange: With this action, you prepare all the required data and preconditions.
        //    string expected = ".xml";

        //    //Act: This action performs the actual test.
            
        //    string actual = "";

        //    //Assert: This final action checks if the expected result has occurred.
        //    Assert.Equal(expected, actual);
        //}
        [Fact]
        public void Should_Deserialize_XMLFile()
        {
            //Arrange: setting up Path of the xml file as the passing parameter for the method DeserializeXML.
            AdapterFileImport xmlSchemaValidator = new AdapterFileImport();
            string path = "C:\\Users\\Tanvi\\source\\repos\\qlifec_datatool\\QLifeC_Datatool\\bin\\Debug\\netcoreapp3.1\\ExportedXML_Test.xml";

            //Act
            xmlSchemaValidator.DeserializeXML(path);

            //Assert
            Assert.True(xmlSchemaValidator.MethodStatus, "Fail");
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
            
            //Assert: This should be true in accordance with the validation status which should also be true.
            Assert.True(xmlSchemaValidator.MethodStatus, xmlSchemaValidator.StatusNotification);
        }

        [Fact]
        public void Should_Parse_CSVFile()
        {
            AdapterFileImport xmlSchemaValidator = new AdapterFileImport();
            string path = "C:\\Users\\Tanvi\\source\\repos\\qlifec_datatool\\QLifeC_Datatool\\bin\\Debug\\netcoreapp3.1\\Export_for_Oral_Exam_Final.csv";

            xmlSchemaValidator.ReadParseCSV(path);

            Assert.True(xmlSchemaValidator.MethodStatus, xmlSchemaValidator.StatusNotification);
        }
    }
}
