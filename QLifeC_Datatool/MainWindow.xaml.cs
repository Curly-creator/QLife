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



namespace QLifeC_Datatool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<City> cityList = new List<City>();
        public List<City> nullCityList;
        public List<City> emptyList = new List<City>();
        public string[] FileTypeAllowed = { ".xml", ".csv" };


        public MainWindow()
        {
            InitializeComponent();

            API_GetData();

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

            for (int i = 0; i <= 200; i += 40)
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
                for (int i = 0; i < city.Categories.Length; i++)
                {
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



        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            ////List<City> ListToBeExported = cityList;
            List<City> ListToBeExported = emptyList;
            //List<City> ListToBeExported = nullCityList;

            bool? exportPossible = CheckIfListCanBeExportedAtAll(ListToBeExported);

            if (exportPossible == true)
            {
                Stream qLifeStream;
                SaveFileDialog exportDialog = new SaveFileDialog
                {
                    InitialDirectory = @"C:\",
                    Filter = "csv files (*.csv)|*.csv|xml files (*.xml)|*.xml",
                    FilterIndex = 2,
                    RestoreDirectory = true,
                    Title = "Export QLifeC file"
                };

                if (exportDialog.ShowDialog() == true)
                {
                    if ((qLifeStream = exportDialog.OpenFile()) != null)
                    {
                        string FilePath = exportDialog.FileName;
                        string FileExt = System.IO.Path.GetExtension(FilePath).ToLower();

                        StartFileExport(FileExt, qLifeStream, ListToBeExported);
                    }
                }
                
            }
            else if (exportPossible == false)
            {
                MessageBox.Show("Export cancelled.");
            }
            else
            {
                MessageBox.Show("Export not possible. Please contact support at Team_QLifeC_Datatool@HTW-UI-InArbeit.de", "Export failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void StartFileExport(string FileExt, Stream qLifeStream, List<City> ListForExport)
        {
            ITarget target;
            
            string caseSwitch = FileExt;
            switch (caseSwitch)
            {
                case ".xml":
                    target = new AdapterXML();
                    target.CallExportAdapter(qLifeStream, ListForExport);
                    break;
                case ".csv":
                    target = new AdapterCSV();
                    target.CallExportAdapter(qLifeStream, ListForExport);
                    break;
                default:
                    MessageBox.Show("Please choose correct file extension");
                    break;
            }
        }

        public bool? CheckIfListCanBeExportedAtAll(List<City> ListToBeExported)
        {
            if (ListToBeExported == null)
            {
                return null;
            }
            else if (ListToBeExported.Count() == 0)
            {
                MessageBoxResult result = MessageBox.Show("No data in this list. Do you want to proceed and export an empty list?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    return true;
                }
                else return false;

            }
            else return true;
        }
    }
}







    //public void WriteToCSV(Stream qLifeCsvStream)
    //{
    //    using StreamWriter exportCSV = new StreamWriter(qLifeCsvStream);
    //    try
    //    {
    //        exportCSV.WriteLine("City_Name, Category_Name, Overall_Category_Score, SubCategory_Label, SubCategory_Score");

    //        foreach (City city in cityList)
    //        {
    //            for (int i = 0; i < city.Categories.Length; i++)
    //            {
    //                for (int j = 0; j < city.Categories[i].SubCategories.Count(); j++)
    //                {
    //                    string cityNameForCsv = city.Name.ToString().Replace(",", "");
    //                    string categoryNameCsv = city.Categories[i].Label;
    //                    decimal scoreAsDecimal = (decimal)Math.Round(city.Categories[i].Score, 2);
    //                    string scoreForCsv = scoreAsDecimal.ToString("F2").Replace(",", ".");//*1
    //                    string subcatLabelCsv = city.Categories[i].SubCategories[j].Label.ToString();
    //                    string subcatScoreCsv = city.Categories[i].SubCategories[j].Value.ToString("F2").Replace(",", ".");
    //                    exportCSV.WriteLine(cityNameForCsv + "," + categoryNameCsv + "," + scoreForCsv + "," + subcatLabelCsv + "," + subcatScoreCsv);
    //                }
    //            }
    //        }
    //        result = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show("Error CSV export" + ex.Message);
    //        result = false;

    //    }

    //}

    //LIST OF REFERENCES
    // *1 ---> "F2" for always 2 places after comma or dot
    //https://stackoverflow.com/questions/36619121/convert-string-to-decimal-to-always-have-2-decimal-places

    //public void WriteToXML(Stream qLifeXmlStream)
    //{
    //    System.Xml.Serialization.XmlSerializer writer =
    //    new System.Xml.Serialization.XmlSerializer(typeof(List<City>));

    //    writer.Serialize(qLifeXmlStream, cityList);
    //}



