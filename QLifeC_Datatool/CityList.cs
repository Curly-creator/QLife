using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace QLifeC_Datatool
{
    public class CityList : List<City>
    {
        public CityList()
        {
          
        }

        public List<City> Filter(double[] valueOfFilter, bool[] filterIsActive)
        {
            List<City> FilterList = new List<City>();
            foreach (var city in this)
            {
                if (FilterByScore(city, 0, valueOfFilter, filterIsActive))
                {
                    FilterList.Add(city);
                }
            }
            return FilterList;
        }

        public bool FilterByScore(City city, int indexOfCategory, double[] valueOfFilter, bool[] filterIsActive)
        {
            if (indexOfCategory < city.Categories.Length)
            {
                if (filterIsActive[indexOfCategory])
                {
                    if (city.Categories[indexOfCategory].Score >= valueOfFilter[indexOfCategory])
                    {
                        return FilterByScore(city, indexOfCategory + 1, valueOfFilter, filterIsActive);
                    }
                    else return false;
                }
                else return FilterByScore(city, indexOfCategory + 1, valueOfFilter, filterIsActive);
            }
            return true;
        }

        public List<City> SearchCity(string searchText)
        {
            List<City> searchList = new List<City>();
       
            if (searchText != "")
                searchText = searchText[0].ToString().ToUpper() + searchText.Substring(1);

            foreach (var city in this)
                if (city.Name.Contains(searchText))
                    searchList.Add(city);

            return searchList;
        }

        public void SortByCategoryScore()
        {
            ComparerCity comparerCity = new ComparerCity();
            foreach (var city in this)
            {
                //cityList.Sort(comparerCity);


            }
        }

    }
}
