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
        public void TestingNullListXML() //testing a list that has not been initialized with AdapterXML
        {
            string mypath = Directory.GetCurrentDirectory() + "\\Unit_Test_Files\\nullList.xml";
            Stream teststream = File.OpenWrite(mypath);
            AdapterXML nullXML = new AdapterXML(teststream, nullCityList);
            Assert.False(nullXML.MethodStatus);

            teststream.Close();
        }

        [Fact]
        public void TestingEmptyList() //testing a list that contains data or has at least been initialized
        {
            string mypath = Directory.GetCurrentDirectory() + "\\Unit_Test_Files\\emptyList.xml";
            Stream teststream = File.OpenWrite(mypath);
            AdapterXML emptyXML = new AdapterXML(teststream, emptyList);
            Assert.True(emptyXML.MethodStatus);

            teststream.Close();
        }

        [Fact]
        public void TestingListWithData() //testing a list that contains data
        {
            string mypath = Directory.GetCurrentDirectory() + "\\Unit_Test_Files\\mockCityList.csv";
            Stream teststream = File.OpenWrite(mypath);
            City Testcity = new City
            {
                Name = "Mocktown"
            };
            mockCityList.Add(Testcity);
            AdapterCSV mockCSV = new AdapterCSV(teststream, mockCityList);
            Assert.True(mockCSV.MethodStatus);

            teststream.Close();
        }

        [Fact]
        public void TestingNullListCSV() //testing a list that has not been initialized with AdapterCSV
        {
            string mypath = Directory.GetCurrentDirectory() + "\\Unit_Test_Files\\nullList.csv";
            Stream teststream = File.OpenWrite(mypath);
            AdapterCSV nullCSV = new AdapterCSV(teststream, nullCityList);
            Assert.False(nullCSV.MethodStatus);

            teststream.Close();
        }
    }
}
