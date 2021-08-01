using Nancy.Json;
using System;
using System.IO;
using System.Net;
using System.Windows;

namespace QLifeC_Datatool
{
    public class API_Request
    {
        private string _Url;
        private CityList _CityList;
        private int _NumberOfCities;
        private bool _ConnectionError = false;
        private bool _UrlFormatError = false;
        private bool _NumberOfCitiesError = false;

        public CityList CityList { get => _CityList; set => _CityList = value; }
        public string Url { get => _Url; set => _Url = value; }

        //Errors
        public int NumberOfCities { get => _NumberOfCities; set => _NumberOfCities = value; }
        public bool ConnectionError { get => _ConnectionError; set => _ConnectionError = value; }
        public bool UrlFormatError { get => _UrlFormatError; set => _UrlFormatError = value; }
        public bool NumberOfCitiesError { get => _NumberOfCitiesError; set => _NumberOfCitiesError = value; }

        //Exeptions
        readonly Exception NullCityNumber = new Exception("Number of City is 0. Function can not be performed");
        readonly Exception NegativCityNumber = new Exception("Number of City is negativ. Function can not be performed");
        readonly Exception TooBigCityNumber = new Exception("Number of City is too big. Maximum number is 266");

        //Konstruktor
        public API_Request(string url, int numberOfCities) 
        {
            Url = url;
            CityList = new CityList();
            NumberOfCities = numberOfCities;
        }

        /// <summary>
        /// Get List of Cities and CityScores
        /// </summary>
        /// <returns></returns>
        public CityList GetCityScores()
        {
            try
            {
                NumberOfCitiesError = false;
                GetCityList(NumberOfCities);
                foreach (var city in CityList)
                {
                    GetCategoryScores(city);
                    GetCategoryDetails(city);
                    city.Id = city.GetHashCode();
                }
                return CityList;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                NumberOfCitiesError = true;
                return null;
            }
        }

        /// <summary>
        /// Serialize Data from API request to json Object
        /// </summary>
        /// <param name="url"></param>
        /// <returns>json Object</returns>
        private dynamic UrlToJsonObj(string url)
        {
            UrlFormatError = false;
            ConnectionError = false;

            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                using Stream dataStreay = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStreay);
                string responseString = reader.ReadToEnd();
                dynamic jsonObj = new JavaScriptSerializer().Deserialize<Object>(responseString);
                return jsonObj;
            }
            catch (WebException e)
            {
                MessageBox.Show("Error: " + e.Message);
                ConnectionError = true;
                return null;
            }
            catch (UriFormatException e)
            {
                MessageBox.Show("Error: " + e.Message);
                UrlFormatError = true;
                return null;
            }
        }

        /// <summary>
        /// Gets Random Cities from API and adds them to CityList. Max NumberOfCities 266.
        /// </summary>
        /// <param name="numberOfCities"></param>
        private void GetCityList(int numberOfCities)
        {
            if (numberOfCities < 0) throw NegativCityNumber;
            if (numberOfCities == 0) throw NullCityNumber;
            

            dynamic jsonObj = UrlToJsonObj(Url);
            Random random = new Random();
            if (jsonObj != null)
            {
                var jsonCities = jsonObj["_links"]["ua:item"];

                if (numberOfCities > jsonCities.Count) throw TooBigCityNumber;

                int intervall = jsonCities.Count / numberOfCities;

                for (int i = 0; i < numberOfCities; i ++)
                {
                    int randomCity = intervall * i + random.Next(0, intervall - 1);

                    if (randomCity > jsonCities.Count-1) randomCity = jsonCities.Count;

                    City city = new City
                    {
                        Url = jsonCities[randomCity]["href"],
                        Name = jsonCities[randomCity]["name"]
                    };
                    CityList.Add(city);                
                }
            }
        }

        /// <summary>
        /// Gets the CategoryScores for a City from API and saves them in  City
        /// </summary>
        /// <param name="city"></param>
        private void GetCategoryScores(City city)
        {
            var url = CityList[CityList.IndexOf(city)].Url + "scores/";

            dynamic jsonObj = UrlToJsonObj(url);

            if (jsonObj != null)
            {
                var jsonCategoryScores = jsonObj["categories"];

                int counter = 0;

                foreach (var jsonScore in jsonCategoryScores)
                {
                    for (int i = 0; i < city.Categories.Length; i++)
                    {
                        if (jsonScore["name"] == city.Categories[i].Label)
                        {
                            city.Categories[i].Score = jsonScore["score_out_of_10"];
                            counter++;
                            break;
                        }
                    }
                    if (counter == city.Categories.Length) break;
                }
            }
        }

        /// <summary>
        /// Gets the SubCategoryScores for a City from API and saves them in City
        /// </summary>
        /// <param name="city"></param>
        private void GetCategoryDetails(City city)
        {
            var url = CityList[CityList.IndexOf(city)].Url + "details/";

            dynamic jsonObj = UrlToJsonObj(url);

            if (jsonObj != null)
            {
                var jsonCategories = jsonObj["categories"];

                int counter = 0;

                foreach (var jsonCategory in jsonCategories)
                {
                    for (int i = 0; i < city.Categories.Length; i++)
                    {
                        if (jsonCategory["label"] == city.Categories[i].Label)
                        {
                            AddSubCategories(city.Categories[i], jsonCategory);
                            counter++;
                            break;
                        }
                    }
                    if (counter == city.Categories.Length) break;
                }
            }
        }

        /// <summary>
        /// Gets Scores for Subcategories from API and saves them in Category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="jsonCategory"></param>
        private void AddSubCategories(Category category, dynamic jsonCategory)
        {
            foreach (var jsonSubCategory in jsonCategory["data"])
            {
                string type = jsonSubCategory["type"];

                SubCategory subCategory = new SubCategory
                {
                    Type = jsonSubCategory["type"],
                    Label = jsonSubCategory["label"],
                };

                subCategory.Value = jsonSubCategory[type + "_value"];
                category.SubCategories.Add(subCategory);
            }
        }
    }
}
