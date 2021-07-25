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
        public void ValidateXMLSchema()
        {
            AdapterFileImport xmlSchemaValidator = new AdapterFileImport();
            
            //XmlSchema myXmlSchema = XmlSchema.Read();
            
            //XmlDocument myXmlDocument = new XmlDocument();
            //myXmlDocument.Load("C:\\Users\\Tanvi\\Documents\\File Import testing\\NewExport_1.xml");
            
            xmlSchemaValidator.ImportFilePath = "C:\\Users\\Tanvi\\Documents\\File Import testing\\NewExport_1.xml";

            //xmlSchemaValidator.ValidateXML();
                        
            Assert.True(xmlSchemaValidator.Validationstatus, "Validation unsuccessful");
        }
    }
}
