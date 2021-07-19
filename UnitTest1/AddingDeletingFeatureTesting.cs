using QLifeC_Datatool;
using System;
using System.Collections.Generic;
using System.Windows;
using Xunit;

namespace UnitTest
{
    public class AddingDeletingFeatureTesting
    {
        [Fact]
        public void TestAdding()
        {
            //Arrange
            City cityToBeAdded;
            List<City> cityList = new List<City>();
            int citiesInList1 = cityList.Count;
            cityToBeAdded = new City("Berlin");
            cityToBeAdded.Categories[0].Score.ScoreOutOf10 = 9.0;
            cityToBeAdded.Categories[1].Score.ScoreOutOf10 = 4.1;
            cityToBeAdded.Categories[2].Score.ScoreOutOf10 = 4.2;
            cityToBeAdded.Categories[3].Score.ScoreOutOf10 = 4.3;
            cityToBeAdded.Categories[4].Score.ScoreOutOf10 = 4.4;
            cityToBeAdded.Categories[5].Score.ScoreOutOf10 = 4.5;
            cityList.Add(cityToBeAdded);            
            int citiesInList2 = cityList.Count;

            //Act
            string before = (citiesInList1+1).ToString();
            string after = citiesInList2.ToString();
            
            //Assert: testing the city counts
            Assert.Equal(before, after);
        }

        /*[Fact]
        public void TestDeleting()
        {
            //Arrange
            double expected = 13.0;

            //Act
            double actual = 13.0;

            //Assert
            Assert.Equal(expected, actual);
        }*/
    }
}
