using Microsoft.Win32;
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
using System.Xml;
using System.Xml.Schema;

namespace QLifeC_Datatool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<City> cityList = new List<City>();
        public string[] FileTypeAllowed = { ".xml", ".csv" };

        public MainWindow()
        {
            InitializeComponent();

            //API_GetData();

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

        public void API_GetData()
        {
            API_GetCityList();
            foreach (var city in cityList)
            {
                API_GetCategoryScores(city);
                API_GetCategoryDetails(city);
            }
        }      

        public void API_GetCityList()
        {    
            var url = "https://api.teleport.org/api/urban_areas";

            dynamic jsonObj = API_UrlToJsonObj(url);
           
            var jsonCities = jsonObj["_links"]["ua:item"];
            
            for(int i = 0; i <= 200; i += 10)
            {          
                City city = new City
                {
                    Url = jsonCities[i]["href"],
                    Name = jsonCities[i]["name"]
                };
                cityList.Add(city);
            };
        }

        public void API_GetCategoryScores(City city)
        {
            var url = cityList[cityList.IndexOf(city)].Url + "scores/";

            dynamic jsonObj = API_UrlToJsonObj(url);

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

        public void API_GetCategoryDetails(City city)
        {
            var url = cityList[cityList.IndexOf(city)].Url + "details/";

            dynamic jsonObj = API_UrlToJsonObj(url);

            var jsonCategories = jsonObj["categories"];

            int counter = 0;
       
            foreach (var jsonCategory in jsonCategories)
            {
                for (int i = 0; i < city.Categories.Length; i++){
                    if (jsonCategory["label"] == city.Categories[i].Label)
                    {
                        API_AddSubCategories(city.Categories[i], jsonCategory);
                        counter++;
                        break;
                    }
                }
                if (counter == city.Categories.Length) break;
            }
        }

        public void API_AddSubCategories(Category category, dynamic jsonCategory)
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

        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=net-5.0

            OpenFileDialog openFileDialog = new OpenFileDialog();
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|XML files (*.xml)|*.xml";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    //Getting the path of the selected file.
                    string FilePath = openFileDialog.FileName;
                    string FileExt = System.IO.Path.GetExtension(FilePath).ToLower();

                    CheckFileExtandImport(FileExt, FilePath);

                }
                Dgd_MainGrid.ItemsSource = cityList;
                Dgd_MainGrid.Items.Refresh();
            }
        }

        private void CheckFileExtandImport(string FileExt, string FilePath)
        {
            ITarget target;
            
            //File Type is XML.
            if (FileExt == FileTypeAllowed[0])
            {
                target = new AdapterXML();
                target.CallImportAdapter(FileExt, FilePath);
                cityList = target.cityList;
                MessageBox.Show(target.StatusNotification);
            }
            //File Type is CSV.
            else if (FileExt == FileTypeAllowed[1])
            {
                target = new AdapterCSV();
                target.CallImportAdapter(FileExt, FilePath);
                cityList = target.cityList;
                MessageBox.Show(target.StatusNotification);
            }
            //File Type is invalid.
            else
            {
                MessageBox.Show("This is not a valid file extension");
            }
        }
    }
}
