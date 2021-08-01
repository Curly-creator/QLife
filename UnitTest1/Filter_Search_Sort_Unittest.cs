using QLifeC_Datatool;
using System;
using System.Collections.Generic;
using Xunit;


namespace Unittest
{
    public class Filter_Search_Sort_Unittest
    {
        readonly CityList TestCityList = new CityList();
        
        //Names for TestCityList
        readonly string[] cityNames = new string[]
        {
            "Berlin", "Brisbane", "Dubai", "Kiev", "Cork", "Aarhus"
        };

        //Values and States for Filter
        readonly bool[] filterStatus = new bool[6];
        readonly double[] filterValue = new double[6];

        /// <summary>
        /// This Mehtod fills the TestCityList with random values 
        /// </summary>
        internal void FillCityList()
        {
            var score = new Random();
            for (int i = 0; i <= 5; i++)
            {
                City city = new City { Name = cityNames[i] };
                TestCityList.Add(city);
            }

            foreach (var city in TestCityList)
            {
                for (int i = 0; i < city.Categories.Length; i++)
                {
                    city.Categories[i].Score = score.NextDouble() * 10;
                }
            }
        }

        /// <summary>
        /// This Mehtod fills the Filter with random values and sets all Filters to active
        /// </summary>
        internal void FillFilterArrays()
        {
            var value = new Random();
            
            for(int i = 0; i < 6; i++)
            {                
                filterValue[i] = value.NextDouble() * 10;
                filterStatus[i] = true;
            }
        }

        /// <summary>
        /// Checks if the order in TestCityList is decending
        /// </summary>
        /// <param name="city"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the TestCityList contains only Cities with Scores higher oder equal than the FilterValues.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        internal bool CheckFilterCategoryScore(CityList city)
        {
            bool result = true;
            for (int i = 0; i < city.Count; i++)
            {
                for (int j = 0; j < city[0].Categories.Length; j++)
                {
                    if (filterStatus[j])
                    {
                        if (filterValue[j] <= city[i].Categories[j].Score)                       
                            result = true;                      
                        else return false;
                    }
                    else result = true;
                }
            }
            return result;
        }

        [Fact]
        public void Test_FilterByCityScore_True()
        {
            //Arrange
            FillCityList();
            FillFilterArrays();

            //Act
            TestCityList.FilterByCategoryScore(filterValue, filterStatus);

            //Assert
            Assert.True(CheckFilterCategoryScore(TestCityList));
        }

        [Fact]
        public void Test_FilterByCityScore_False()
        {
            //Arrange
            FillCityList();
            FillFilterArrays();         
            
            //Act

            //Assert
            Assert.False(CheckFilterCategoryScore(TestCityList));
        }

        [Fact]
        public void Test_SearchByCityName_Random()
        {
            //Arrange
            FillCityList();
         
            var name = new Random();
            int index = name.Next(cityNames.Length);
            string expected = cityNames[index];

            //Act
            List<City> searchList = TestCityList.SearchByCityName(cityNames[index]);
            string actual = searchList[0].Name;
         
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_SearchByCityName_Empty()
        {
            //Arrange
            FillCityList();

            List<City> expected = TestCityList;
      
            //Act
            List<City> actual = TestCityList.SearchByCityName("");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_SortByCategoryScore_True()
        {
            //Arrange
            FillCityList();

            //Act
            TestCityList.SortByCategoryScore(0);

            //Assert
            Assert.True(CheckOrderCategoryScore(TestCityList, 0));
        }

        [Fact]
        public void Test_SortByCategoryScore_False()
        {
            //Arrange
            FillCityList();
          
            //Act

            //Assert
            Assert.False(CheckOrderCategoryScore(TestCityList, 0));
        }
    }
}
