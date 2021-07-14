using Nancy.Json;
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
        public List<TestCity> testCityList = new List<TestCity>();

        public MainWindow()
        {
            InitializeComponent();
            Dgd_MainGrid.ItemsSource = testCityList;

            API_GetCityList();

            for(int i = 0; i < testCityList.Count; i += 8)
            {
                API_GetCityData(testCityList[i]);
            }                  
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

            foreach (var item in cities)
            {
                TestCity testCity = new TestCity
                {
                    Url = item["href"],
                    Name = item["name"]
                };
                testCityList.Add(testCity);
            }
        }

        public void API_GetCategorieScores(TestCity city)
        {
            var url = testCityList[testCityList.IndexOf(city)].Url + "scores/";

            dynamic jsonObj = API_UrlToJsonObj(url);

            var scores = jsonObj["categories"];
            int indexScore = 0;
            foreach (var score in scores)
            {
                Score categorieScore = new Score
                {
                    Color = scores[indexScore]["color"],
                    Name = scores[indexScore]["name"],
                    ScoreOutOf10 = scores[indexScore]["score_out_of_10"]
                };
                city.Categories[indexScore].Score = categorieScore;

                indexScore++;
            }
        }

        public void API_GetCityData(TestCity city)
        {
            var url = testCityList[testCityList.IndexOf(city)].Url + "details/";

            dynamic jsonObj = API_UrlToJsonObj(url);

            var categories = jsonObj["categories"];

            int indexCategorie = 0;

            foreach (var item in categories)
            {
                Categorie categorie = new Categorie
                {
                    Lable = categories[indexCategorie]["label"],
                    Id = categories[indexCategorie]["id"]
                };

                var datapoints = jsonObj["categories"][indexCategorie]["data"];
                indexCategorie++;
                foreach (var datapoint in datapoints)
                {
                    string type = datapoint["type"];
                    Data data = new Data
                    {
                        Id = datapoint["id"],
                        Type = datapoint["type"],
                        Label = datapoint["label"]
                    };

                    if (data.Id != "WEATHER-SUNSHINE-AMOUNT") //Datenquelle fehlerhaft, "WHEATHER-SUNSHINE-AMOUNT" ist als float angegeben wird aber als string übergeben
                    {
                        if (type == "url" || type == "string") data.StringValue = datapoint[type + "_value"];

                        else data.NumberValue = datapoint[type + "_value"];
                    }
                    categorie.Data.Add(data);
                }
                city.Categories.Add(categorie);
            }

            API_GetCategorieScores(city);

            //List<City> cityList = new List<City>();
            //City city = new City();

            //categories.Add(jsonObj["categories"]);                   
            //Data data = JsonConvert.DeserializeObject<Data>(responseString);



            ////CostOfLiving
            //city.CostOfLiving.InflationScore = jsonObj["categories"][3]["data"][0]["float_value"];
            //city.CostOfLiving.AKgApples = jsonObj["categories"][3]["data"][1]["currency_dollar_value"];
            //city.CostOfLiving.Bread = jsonObj["categories"][3]["data"][2]["currency_dollar_value"];
            //city.CostOfLiving.ACappucchino = jsonObj["categories"][3]["data"][3]["currency_dollar_value"];
            //city.CostOfLiving.MovieTicket = jsonObj["categories"][3]["data"][4]["currency_dollar_value"];
            //city.CostOfLiving.MthlyFitnessClubMmbship = jsonObj["categories"][3]["data"][5]["currency_dollar_value"];
            //city.CostOfLiving.ABeer = jsonObj["categories"][3]["data"][6]["currency_dollar_value"];
            //city.CostOfLiving.MthlyPublicTransport = jsonObj["categories"][3]["data"][7]["currency_dollar_value"];
            //city.CostOfLiving.Lunch = jsonObj["categories"][3]["data"][8]["currency_dollar_value"];
            //city.CostOfLiving.A5kmTaxiRide = jsonObj["categories"][3]["data"][9]["currency_dollar_value"];
            //city.CostOfLiving.MealInRestaurant = jsonObj["categories"][3]["data"][10]["currency_dollar_value"];

            ////EnviromentalQualitiy
            //city.EnvironmentalQuality.AirQualityScore = jsonObj["categories"][15]["data"][0]["float_value"];
            //city.EnvironmentalQuality.CleanlinessScore = jsonObj["categories"][15]["data"][1]["float_value"];
            //city.EnvironmentalQuality.DrinkingWaterQualityScore = jsonObj["categories"][15]["data"][2]["float_value"];
            //city.EnvironmentalQuality.UrbanGreeneryScore = jsonObj["categories"][15]["data"][3]["float_value"];

            ////TravelConnectivity
            //city.TravelConnectivity.AirportHub = jsonObj["categories"][20]["data"][0]["int_value"];
            //city.TravelConnectivity.AirportHubScore = jsonObj["categories"][20]["data"][1]["float_value"];
            //city.TravelConnectivity.IcTrainConnectScore = jsonObj["categories"][20]["data"][2]["float_value"];

            ////HealthCare
            //city.HealthCare.HcExpendScore = jsonObj["categories"][7]["data"][0]["float_value"];
            //city.HealthCare.LifeExpectancyInYears = jsonObj["categories"][7]["data"][1]["float_value"];
            //city.HealthCare.LifeExpectancyScore = jsonObj["categories"][7]["data"][2]["float_value"];
            //city.HealthCare.HcQualityScore = jsonObj["categories"][7]["data"][3]["float_value"];

            ////InternetAcess
            //city.InternetAccess.DownloadSpeed = jsonObj["categories"][13]["data"][0]["float_value"];
            //city.InternetAccess.DownloadScore = jsonObj["categories"][13]["data"][1]["float_value"];
            //city.InternetAccess.UploadSpeed = jsonObj["categories"][13]["data"][2]["float_value"];
            //city.InternetAccess.UploadScore = jsonObj["categories"][13]["data"][3]["float_value"];

            ////Outdoors 
            //city.Outdoors.UrbanAreaElevation = jsonObj["categories"][14]["data"][0]["float_value"];
            //city.Outdoors.PresenceOfHillsInCity = jsonObj["categories"][14]["data"][1]["float_value"];
            //city.Outdoors.PresenceOfMountainsInCity = jsonObj["categories"][14]["data"][2]["float_value"];
            //city.Outdoors.MedianPeakInM = jsonObj["categories"][14]["data"][3]["float_value"];
            //city.Outdoors.Elevation = jsonObj["categories"][14]["data"][4]["float_value"];
            //city.Outdoors.WaterAccess = jsonObj["categories"][14]["data"][5]["float_value"];

            //cityList.Add(city);
        }

        private void addCity_btn_Click(object sender, RoutedEventArgs e)
        {
            AddCity addCityWindow = new AddCity();
            addCityWindow.ShowDialog();

            //Dgd_MainGrid.ItemsSource = testCityList;
            // personen_lb.Items.Refresh()
            Dgd_MainGrid.Items.Refresh();
            //refresh testCityList
        }
    }
}
