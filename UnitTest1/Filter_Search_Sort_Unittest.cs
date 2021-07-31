using QLifeC_Datatool;
using System;
using System.Collections.Generic;
using Xunit;


namespace Unittest
{
    public class Filter_Search_Sort_Unittest
    {
        readonly CityList cityList = new CityList();

        readonly string[] cityNames = new string[]
        {
            "Berlin", "Brisbane", "Dubai", "Kiev", "Cork", "Aarhus"
        };

        readonly bool[] filterStatus = new bool[5];
        readonly double[] filterValue = new double[5];

        internal void FillCityList()
        {
            var score = new Random();
            for (int i = 0; i <= 5; i++)
            {
                City city = new City { Name = cityNames[i] };
                cityList.Add(city);
            }

            foreach (var city in cityList)
            {
                for (int i = 0; i < city.Categories.Length; i++)
                {
                    city.Categories[i].Score = score.NextDouble() * 10;
                }
            }
        }

        internal void FillFilterArrays()
        {
            var value = new Random();
            
            for(int i = 0; i < 5; i++)
            {                
                filterValue[i] = value.NextDouble() * 10;
                if(value.NextDouble() < 0.5) filterStatus[i] = true;
                else filterStatus[i] = false;
            }
        }

        internal bool CheckOrderCategoryScore(CityList city, int counter)
        {
            while( counter+1 < city.Count)
            {
                if (city[counter].Categories[0].Score > city[counter + 1].Categories[0].Score)
                {
                    counter++;
                    return CheckOrderCategoryScore(city, counter);
                }
                else return false;
            }
            return true;
        }

        internal bool CheckFilterCategoryScore(CityList city)
        {
            bool result = true;
            for (int i = 0; i < city.Count; i++)
            {
                for (int j = 0; j < city[j].Categories.Length; j++)
                {
                    if (filterStatus[j])
                    {
                        if (filterValue[j] <= city[i].Categories[j].Score)                       
                            result = true;                      
                        else return false;
                    }
                }
            }
            return result;
        }

        [Fact]
        public void Test_FilterByCityScore_True()
        {
            FillCityList();
            FillFilterArrays();
  
            //Arrange
            bool expected = true;

            //Act
            cityList.FilterByCategoryScore(filterValue, filterStatus);

            bool actual = CheckFilterCategoryScore(cityList);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_FilterByCityScore_False()
        {
            FillCityList();
            FillFilterArrays();         

            //Arrange
            bool expected = false;

            //Act
            bool actual = CheckFilterCategoryScore(cityList);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_SearchByCityName_Random()
        {
            FillCityList();

            //Arrange            
            var name = new Random();
            int index = name.Next(cityNames.Length);
            string expected = cityNames[index];

            //Act
            List<City> searchList = cityList.SearchByCityName(cityNames[index]);
            string actual = searchList[0].Name;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_SearchByCityName_Empty()
        {
            FillCityList();

            //Arrange
            List<City> expected = cityList;

            //Act           
            List<City> actual = cityList.SearchByCityName("");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_SortByCategoryScore_True()
        {
            FillCityList();

            //Arrange
            bool expected = true;

            //Act
            cityList.SortByCategoryScore(0);
            bool actual = CheckOrderCategoryScore(cityList, 0);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_SortByCategoryScore_False()
        {
            FillCityList();

            //Arrange
            bool expected = false;

            //Act
            bool actual = CheckOrderCategoryScore(cityList, 0);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
