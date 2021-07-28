using System;
using System.Windows;
using System.Windows.Controls;


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
        public Button[] SortButtonArray;

        public CityList cityList = new CityList();

        public double[] FilterValues;
        public MainWindow()
        {
            InitializeComponent();            

            FilterSliderArray = new Slider[] { sl_CoLFilter, sl_HCFilter, sl_IAFilter, sl_EQFilter, sl_TCFilter,  sl_OFilter };
            FilterCheckBoxArray = new CheckBox[] { cb_CoLFilter, cb_HCFilter, cb_IAFilter, cb_EQFilter, cb_TCFilter, cb_OFilter };
            FilterLabelArray = new Label[] { lbl_CoL, lbl_HC, lbl_IA, lbl_EQ, lbl_TC, lbl_O };
            SortButtonArray = new Button[] { btn_SortCoL, btn_SortHC, btn_SortIA, btn_SortEQ, btn_SortTC, btn_SortO};

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
            cityList.GetCityScores("https://api.teleport.org/api/urban_areas/");
            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        private void tbx_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dgd_MainGrid.ItemsSource = cityList.SearchByCityName(tbx_SearchBar.Text);
            Dgd_MainGrid.Items.Refresh();
        }

        public void SliderValueChanged(object slider, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider actSlider = (Slider)slider;
           
            for (int i = 0; i < FilterSliderArray.Length; i++)
            {
                if (actSlider == FilterSliderArray[i])
                {
                    FilterLabelArray[i].Content = String.Format("Filtervalue: {0,1:N1}", Math.Round(FilterSliderArray[i].Value, 1));
                }
                if (actSlider.Name == FilterSliderArray[i].Name && (bool)FilterCheckBoxArray[i].IsChecked)
                {
                    cityList.FilterByCategoryScore(GetFilterValues(FilterSliderArray), GetFilterStatus(FilterCheckBoxArray));
                    Dgd_MainGrid.ItemsSource = cityList;
                    Dgd_MainGrid.Items.Refresh();
                    break;
                }
            }  
        }

        public void FilterStatusChanged(object sender, RoutedEventArgs e)
        {
            cityList.FilterByCategoryScore(GetFilterValues(FilterSliderArray), GetFilterStatus(FilterCheckBoxArray));          
            Dgd_MainGrid.Items.Refresh();
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
          
            cityList.Reset();
            FilterReset();
            tbx_SearchBar.Text = "";
            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        private void FilterReset()
        {
            for (int i = 0; i < FilterSliderArray.Length; i++)
            {
                FilterSliderArray[i].Value = 0;
            }

            for (int i = 0; i < FilterCheckBoxArray.Length; i++)
            {
                FilterCheckBoxArray[i].IsChecked = false;
            }
        }      

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < SortButtonArray.Length; i++)
            {
                if (SortButtonArray[i] == sender)
                {
                    cityList.SortByCategoryScore(i);                   
                    break;
                }                
            }
            Dgd_MainGrid.Items.Refresh();
        }

        private void SortCityButton_Click(object sender, RoutedEventArgs e)
        {
            cityList.SortByCityName();
            Dgd_MainGrid.Items.Refresh();
        }
    }
}
