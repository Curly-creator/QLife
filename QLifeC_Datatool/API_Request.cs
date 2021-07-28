using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;

namespace QLifeC_Datatool
{
    public class API_Request
    {
        private string _Url;
        private List<City> _cityList;
        private bool _ConnectionError = false;
        private bool _UrlFormatError = false;
        private bool _IntervallError = false;

        readonly Exception NullIntervall = new Exception("Intervall is 0. Function can not be performed");
        readonly Exception NegativIntervall = new Exception("Intervall is negativ. Function can not be performed");

        public List<City> CityList { get => _cityList; set => _cityList = value; }
        public string Url { get => _Url; set => _Url = value; }

        public bool GetIntervallError()
        {
            return _IntervallError;
        }

        private void SetIntervallError(bool value)
        {
            _IntervallError = value;
        }

        public bool GetConnectionError()
        {
            return _ConnectionError;
        }

        private void SetConnectionError(bool value)
        {
            _ConnectionError = value;
        }

        public bool GetUrlFormatError()
        {
            return _UrlFormatError;
        }

        private void SetUrlFormatError(bool value)
        {
            _UrlFormatError = value;
        }

        public API_Request(string url) 
        {
            Url = url;
            CityList = new List<City>();
        }

        public List<City> GetCityScores(int intervall)
        {
            try
            {
                GetCityList(intervall);
                foreach (var city in CityList)
                {
                    GetCategoryScores(city);
                    GetCategoryDetails(city);
                }
                return CityList;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                SetIntervallError(true);
                return null;
            }       
        }

        private dynamic UrlToJsonObj(string url)
        {
            SetUrlFormatError(false);
            SetConnectionError(false);

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
                SetConnectionError(true);
                return null;
            }
            catch (UriFormatException e)
            {
                MessageBox.Show("Error: " + e.Message);
                SetUrlFormatError(true);
                return null;
            }
        }

        private void GetCityList(int intervall)
        {
            SetIntervallError(false);

            dynamic jsonObj = UrlToJsonObj(Url);
            if (intervall > 0)
            {
                if (jsonObj != null)
                {
                    var jsonCities = jsonObj["_links"]["ua:item"];

                    for (int i = 0; i < jsonCities.Count; i += intervall)
                    {
                        City city = new City
                        {
                            Url = jsonCities[i]["href"],
                            Name = jsonCities[i]["name"]
                        };
                        CityList.Add(city);
                    }
                }
            }
            else if (intervall == 0) throw NullIntervall;
            else throw NegativIntervall; 
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
