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
    }
}
