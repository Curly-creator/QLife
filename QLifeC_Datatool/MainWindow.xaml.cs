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


namespace QLifeC_Datatool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public List<City> cityList = new List<City>();
        public CategoryName CategoryName = new CategoryName();
        public Slider[] FilterSliderArray;
        public CheckBox[] FilterCheckBoxArray;
        public Label[] FilterLabelArray;

        public MainWindow()
        {
            InitializeComponent();
       
            API_GetCityList();

            int indexOfCity = 0;
            foreach (var item in cityList)
            {
                API_GetCategoryScores(cityList[indexOfCity]);
                indexOfCity++;
            }

            FilterSliderArray = new Slider[] { sl_CoLFilter, sl_HCFilter, sl_IAFilter, sl_EQFilter, sl_TCFilter,  sl_OFilter };
            FilterCheckBoxArray = new CheckBox[] { cb_CoLFilter, cb_HCFilter, cb_IAFilter, cb_EQFilter, cb_TCFilter, cb_OFilter };
            FilterLabelArray = new Label[] { lbl_CoL, lbl_HC, lbl_IA, lbl_EQ, lbl_TC, lbl_O };

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
                City city = new City
                {
                    Url = cities[i]["href"],
                    Name = cities[i]["name"]
                };
                cityList.Add(city);
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

        public bool FilterByScore(City city, int indexOfCategory)
        {   
            if (indexOfCategory < 5)
            {
                if ((bool)FilterCheckBoxArray[indexOfCategory].IsChecked)
                {
                    if (city.Categories[indexOfCategory].Score.ScoreOutOf10 >= FilterSliderArray[indexOfCategory].Value)
                    {
                        if (FilterByScore(city, indexOfCategory + 1)) return true;

                        else return false;
                    }
                    else return false;
                }
                else return true;
            }
            return true;
        }


        private void tbx_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchCity();
        }

        private void btn_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<City> FilterList = new List<City>();
            foreach (var city in cityList)
            {
                if (FilterByScore(city, 0)) FilterList.Add(city);
            }
                       
            Dgd_MainGrid.ItemsSource = FilterList;
            Dgd_MainGrid.Items.Refresh();           
        }

        public void SliderValueChanged(object slider, RoutedPropertyChangedEventArgs<double> e)
        {
            for (int i = 0; i < FilterSliderArray.Length; i++)
            {
                if (slider == FilterSliderArray[i])
                {
                    FilterLabelArray[i].Content = String.Format("Filtervalue: {0,1:N1}", Math.Round(FilterSliderArray[i].Value, 1));
                }    
            }        
        }
    }
}
