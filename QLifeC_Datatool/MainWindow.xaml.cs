using Nancy.Extensions;
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
//using Newtonsoft.Json;

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

            cmb_Filter.ItemsSource = CategoryName.Name;
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
                                //Id = datapoint["id"],
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
            SearchCity Search = new SearchCity();
            List<City> FilterList = new List<City>();
                      
            City searchingCity = new City
            {
                Name = tbx_SearchBar.Text
            };

            FilterList.Add(searchingCity);

            int index = cityList.BinarySearch(searchingCity, Search);

            if (index >= 0)
            {
                FilterList[0] = cityList[index];
                Dgd_MainGrid.ItemsSource = FilterList;
            }
            else Dgd_MainGrid.ItemsSource = cityList;

            Dgd_MainGrid.Items.Refresh();
        }

        public void Filter_Min()
        {
            List<City> FilterList = new List<City>();

            foreach (var city in cityList)
            {
                if (city.Categories[cmb_Filter.SelectedIndex].Score.ScoreOutOf10 < int.Parse(tbx_Filter.Text)) FilterList.Add(city);
            }

            Dgd_MainGrid.ItemsSource = FilterList;
            Dgd_MainGrid.Items.Refresh();
        }

        public void Filter(bool reverse)
        {
            List<City> FilterList = new List<City>();
            
            foreach (var city in cityList)
            {
                if (reverse && city.Categories[cmb_Filter.SelectedIndex].Score.ScoreOutOf10 > int.Parse(tbx_Filter.Text)) FilterList.Add(city);
                if (!reverse && city.Categories[cmb_Filter.SelectedIndex].Score.ScoreOutOf10 < int.Parse(tbx_Filter.Text)) FilterList.Add(city);
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
            if(tbx_Filter.Text != null)
            {
                if (int.Parse(tbx_Filter.Text) >= 0 && int.Parse(tbx_Filter.Text) < 10)
                {
                    if ((bool)cb_FilterMax.IsChecked) Filter(true);
                    if ((bool)cb_FilterMin.IsChecked) Filter(false);
                }
            }                           
        }

        private void cb_FilterMin_Checked(object sender, RoutedEventArgs e)
        {
            cb_FilterMax.IsChecked = false;
        }

        private void cb_FilterMax_Checked(object sender, RoutedEventArgs e)
        {
            cb_FilterMin.IsChecked = false;
        }
    }
}
