using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using QLifeC_Datatool;
using System.IO;
using Xunit.Sdk;

namespace UnitTest
{
    public class DownloadFeatureTesting
    {
        //for Test1:
        public List<City> nullCityList;

        //for Test2:
        public List<City> someCityList = new List<City>();

        //for Test3:
        

        [Fact]
        public void Test1() //testing a list that has not been initialized
        {
            CsvFile testfile = new CsvFile
            {
                Filename = "nullList.csv",
                CSVStream = File.OpenWrite(@"C:\\Users\\ThinkPad T540p\\Desktop\\nullList.csv"),
                SourceForCsv = nullCityList
            };

            testfile.WriteToCSV();

            //WriteToCSV checks if the SourceForCsv can be read with Any() method before it actually starts writing the stream

            bool exportFail = testfile.ExportResult;

            Assert.False(exportFail);

        }

        [Fact]
        public void Test2() //testing a list that contains data or has at least been initialized
        {
            CsvFile testfile = new CsvFile
            {
                Filename = "someCityList.csv",
                CSVStream = File.OpenWrite(@"C:\\Users\\ThinkPad T540p\\Desktop\\someCityList.csv"),
                SourceForCsv = someCityList
            };

            //someCityList contains data and should be exported with success
            //as long as a list has been initialized the export will be successful even though it does not contain data

            testfile.WriteToCSV();

            bool exportSuccess = testfile.ExportResult;

            Assert.True(exportSuccess);

        }

        [Fact]
        public void Test3() //testing a list that has not been initialized
        {
            XmlFile testfile = new XmlFile
            {
                Filename = "catList.xml",
                XMLStream = File.OpenWrite(@"C:\\Users\\ThinkPad T540p\\Desktop\\catList.xml"),
                SourceForXml = nullCityList
            };

            testfile.WriteToXML();

            bool exportFail = testfile.ExportResult;

            Assert.False(exportFail);

        }
    }
}
