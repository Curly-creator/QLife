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
        public Slider[] FilterSliderArray;
        public CheckBox[] FilterCheckBoxArray;
        public Label[] FilterLabelArray;

        public CityList cityList = new CityList();

        public double[] FilterValues;
        public MainWindow()
        {
            InitializeComponent();
            
            FilterSliderArray = new Slider[] { sl_CoLFilter, sl_HCFilter, sl_IAFilter, sl_EQFilter, sl_TCFilter,  sl_OFilter };
            FilterCheckBoxArray = new CheckBox[] { cb_CoLFilter, cb_HCFilter, cb_IAFilter, cb_EQFilter, cb_TCFilter, cb_OFilter };
            FilterLabelArray = new Label[] { lbl_CoL, lbl_HC, lbl_IA, lbl_EQ, lbl_TC, lbl_O };

            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        public double[] GetFilterValues (Slider[] ArrayOfSlider)
        {
            double[] FilterValues = new double[ArrayOfSlider.Length];
            for (int i = 0; i < ArrayOfSlider.Length; i++)
            {
                FilterValues[i] = ArrayOfSlider[i].Value;
            }
            return FilterValues;
        }

        public bool[] GetFilterStatus (CheckBox[] ArrayOfCheckbox)
        {
            bool[] FilterStatus = new bool[ArrayOfCheckbox.Length];
            for (int i = 0; i < ArrayOfCheckbox.Length; i++)
            {
                FilterStatus[i] = (bool)ArrayOfCheckbox[i].IsChecked;
            }
            return FilterStatus;
        }

        private void btn_Download_Click(object sender, RoutedEventArgs e)
        {
            cityList = cityList.GetCityData("https://api.teleport.org/api/urban_areas/");
            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        private void tbx_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dgd_MainGrid.ItemsSource = cityList.SearchCity(tbx_SearchBar.Text);
            Dgd_MainGrid.Items.Refresh();
        }

        private void btn_Filter_Click(object sender, RoutedEventArgs e)
        {
            Dgd_MainGrid.ItemsSource = cityList.Filter(GetFilterValues(FilterSliderArray), GetFilterStatus(FilterCheckBoxArray));
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
