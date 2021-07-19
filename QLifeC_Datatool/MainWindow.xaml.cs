﻿using Nancy.Extensions;
using Nancy.Json;
using Nancy.ModelBinding.DefaultBodyDeserializers;
using Nancy.ViewEngines;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace QLifeC_Datatool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public List<City> cityList = new List<City>();
        public CategoryName CategoryName = new CategoryName();

        public MainWindow()
        {
            InitializeComponent();

            API_GetCityList();

            int indexCity = 0;
            foreach (var item in cityList)
            {
                API_GetCategoryScores(cityList[indexCity]);
                indexCity++;
            }

            //List<SubCategory> SubCat = new List<SubCategory>();

            //foreach (object item in cityList[0].Categories[0].SubCategory)
            //{
            //    SubCat.Add((SubCategory)item);                  
            //}
            //cmb_Sub1.ItemsSource = SubCat;
            //int i = 0;
            //foreach (object item in cityList)
            //{
            //    SubCat.Add((SubCategory)cityList[i].Categories[0].SubCategory[0]);
            //        i++;
            //}
                    
            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        public dynamic API_UrlToJsonObj(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using Stream dataStreay = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStreay);
            string responseString = reader.ReadToEnd();

            dynamic jsonObj = new JavaScriptSerializer().Deserialize<Object>(responseString);
            return jsonObj;
        }

        public void API_GetCityList()
        {    
            var url = "https://api.teleport.org/api/urban_areas";

            dynamic jsonObj = API_UrlToJsonObj(url);
           
            var cities = jsonObj["_links"]["ua:item"];
            
            for(int i = 0; i <= 200; i += 10)
            {          
                City testCity = new City
                {
                    Url = cities[i]["href"],
                    Name = cities[i]["name"]
                };
                cityList.Add(testCity);
            };
        }

        public void API_GetCategoryScores(City city)
        {
            var url = cityList[cityList.IndexOf(city)].Url + "scores/";

            dynamic jsonObj = API_UrlToJsonObj(url);

            var scores = jsonObj["categories"];

            int indexScore = 0;
        
            
            foreach (var item in scores)
            {             
                foreach (var name in CategoryName.Name)
                {
                    if (scores[indexScore]["name"] == name)
                    {
                        Category categorie = new Category();                                                    
                        //categorie.Score.Color = scores[indexScore]["color"];
                        categorie.Score.Name = scores[indexScore]["name"];
                        categorie.Score.ScoreOutOf10 = scores[indexScore]["score_out_of_10"];                   
                        city.Categories.Add(categorie);
                        }                   
                }
                indexScore++;
            }
            API_GetCityData(city);
        }

        public void API_GetCityData(City city)
        {
            var url = cityList[cityList.IndexOf(city)].Url + "details/";

            dynamic jsonObj = API_UrlToJsonObj(url);

            var categories = jsonObj["categories"];

            int indexOfCategorie = 0;           
            foreach (var category in categories)
            {
                int indexOfName = 0;
                foreach ( var name in CategoryName.Name)
                {
                    if (categories[indexOfCategorie]["label"] == name)
                    {
                        int i = 0;
                        foreach (var attribute in city.Categories)
                        {
                            if (city.Categories[i].Score.Name == name) break;
                            i++;
                        }
                        city.Categories[i].Label = categories[indexOfCategorie]["label"];
                        city.Categories[i].Id = categories[indexOfCategorie]["id"];
                        
                        var datapoints = jsonObj["categories"][indexOfCategorie]["data"];
                        
                        foreach (var datapoint in datapoints)
                        {
                            string type = datapoint["type"];
                            
                            SubCategory subCategory = new SubCategory
                            {
                                Type = datapoint["type"],
                                Label = datapoint["label"]
                            };

                            if (type == "url" || type == "string") subCategory.StringValue = datapoint[type + "_value"];

                            else subCategory.NumberValue = datapoint[type + "_value"];

                            city.Categories[indexOfName].SubCategory.Add(subCategory);
                        }
                    }
                    indexOfName++;
                }
                indexOfCategorie++;
            }
        }

        public void SearchCity()
        {
            ComparerCity compareCity = new ComparerCity();
            List<City> searchList = new List<City>();
                      
            City searchObject = new City
            {
                Name = tbx_SearchBar.Text
            };

            searchList.Add(searchObject);

            int index = cityList.BinarySearch(searchObject, compareCity);

            if (index >= 0)
            {
                searchList[0] = cityList[index];
                Dgd_MainGrid.ItemsSource = searchList;
            }
            else Dgd_MainGrid.ItemsSource = cityList;

            Dgd_MainGrid.Items.Refresh();
        }

        public void Filter()
        {
            List<City> FilterList = new List<City>();
            
            foreach (var city in cityList)
            {
                if (city.Categories[0].Score.ScoreOutOf10 > sl_CoLFilter.Value)
                {
                    if (city.Categories[1].Score.ScoreOutOf10 > sl_HCFilter.Value)
                    {
                        if (city.Categories[2].Score.ScoreOutOf10 > sl_IAFilter.Value)
                        {
                            if (city.Categories[3].Score.ScoreOutOf10 > sl_EQFilter.Value)
                            {
                                if (city.Categories[4].Score.ScoreOutOf10 > sl_TCFilter.Value)
                                {
                                    if (city.Categories[5].Score.ScoreOutOf10 > sl_OFilter.Value)
                                    {
                                        FilterList.Add(city);
                                    }
                                }
                            }
                        }
                    }
                }              
            }
           
            Dgd_MainGrid.ItemsSource = FilterList;
            Dgd_MainGrid.Items.Refresh();        
        }


        private void tbx_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchCity();
        }

        private void btn_Filter_Click(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void sl_CoLFilter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_CoL.Content = Math.Round(sl_CoLFilter.Value, 1);            
        }

        private void sl_HCFilter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_HC.Content = Math.Round(sl_HCFilter.Value, 1);            
        }

        private void sl_IAFilter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {           
            lbl_IA.Content = Math.Round(sl_IAFilter.Value, 1);
        }

        private void sl_EQFilter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_EQ.Content = Math.Round(sl_EQFilter.Value, 1);
            
        }

        private void sl_TCFilter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_TC.Content = Math.Round(sl_TCFilter.Value, 1);
        }

        private void sl_OFilter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lbl_O.Content = Math.Round(sl_OFilter.Value, 1);
        }
    }
}
