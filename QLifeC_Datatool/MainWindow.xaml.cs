using Microsoft.Win32;
using System;
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
        //Array for Sort/Filter GUI-Elements
        public Slider[] FilterSliderArray;
        public CheckBox[] FilterCheckBoxArray;
        public Label[] FilterLabelArray;
        public Button[] SortButtonArray;

        //Allowed FileTypes (".xml", ".csv")
        public string[] FileTypeAllowed = { ".xml", ".csv" };

        //List of Cities
        public CityList cityList = new CityList();

        //Stack of Changes
        public ChangeCityStack changeCityStack = new ChangeCityStack();

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

        /// <summary>
        /// This Method gets the values of Sliders[] and returns the values in double[]
        /// </summary>
        /// <param name="ArrayOfSlider"></param>
        /// <returns>FilterValues in double[]</returns>
        public double[] GetFilterValues(Slider[] ArrayOfSlider)
        {
            double[] FilterValues = new double[ArrayOfSlider.Length];
            for (int i = 0; i < ArrayOfSlider.Length; i++)
            {
                FilterValues[i] = ArrayOfSlider[i].Value;
            }
            return FilterValues;
        }

        /// <summary>
        /// This Method gets the states of Checkbox[] and returns the states in bool[]
        /// </summary>
        /// <param name="ArrayOfCheckbox"></param>
        /// <returns>Filterstates in bool[]</returns>
        public bool[] GetFilterState(CheckBox[] ArrayOfCheckbox)
        {
            bool[] FilterStates = new bool[ArrayOfCheckbox.Length];
            for (int i = 0; i < ArrayOfCheckbox.Length; i++)
            {
                FilterStates[i] = (bool)ArrayOfCheckbox[i].IsChecked;
            }
            return FilterStates;
        }

        /// <summary>
        /// Performs the GetCityScore() Method with the actual Number of Cities.
        /// Throws an Exception if Number of Cities is not an Integer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadAPIData_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                cityList.Clear();
                cityList.GetCityScores("https://api.teleport.org/api/urban_areas/", int.Parse(tbx_CityCount.Text));
                Dgd_MainGrid.ItemsSource = cityList;
                Dgd_MainGrid.Items.Refresh();
            }
            catch (FormatException ex)
            {
                MessageBox.Show( ex.Message + " Please insert an integer number");
            }
        }

        /// <summary>
        /// If the text in the Searchbar has changed this Mehtod will perform the SerchByCityname() Method with the actual SearchBar Text.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbx_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dgd_MainGrid.ItemsSource = cityList.SearchByCityName(tbx_SearchBar.Text);
            Dgd_MainGrid.Items.Refresh();
        }

        /// <summary>
        /// If the value of an Slider is changed this method will set the actual value to the FilterLabel[]. 
        /// If the Filter is active it will also perform the FilterByCategoryScore Method with the actual setting.
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="e"></param>
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
                    cityList.FilterByCategoryScore(GetFilterValues(FilterSliderArray), GetFilterState(FilterCheckBoxArray));
                    Dgd_MainGrid.Items.Refresh();
                    break;
                }
            }
        }

        /// <summary>
        /// If the state of a filter is changed this Method will perfom the FilterByCategoryScore Method with the actual setting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FilterStateChanged(object sender, RoutedEventArgs e)
        {
            cityList.FilterByCategoryScore(GetFilterValues(FilterSliderArray), GetFilterState(FilterCheckBoxArray));          
            Dgd_MainGrid.Items.Refresh();
        }

        /// <summary>
        /// Performs FilterReset() and empthys the searchbar. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            FilterReset();
            tbx_SearchBar.Text = "";
            Dgd_MainGrid.ItemsSource = cityList;
            Dgd_MainGrid.Items.Refresh();
        }

        /// <summary>
        /// Sets all FilterSliders to 0 and unchecks all FilterCheckboxes
        /// </summary>
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

        /// <summary>
        /// Performs the SortByCategoryScore() Method for the selected category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Performs the SortByCityName() Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    cityList.UpdateCityList(target.cityList);
                }
                //File Type is CSV.
                else if (FileExt == FileTypeAllowed[1])
                {
                    target = new AdapterCSV(FilePath);
                    cityList.UpdateCityList(target.cityList);
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

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            InputMask addingCityWindow = new InputMask();
            if(addingCityWindow.ShowDialog() == true)
            {
                cityList.AddCity(addingCityWindow.cityToBeAdded);
                AddChangeCity(addingCityWindow.cityToBeAdded, "Undo_Add");
            }
            //refresh CityList im View after adding a city
            Dgd_MainGrid.Items.Refresh();             
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            // check if a city is selected
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
                        cityList.RemoveCity((City)Dgd_MainGrid.SelectedItem);
                        AddChangeCity((City)Dgd_MainGrid.SelectedItem, "Undo_Delete");
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("The city is still here.", "Deleting Chosen City");
                        break;
                }
            }
            // refresh the datagrid (after deleting the selected city)
            Dgd_MainGrid.Items.Refresh();                   
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Dgd_MainGrid.SelectedValue != null)
            {
                City editCity = (City)Dgd_MainGrid.SelectedItem;
                InputMask editCityWindow = new InputMask(editCity);
               
                if(editCityWindow.ShowDialog() == true)
                {
                    AddChangeCity(editCity, "Undo_Edit");
                    cityList.EditCity(editCityWindow.cityToBeAdded, cityList.IndexOf(editCity));
                }
                    
                Dgd_MainGrid.Items.Refresh();
            }
            else
                MessageBox.Show("Please first select a city that you would like to edit");
        }
        public void AddChangeCity(City city, string changeType)
        {
            City changeCity = city;
            changeCity.Changetype = changeType;
            changeCityStack.Push(changeCity);
            cb_undo.Items.Refresh();
            cb_undo.Items.Insert(0, city.Name + " : " + city.Changetype);

            cb_undo.SelectedItem = cb_undo.Items[0];
        }

        private void btn_Undo_Click(object sender, RoutedEventArgs e)
        {
            changeCityStack.cityList.Clear();
            changeCityStack.cityList.AddRange(cityList.Backup);
            cityList.UpdateCityList(changeCityStack.Undo(cb_undo.SelectedIndex));
            for (int i =0; i<= cb_undo.SelectedIndex; i++)
            {
                cb_undo.Items.RemoveAt(0);
            }
            if (cb_undo.Items.Count > 0)
                cb_undo.SelectedItem = cb_undo.Items[0];
            else cb_undo.SelectedItem = -1; ;

            Dgd_MainGrid.Items.Refresh();
        }
    }
}
