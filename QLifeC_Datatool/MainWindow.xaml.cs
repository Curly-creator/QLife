using Microsoft.Win32;
using Nancy.Json;
using Nancy.ModelBinding.DefaultBodyDeserializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public Stack<City> changeCityStack = new Stack<City>();

        public string[] FileTypeAllowed = { ".xml", ".csv" };

        public API_Request test = new API_Request("https://api.teleport.org/api/urban_areas", 20);

        public double[] FilterValues;
        public MainWindow()
        {
            InitializeComponent();

            FilterSliderArray = new Slider[] { sl_CoLFilter, sl_HCFilter, sl_IAFilter, sl_EQFilter, sl_TCFilter, sl_OFilter };
            FilterCheckBoxArray = new CheckBox[] { cb_CoLFilter, cb_HCFilter, cb_IAFilter, cb_EQFilter, cb_TCFilter, cb_OFilter };
            FilterLabelArray = new Label[] { lbl_CoL, lbl_HC, lbl_IA, lbl_EQ, lbl_TC, lbl_O };
            SortButtonArray = new Button[] { btn_SortCoL, btn_SortHC, btn_SortIA, btn_SortEQ, btn_SortTC, btn_SortO };

            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        public double[] GetFilterValues(Slider[] ArrayOfSlider)
        {
            double[] FilterValues = new double[ArrayOfSlider.Length];
            for (int i = 0; i < ArrayOfSlider.Length; i++)
            {
                FilterValues[i] = ArrayOfSlider[i].Value;
            }
            return FilterValues;
        }

        public bool[] GetFilterStatus(CheckBox[] ArrayOfCheckbox)
        {
            bool[] FilterStatus = new bool[ArrayOfCheckbox.Length];
            for (int i = 0; i < ArrayOfCheckbox.Length; i++)
            {
                FilterStatus[i] = (bool)ArrayOfCheckbox[i].IsChecked;
            }
            return FilterStatus;
        }

        private void btn_LoadAPIData_Click(object sender, RoutedEventArgs e)
        {
            cityList.Clear();
            cityList.GetCityScores("https://api.teleport.org/api/urban_areas/", 20);
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
                    Dgd_MainGrid.Items.Refresh();
                    break;
                }
            }
        }

        public void FilterStatusChanged(object sender, RoutedEventArgs e)
        {
            Dgd_MainGrid.ItemsSource = cityList.FilterByCategoryScore(GetFilterValues(FilterSliderArray), GetFilterStatus(FilterCheckBoxArray));          
            Dgd_MainGrid.Items.Refresh();
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
          
            //cityList.Reset();
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
                    string FileExt = Path.GetExtension(FilePath).ToLower();

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
            //try
            {
                //File Type is XML.
                if (FileExt == FileTypeAllowed[0])
                {
                    target = new AdapterXML(FilePath);
                    cityList = target.cityList;
                }
                //File Type is CSV.
                else if (FileExt == FileTypeAllowed[1])
                {
                    target = new AdapterCSV(FilePath);
                    cityList = target.cityList;
                    MessageBox.Show(target.StatusNotification, "Import Window", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //File Type is invalid.
                else
                {
                    MessageBox.Show("This is not a valid file extension", "Import Window", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Event handler for "Export" Button Click
        /// First: the list to be exported is set to the actual list shown in listview, so user can export what is shown, filtered, etc.
        /// Second: list gets checked in a separate method, only if true a SafeFileDialog opens, a Stream is created and file export starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            CityList ListToBeExported = cityList; 
            //for testing reasons if you want to see my preferred exception handling in action use the two following lists, this is where I want to check list, but could not test this in UnitTest as this has to happen in MainWindow right after user clicks export button
            //CityList ListToBeExported = emptyList;
            //CityList ListToBeExported = nullCityList;

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

                //name of file, extension and storage location gets chosen by user in export dialog
                if (exportDialog.ShowDialog() == true)
                {
                    if ((qLifeStream = exportDialog.OpenFile()) != null)
                    {
                        string FilePath = exportDialog.FileName;
                        string FileExt = Path.GetExtension(FilePath).ToLower();

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

        /// <summary>
        /// checks if a list can be exported, there are 3 possible outcomes: null, true and false
        /// null) if list has not been initialized, export does not start, user gets information
        /// false) if list is empty, user is asked if the export should continue and chooses NO (export then cancelled)
        /// true) if list is empty and user chooses YES to continue the export OR if list is not empty which should be the default case here
        /// </summary>
        /// <param name="ListToBeExported"></param>
        /// <returns></returns>
        public bool? CheckIfListCanBeExportedAtAll(CityList ListToBeExported)
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

        /// <summary>
        /// Method to trigger the actual file export by parsing the list and the opened stream to the Adapter that fits the file extension chosen by user
        /// </summary>
        /// <param name="FileExt"></param>
        /// <param name="qLifeStream"></param>
        /// <param name="ListForExport"></param>
        public void StartFileExport(string FileExt, Stream qLifeStream, CityList ListForExport)
        {
            ITarget target;

            string caseSwitch = FileExt;
            switch (caseSwitch)
            {
                case ".xml":
                    target = new AdapterXML(qLifeStream, ListForExport);
                    break;
                case ".csv":
                    target = new AdapterCSV(qLifeStream, ListForExport);
                    break;
                default:
                    //if file extensions get added to Filter of SafeFileDialog without implementing an adapter for them:
                    MessageBox.Show("There is no adapter for the file extention you chose.");
                    break;
            }
        }

        private void btn_NewEntry_Click(object sender, RoutedEventArgs e)
        {
            InputMask addingCityWindow = new InputMask();
            addingCityWindow.ShowDialog();
            Dgd_MainGrid.Items.Refresh();              //refresh CityList im View
        }

        private void btn_DelEntry_Click(object sender, RoutedEventArgs e)
        {
            if (Dgd_MainGrid.SelectedValue == null)
            {
                MessageBox.Show("Please select a city you want to delete first.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to continue?", "Deleting Chosen City", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        int i = (Dgd_MainGrid.SelectedIndex);
                        cityList.RemoveAt(i); //Lor:todo
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("The city is still here.", "Deleting Chosen City");
                        break;
                }
            }

            Dgd_MainGrid.Items.Refresh();                   // refresh the datagrid (after deleting the selected city)
        }

        private void btn_UpdEntry_Click(object sender, RoutedEventArgs e)
        {
            if (Dgd_MainGrid.SelectedValue != null)
            {

                InputMask editCityWindow = new InputMask(cityList[Dgd_MainGrid.SelectedIndex]);
                editCityWindow.ShowDialog();
                Dgd_MainGrid.Items.Refresh();
            }
            else
                MessageBox.Show("Please first select a city that you would like to edit");
        }

        }
        public void Reverse(int count)
        {
            int i = count;
            while (i >= 0 && i <= count && changeCityStack.Count > 0)
            {
                if (changeCityStack.Peek().Changetype == 3)
                {
                    cityList.Insert(changeCityStack.Peek().Index, changeCityStack.Pop());
                }
                else if (changeCityStack.Peek().Changetype == 2)
                {
                    cityList.RemoveAt(changeCityStack.Pop().Index-1);
                }
                else if (changeCityStack.Peek().Changetype == 1)
                {
                    cityList[changeCityStack.Peek().Index] = changeCityStack.Pop();
                }
                else
                {
                    MessageBox.Show("Undo failed ERROR: unknown changetype");
                }
                cb_undo.Items.RemoveAt(0);
                if (cb_undo.Items.Count != 0)
                {
                    cb_undo.SelectedItem = cb_undo.Items[0];
                }
                else
                {
                    cb_undo.SelectedItem = cb_undo.Items.Count - 1;
                }
                i--;
            }
            Dgd_MainGrid.Items.Refresh();
        }
        public void AddChangeCity(City city, int index, int changeType)
        {
            City changeCity = city;

            changeCity.Index = index;
            changeCity.Changetype = changeType;
            string changeTypeStr;
            if (changeType == 1)
            {
                changeTypeStr = "Edit; ";
            }
            else if (changeType == 2)
            {
                changeTypeStr = "Add; ";
            }
            else if (changeType == 3)
            {
                changeTypeStr = "Delete; ";
            }
            else
            {
                changeTypeStr = "";
            }

            changeCityStack.Push(changeCity);
            cb_undo.Items.Insert(0, changeTypeStr + changeCityStack.Peek().Name);
            cb_undo.SelectedItem = cb_undo.Items[0];
        }

        private void btn_Undo_Click(object sender, RoutedEventArgs e)
        {
            Reverse(cb_undo.SelectedIndex);
        }
    }
}
