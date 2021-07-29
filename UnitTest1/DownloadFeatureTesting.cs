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
        //for Test1 and for Test4:
        public CityList nullCityList;

        //for Test2:
        public CityList emptyList = new CityList();

        //for Test3:
        public CityList mockCityList = new CityList();


        [Fact]
        public void Test1() //testing a list that has not been initialized with AdapterXML
        {
            string mypath = Directory.GetCurrentDirectory() + "\\DownloadTestingFiles\\nullList.xml";
            Stream teststream = File.OpenWrite(mypath);
            AdapterXML nullXML = new AdapterXML();
            nullXML.CallExportAdapter(teststream, nullCityList);
            Assert.False(nullXML.MethodStatus);

            teststream.Close();
        }

        [Fact]
        public void Test2() //testing a list that contains data or has at least been initialized
        {
            string mypath = Directory.GetCurrentDirectory() + "\\DownloadTestingFiles\\emptyList.xml";
            Stream teststream = File.OpenWrite(mypath);
            AdapterXML emptyXML = new AdapterXML();
            emptyXML.CallExportAdapter(teststream, emptyList);
            Assert.True(emptyXML.MethodStatus);

            teststream.Close();
        }

        [Fact]
        public void Test3() //testing a list that contains data
        {
            string mypath = Directory.GetCurrentDirectory() + "\\DownloadTestingFiles\\mockCityList.csv";
            Stream teststream = File.OpenWrite(mypath);
            City Testcity = new City();
            Testcity.Name = "Mocktown";
            mockCityList.Add(Testcity);
            AdapterCSV mockCSV = new AdapterCSV();
            mockCSV.CallExportAdapter(teststream, mockCityList);
            Assert.True(mockCSV.MethodStatus);

            teststream.Close();
        }

        [Fact]
        public void Test4() //testing a list that has not been initialized with AdapterCSV
        {
            string mypath = Directory.GetCurrentDirectory() + "\\DownloadTestingFiles\\nullList.csv";
            Stream teststream = File.OpenWrite(mypath);
            AdapterCSV nullCSV = new AdapterCSV();
            nullCSV.CallExportAdapter(teststream, nullCityList);
            Assert.False(nullCSV.MethodStatus);

            teststream.Close();
        }
    }
}
