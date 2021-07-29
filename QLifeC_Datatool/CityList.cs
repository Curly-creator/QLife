using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace QLifeC_Datatool
{
    public class CityList : List<City>
    {
        private bool reverse = false;
        private List<City> _Backup = new List<City>();
        private readonly CategoryScoreComparer categoryComparer = new CategoryScoreComparer();
        private readonly CityNameComparerDecending nameComparerDecending = new CityNameComparerDecending();
        private readonly CityNameComparerAcending nameComparerAcending = new CityNameComparerAcending();

        public List<City> Backup { get => _Backup; set => _Backup = value; }

        public void UpdateBackup()
        {
            Backup = this;
        }

        public void GetCityScores(string url, int numberOfCities)
        {
            API_Request aPI_Request = new API_Request(url, numberOfCities);
            this.AddRange(aPI_Request.GetCityData());
            UpdateBackup();
        }

        public List<City> FilterByCategoryScore(double[] valueOfFilter, bool[] filterIsActive)
        {
            List<City> FilterList = new List<City>();

            foreach (var city in this)
                if (Filter(city, 0, valueOfFilter, filterIsActive))
                    FilterList.Add(city);
            
            return FilterList;
        }

        private bool Filter(City city, int indexOfCategory, double[] valueOfFilter, bool[] filterIsActive)
        {
            if (indexOfCategory < city.Categories.Length)
                if (filterIsActive[indexOfCategory])
                    if (city.Categories[indexOfCategory].Score >= valueOfFilter[indexOfCategory])
                        return Filter(city, indexOfCategory + 1, valueOfFilter, filterIsActive);
                    else return false;
                else return Filter(city, indexOfCategory + 1, valueOfFilter, filterIsActive);
            return true;
        }

        public List<City> SearchByCityName(string searchText)
        {
            List<City> searchList = new List<City>();

            if (searchText != "")
                searchText = searchText[0].ToString().ToUpper() + searchText.Substring(1);
            else return this;

            foreach (var city in this)
                if (city.Name.Contains(searchText))
                    searchList.Add(city);

            return searchList;
        }

        public void SortByCategoryScore(int indexOfCategory)
        {
            categoryComparer.Index = indexOfCategory;
            Sort(categoryComparer);
        }

        public void SortByCityName()
        {
            if (reverse)
            {
                Sort(nameComparerAcending);
                reverse = false;
            }
            else
            {
                Sort(nameComparerDecending);
                reverse = true;
            }       
        }

        //public void Reset()
        //{
        //    this.Clear();
        //    this.AddRange(Backup);
        //}

    }
}
