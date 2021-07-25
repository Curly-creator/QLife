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

       

        private void btn_Download_Click(object sender, RoutedEventArgs e)
        {
            Stream qLifeStream;
            SaveFileDialog downloadDialog = new SaveFileDialog();

            downloadDialog.InitialDirectory = @"C:\";
            downloadDialog.Filter = "csv files (*.csv)|*.csv|xml files (*.xml)|*.xml";
            downloadDialog.FilterIndex = 2;
            downloadDialog.RestoreDirectory = true;
            downloadDialog.Title = "Download QLifeC file";
            
            if (downloadDialog.ShowDialog() == true)
            {
                if (downloadDialog.FileName.EndsWith("csv") == true)
                {

                    if ((qLifeStream = downloadDialog.OpenFile()) != null)
                    {
                        {
                            WriteToCSV(qLifeStream);
                            MessageBox.Show("Your file can be found here: " + downloadDialog.FileName, "CSV download complete", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                    
                else if (downloadDialog.FileName.EndsWith("xml") == true)
                {
                    if ((qLifeStream = downloadDialog.OpenFile()) != null)
                    {
                        {
                            WriteToXML(qLifeStream);
                            MessageBox.Show("Your file can be found here: " + downloadDialog.FileName, "XML download complete", MessageBoxButton.OK ,MessageBoxImage.Information);
                        }
                    }
                    
                }
            }
        }

        public void WriteToCSV(Stream qLifeCsvStream)
        {
            
            City x = new City();
            int amountCategories = x.Categories.Length;

            using StreamWriter exportCSV = new StreamWriter(qLifeCsvStream);
            //path: C: \Users\ThinkPad T540p\UI Coding\2.Semester Prog 2\QLifeC Datatool App\QLifeC_Datatool\bin\Debug\netcoreapp3.1

            exportCSV.WriteLine("City_Name" + "," + "Category_Name" + "," + "Overall_Category_Score" + "," + "SubCategory_Label" + "," + "SubCategory_Score");

            foreach (City city in cityList)
            {
                string cityNameForCsv = city.Name.ToString().Replace(",", "");
                //exportCSV.WriteLine(cityNameForCsv + ",");


                for (int i = 0; i < amountCategories; i++)
                {
                    string categoryNameCsv = x.Categories[i].Label;
                    decimal scoreAsDecimal = (decimal)Math.Round(city.Categories[i].Score, 2);
                    string scoreForCsv = scoreAsDecimal.ToString("F2").Replace(",", ".");//*1
                    //exportCSV.WriteLine(cityNameForCsv + "," + categoryNameCsv + "," + scoreForCsv + ",");
                    //exportCSV.WriteLine();

                    for (int j = 0; j < city.Categories[i].SubCategories.Count(); j++)
                    {
                        string subcatLabelCsv = city.Categories[i].SubCategories[j].Label.ToString();
                        string subcatScoreCsv = city.Categories[i].SubCategories[j].Value.ToString("F2").Replace(",", ".");
                        exportCSV.WriteLine(cityNameForCsv + "," + categoryNameCsv + "," + scoreForCsv + "," + subcatLabelCsv + "," + subcatScoreCsv);
                    }
                    //exportCSV.WriteLine();
                }
                //exportCSV.WriteLine();
            }
        }

        //LIST OF REFERENCES
        // *1 ---> "F2" for always 2 places after comma or dot
        //https://stackoverflow.com/questions/36619121/convert-string-to-decimal-to-always-have-2-decimal-places

        public void WriteToXML(Stream qLifeXmlStream)
        {
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<City>));

            writer.Serialize(qLifeXmlStream, cityList);
        }

        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=net-5.0

            OpenFileDialog openFileDialog = new OpenFileDialog();
            {
                ITarget target = new AdapterFileImport();

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|XML files (*.xml)|*.xml";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    //Getting the path of the selected file.
                    target.ImportFilePath = openFileDialog.FileName;
                    target.ImportFileExt = System.IO.Path.GetExtension(target.ImportFilePath).ToLower();
                    target.FileName = System.IO.Path.GetFileNameWithoutExtension(target.ImportFilePath);

                    //Read the contents of the file into a stream
                    target.ImportFileStream = openFileDialog.OpenFile();

                    //using (StreamReader reader = new StreamReader(target.ImportFileStream))
                    //{
                    //    var fileContent = reader.ReadToEnd();
                    //}

                    //Checking if the file type matches the allowed file types.
                    //If file extension is .xml.
                    if (target.ImportFileExt == target.FileTypeAllowed[0])
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(target.ImportFileStream);

                        target.ValidateXML(target.ImportFileStream);
                        if (target.Validationstatus == false)
                        {
                            MessageBox.Show(target.ValidationstatusNotification);
                        }
                    }
                    //If file extension is .csv
                    else if (target.ImportFileExt == target.FileTypeAllowed[1])
                    {
                        target.ReadParseCSV(target.ImportFilePath);
                        MessageBox.Show("Import successful.");
                    }
                    else
                    {
                        MessageBox.Show("The selected file type is not allowed. Import failed.");
                    }
                }
                Dgd_MainGrid.ItemsSource = target.cityList;
                Dgd_MainGrid.Items.Refresh();
            }
        }

        private void schema(object sender, ValidationEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
