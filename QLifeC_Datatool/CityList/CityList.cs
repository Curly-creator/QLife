using System;
using System.Collections.Generic;
using System.Windows;

namespace QLifeC_Datatool
{
    public class CityList : List<City>
    {
        private bool reverse = false;
        private readonly CategoryScoreComparer categoryComparer = new CategoryScoreComparer();
        private readonly CityNameComparerDecending nameComparerDecending = new CityNameComparerDecending();
        private readonly CityNameComparerAcending nameComparerAcending = new CityNameComparerAcending();

        //BackupList - Is not affected by Filter- or Sort-Method
        public List<City> Backup { get; set; } = new List<City>();

        /// <summary>
        /// Adds City to CityList and BackupList
        /// </summary>
        /// <param name="city"></param>
        public void AddCity(City city)
        {
            Backup.Add(city);
            Clear();
            AddRange(Backup);
        }

        /// <summary>
        /// Removes City from CityList and BackupList
        /// </summary>
        /// <param name="city"></param>
        public void RemoveCity(City city)
        {
            Backup.Remove(city);
            Clear();
            AddRange(Backup);
        }

        /// <summary>
        /// Edits City in CityList and BackupList
        /// </summary>
        /// <param name="city"></param>
        /// <param name="index"></param>
        public void EditCity(City city, int index)
        {
            Backup[index] = city;
            Clear();
            AddRange(Backup);
        }

        /// <summary>
        /// Removes all Cites from CityList/BackupList and Adds an new CityList
        /// </summary>
        /// <param name="cityList"></param>
        public void UpdateCityList(CityList cityList)
        {
            try
            {
                Clear();
                Backup.Clear();
                foreach(var city in cityList)
                {
                    city.Id = city.GetHashCode();
                    Add(city);
                    Backup.Add(city);

                }
            }
            catch(ArgumentNullException e)
            {
                MessageBox.Show("File import fail. Data corrupt.");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Sets a new Instance of API_Request with the url and the number of cities
        /// </summary>
        /// <param name="url"></param>
        /// <param name="numberOfCities"></param>
        public void GetCityScores(string url, int numberOfCities)
        {
            API_Request aPI_Request = new API_Request(url, numberOfCities);
            try
            {
                UpdateCityList(aPI_Request.GetCityScores());
            }
            catch (Exception)
            {
                
            }            
        }

        /// <summary>
        /// Checks if CategoryScores of Citys are in Filterrange. If true the City is added to FilterList, if false not.
        /// </summary>
        /// <param name="valueOfFilter"></param>
        /// <param name="filterIsActive"></param>
        public void FilterByCategoryScore(double[] valueOfFilter, bool[] filterIsActive)
        {
            List<City> FilterList = new List<City>();

            foreach (var city in Backup)
                if (Filter(city, 0, valueOfFilter, filterIsActive))
                    FilterList.Add(city);
            Clear();
            AddRange(FilterList);
        }

        /// <summary>
        /// Checks if Value of CategoryScore is bigger or equal than FilterValues. If true check next Category, else false.
        /// </summary>
        /// <param name="city"></param>
        /// <param name="indexOfCategory"></param>
        /// <param name="valueOfFilter"></param>
        /// <param name="filterIsActive"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if any City.Name in CityList contains searchText. If true the city is added to searchList.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>List of Cities which contain searchText</returns>
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

        /// <summary>
        /// Sorts CityList decending by the Score of the selected Category
        /// </summary>
        /// <param name="indexOfCategory"></param>
        public void SortByCategoryScore(int indexOfCategory)
        {
            categoryComparer.Index = indexOfCategory;
            Sort(categoryComparer);
        }

        /// <summary>
        /// Sorts the CityList by the Name of the Cities. Switches between acending and decending sort.
        /// </summary>
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
    }
}
