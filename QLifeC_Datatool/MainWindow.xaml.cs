using Nancy.Json;
using Nancy.ModelBinding.DefaultBodyDeserializers;
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
        public CategoryID categorieID = new CategoryID();        

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

            int indexArray = 0;
            foreach (var item in scores)
            {             
                foreach (var name in categorieID.Name)
                {
                    if (scores[indexScore]["name"] == name)
                    {
                        Category category = new Category();                                                    
                        category.Score.Color = scores[indexScore]["color"];
                        category.Score.Name = scores[indexScore]["name"];
                        category.Score.ScoreOutOf10 = scores[indexScore]["score_out_of_10"];
                        city.Categories[indexArray] = category;
                        indexArray++;
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

            int indexCategorie = 0;
            
            foreach (var item in categories)
            {
                int indexName = 0;
                foreach ( var id in categorieID.Name)
                {
                    if (categories[indexCategorie]["label"] == id)
                    {
                        int i = 0;
                        foreach (var attribute in city.Categories)
                        {
                            if (city.Categories[i].Score.Name == id) break;
                            i++;
                        }
                        city.Categories[i].Label = categories[indexCategorie]["label"];
                        city.Categories[i].Id = categories[indexCategorie]["id"];
                        
                        var datapoints = jsonObj["categories"][indexCategorie]["data"];
                        
                        foreach (var datapoint in datapoints)
                        {
                            string type = datapoint["type"];
                            
                            Data data = new Data
                            {
                                Id = datapoint["id"],
                                Type = datapoint["type"],
                                Label = datapoint["label"]
                            };

                            if (type == "url" || type == "string") data.StringValue = datapoint[type + "_value"];

                            else data.NumberValue = datapoint[type + "_value"];

                            city.Categories[indexName].Data.Add(data);
                        }
                    }
                    indexName++;
                }
                indexCategorie++;
            }
        }

        private void btn_NewEntry_Click(object sender, RoutedEventArgs e)
        {
            AddCity addCityWindow = new AddCity();
            addCityWindow.ShowDialog();

            Dgd_MainGrid.Items.Refresh();              //refresh testCityList im View
        }

        private void btn_DelEntry_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to continue?", "Deleting Chosen City", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    int i = (Dgd_MainGrid.SelectedIndex);
                    cityList.RemoveAt(i);
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("The city is still here.", "Deleting Chosen City");
                    break;
            }
            Dgd_MainGrid.Items.Refresh();                   // refresh the datagrid (after deleting the selected city)
        }

        private void btn_UpdEntry_Click(object sender, RoutedEventArgs e)
        {
            if (Dgd_MainGrid.SelectedValue != null)
            {
                
                AddCity editCityWindow = new AddCity(cityList[Dgd_MainGrid.SelectedIndex]);
                editCityWindow.ShowDialog();
                Dgd_MainGrid.Items.Refresh();
            }
            else
                MessageBox.Show("Please first select a city that you would like to edit");

        }
    }
}
