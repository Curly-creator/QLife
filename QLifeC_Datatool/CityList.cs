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

        public void AddCity(City city)
        {
            Backup.Add(city);
            Clear();
            AddRange(Backup);
        }
        public void RemoveCity(City city)
        {
            Backup.Remove(city);
            Clear();
            AddRange(Backup);
        }
        public void EditCity(City city, int index)
        {
            Backup[index] = city;
            Clear();
            AddRange(Backup);

        }

        public void UpdateCityList(CityList cityList)
        {
            Clear();
            Backup.Clear();
            AddRange(cityList);
            Backup.AddRange(cityList);
        }


        public void GetCityScores(string url, int numberOfCities)
        {
            API_Request aPI_Request = new API_Request(url, numberOfCities);
            UpdateCityList(aPI_Request.GetCityData());
        }

        public void FilterByCategoryScore(double[] valueOfFilter, bool[] filterIsActive)
        {
            List<City> FilterList = new List<City>();

            foreach (var city in Backup)
                if (Filter(city, 0, valueOfFilter, filterIsActive))
                    FilterList.Add(city);
            Clear();
            AddRange(FilterList);
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

        public void Reset()
        {
            this.Clear();
            this.AddRange(Backup);
        }

    }
}
