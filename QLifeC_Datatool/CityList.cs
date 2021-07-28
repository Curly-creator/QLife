using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace QLifeC_Datatool
{
    public class CityList : List<City>
    {
        private readonly List<City> Backup = new List<City>();
        private readonly CategoryScoreComparer categoryComparer = new CategoryScoreComparer();
        private readonly CityNameComparer nameComparer = new CityNameComparer();

        public void GetCityScores(string url)
        {
            API_Request aPI_Request = new API_Request(url);
            this.AddRange(aPI_Request.GetCityData());
            Backup.AddRange(this);
        }

        public void FilterByCategoryScore(double[] valueOfFilter, bool[] filterIsActive)
        {
            List<City> FilterList = new List<City>();

            foreach (var city in Backup)        
                if (Filter(city, 0, valueOfFilter, filterIsActive))          
                    FilterList.Add(city);

            this.Clear();
            this.AddRange(FilterList);
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
            if (nameComparer.Acending) nameComparer.Acending = false;
            else nameComparer.Acending = true;

            Sort(nameComparer);
        }

        public void Reset()
        {
            this.Clear();
            this.AddRange(Backup);
        }

    }
}
