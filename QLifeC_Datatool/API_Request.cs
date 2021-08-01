using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
        private bool _IntervallError = false;

        public CityList CityList { get => _CityList; set => _CityList = value; }
        public string Url { get => _Url; set => _Url = value; }
        public int NumberOfCities { get => _NumberOfCities; set => _NumberOfCities = value; }
        public bool ConnectionError { get => _ConnectionError; set => _ConnectionError = value; }
        public bool UrlFormatError { get => _UrlFormatError; set => _UrlFormatError = value; }
        public bool IntervallError { get => _IntervallError; set => _IntervallError = value; }

        readonly Exception NullIntervall = new Exception("Intervall is 0. Function can not be performed");
        readonly Exception NegativIntervall = new Exception("Intervall is negativ. Function can not be performed");

        public API_Request(string url, int numberOfCities) 
        {
            Url = url;
            CityList = new CityList();
            NumberOfCities = numberOfCities;
        }

        public CityList GetCityScores()
        {
            try
            {
                GetCityList(NumberOfCities);
                foreach (var city in CityList)
                {
                    GetCategoryScores(city);
                    GetCategoryDetails(city);
                }
                return CityList;
            }
            catch (DivideByZeroException e)
            {
                MessageBox.Show("Error: " + e.Message);
                IntervallError = true;
                return null;
            }
            catch (ArgumentOutOfRangeException e)
            {
                MessageBox.Show("Error: " + e.Message);
                IntervallError = true;
                return null;
            }
            catch (FormatException e)
            {
                MessageBox.Show("Error: " + e.Message);
                IntervallError = true;
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                IntervallError = true;
                return null;
            }
        }

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

        private void GetCityList(int numberOfCities)
        {
            dynamic jsonObj = UrlToJsonObj(Url);
            Random random = new Random();
            if (jsonObj != null)
            {
                var jsonCities = jsonObj["_links"]["ua:item"];

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
