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

        /// <summary>
        /// This is the event handler for "Import" button click. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    //Getting the path and extension of the selected file to be imported.
                    string FilePath = openFileDialog.FileName;
                    string FileExt = System.IO.Path.GetExtension(FilePath).ToLower();

                    //Calling the import method to initiate file import.
                    CheckFileExtandImport(FileExt, FilePath);
                }
                //Refreshing the datagrid view with the udpated list after import.
                Dgd_MainGrid.ItemsSource = cityList;
                Dgd_MainGrid.Items.Refresh();
            }
        }
        /// <summary>
        /// The method takes type of file (extension) and the path of the file to be imported. Checks if it is the right format and imports.
        /// </summary>
        /// <param name="FileExt"></param>
        /// <param name="FilePath"></param>
        public void CheckFileExtandImport(string FileExt, string FilePath)
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
            }

        }

    }
}
