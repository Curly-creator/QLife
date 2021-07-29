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
        public CityList CityList { get => _CityList; set => _CityList = value; }
        public string Url { get => _Url; set => _Url = value; }
        public int NumberOfCities { get => _NumberOfCities; set => _NumberOfCities = value; }

        public API_Request(string url, int numberOfCities) 
        {
            Url = url;
            CityList = new CityList();
            NumberOfCities = numberOfCities;
        }

        public CityList GetCityData()
        {
            GetCityList(NumberOfCities);
            foreach (var city in CityList)
            {
                GetCategoryScores(city);
                GetCategoryDetails(city);
            }
            return CityList;
        }

        private dynamic UrlToJsonObj(string url)
        {
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
            catch (WebException)
            {
                MessageBox.Show("No Connection to Server. Please check your Internet Connection.");
                return null;
            }
        }

        private void GetCityList(int numberOfCities)
        {
            dynamic jsonObj = UrlToJsonObj(Url);
            
            if (jsonObj != null)
            {
                var jsonCities = jsonObj["_links"]["ua:item"];
                int intervall = jsonCities.Count / numberOfCities;

                if (intervall < 1) intervall = 1;
                
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
