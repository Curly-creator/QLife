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
        
        //Method Status for unit test purposes to check if the method was successfully implemented.
        public bool MethodStatus;
        public string ErrorNotification;

        public API_Request test = new API_Request("https://api.teleport.org/api/urban_areas");

        public MainWindow()
        {
            InitializeComponent();
            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        private void btn_Download_Click(object sender, RoutedEventArgs e)
        {
            cityList = test.GetCityData();
            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
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
                    //Getting the path and extension of the selected file to be imported.
                    string FilePath = openFileDialog.FileName;
                    string FileExt = System.IO.Path.GetExtension(FilePath).ToLower();
                    if ((qLifeStream = exportDialog.OpenFile()) != null)
                    {
                        string FilePath = exportDialog.FileName;
                        string FileExt = System.IO.Path.GetExtension(FilePath).ToLower();

                    //Calling the import method to initiate file import.
                    CheckFileExtandImport(FileExt, FilePath);
                }
                //Refreshing the datagrid view with the udpated list after import.
                Dgd_MainGrid.ItemsSource = cityList;
                Dgd_MainGrid.Items.Refresh();
                        StartFileExport(FileExt, qLifeStream, ListToBeExported);
                    }
                }
                
            }
            else if (exportPossible == false)
            {
                MessageBox.Show("Export cancelled.");
            }
        }
        /// <summary>
        /// The method takes type of file (extension) and the path of the file to be imported. Checks if it is the right format and imports.
        /// </summary>
        /// <param name="FileExt"></param>
        /// <param name="FilePath"></param>
        public void CheckFileExtandImport(string FileExt, string FilePath)
            else
            {
                MessageBox.Show("Export not possible. Please contact support at Team_QLifeC_Datatool@HTW-UI-InArbeit.de", "Export failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void StartFileExport(string FileExt, Stream qLifeStream, List<City> ListForExport)
        {
            ITarget target;
            try
            {
                //File Type is XML.
                if (FileExt == FileTypeAllowed[0])
                {
                    target = new AdapterXML();
                    target.CallImportAdapter(FileExt, FilePath);
                    cityList = target.cityList;
                    MessageBox.Show(target.StatusNotification, "Import Window", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //File Type is CSV.
                else if (FileExt == FileTypeAllowed[1])
                {
                    target = new AdapterCSV();
                    target.CallImportAdapter(FileExt, FilePath);
                    cityList = target.cityList;
                    MessageBox.Show(target.StatusNotification, "Import Window", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //File Type is invalid.
                else
                {
                    MessageBox.Show("This is not a valid file extension", "Import Window", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MethodStatus = true;
            }
            catch (Exception ex)
            {
                ErrorNotification = "Error: " + ex.Message;
                MethodStatus = false;
            
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
