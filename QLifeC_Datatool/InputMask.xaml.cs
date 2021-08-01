using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QLifeC_Datatool
{
    /// <summary>
    /// Interaktionslogik für AddCity.xaml
    /// </summary>
    public partial class InputMask : Window
    {
        public City cityToBeAdded = new City();
        public List<Slider> allSliders;
        public List<Label> allLabelSliders;

        //Adding Numbers in Data/Subcategories for Cat[0]/col. =Cost of living
        public List<Label> cat0labellist;
        public List<TextBox> cat0tblist;
        //Adding Numbers in Data/Subcategories for Cat[1]/h. =Healthcare
        public List<Label> cat1labellist;
        public List<TextBox> cat1tblist;
        //Adding Numbers in Data/Subcategories for Cat[2]/ia. =Internet Access
        public List<Label> cat2labellist;
        public List<TextBox> cat2tblist;
        //Adding Numbers in Data/Subcategories for Cat[3]/eq. =Enviromentall Quality
        public List<Label> cat3labellist;
        public List<TextBox> cat3tblist;
        //Adding Numbers in Data/Subcategories for Cat[4]/tc. =Travel Connectivity
        public List<Label> cat4labellist;
        public List<TextBox> cat4tblist;
        //Adding Numbers in Data/Subcategories for Cat[5]/o. =Outdoors
        public List<Label> cat5labellist;
        public List<TextBox> cat5tblist;

        public List<List<Label>> catLabelListOfList;
        public List<List<TextBox>> catTextBoxListOfList;

        public List<CheckBox> allCheckBox;

        public bool cityToBeEdit;

        public InputMask()
        {
            InitializeComponent();
            
            cityToBeEdit = false;
            enter_bt.Content = "Add To List";

            allSliders = new List<Slider> { col_sd, h_sd, ia_sd, eq_sd, tc_sd, o_sd };
            allLabelSliders = new List<Label> { col_lb, h_lb, ia_lb, eq_lb, tc_lb, o_lb };

            cat0labellist = new List<Label> { col1_lb, col2_lb, col3_lb, col4_lb, col5_lb, col6_lb, col7_lb, col8_lb, col9_lb, col10_lb, col11_lb };
            cat0tblist = new List<TextBox> { col1_tb, col2_tb, col3_tb, col4_tb, col5_tb, col6_tb, col7_tb, col8_tb, col9_tb, col10_tb, col11_tb };

            cat1labellist = new List<Label> { h1_lb, h2_lb, h3_lb, h4_lb };
            cat1tblist = new List<TextBox> { h1_tb, h2_tb, h3_tb, h4_tb };

            cat2labellist = new List<Label> { ia1_lb, ia2_lb, ia3_lb, ia4_lb };
            cat2tblist = new List<TextBox> { ia1_tb, ia2_tb, ia3_tb, ia4_tb };

            cat3labellist = new List<Label> { eq1_lb, eq2_lb, eq3_lb, eq4_lb };
            cat3tblist = new List<TextBox> { eq1_tb, eq2_tb, eq3_tb, eq4_tb };

            cat4labellist = new List<Label> { tc1_lb, tc2_lb, tc3_lb };
            cat4tblist = new List<TextBox> { tc1_tb, tc2_tb, tc3_tb };

            cat5labellist = new List<Label> { o1_lb, o2_lb, o3_lb, o4_lb, o5_lb, o6_lb, o7_lb };
            cat5tblist = new List<TextBox> { o1_tb, o2_tb, o3_tb, o4_tb, o5_tb, o6_tb, o7_tb };

            catLabelListOfList = new List<List<Label>> { cat0labellist, cat1labellist, cat2labellist, cat3labellist, cat4labellist, cat5labellist };
            catTextBoxListOfList = new List<List<TextBox>> { cat0tblist, cat1tblist, cat2tblist, cat3tblist, cat4tblist, cat5tblist };
            allCheckBox = new List<CheckBox> { col_cb, h_cb, ia_cb, eq_cb, tc_cb, o_cb };
        }

        public InputMask(City city) : this()
        {
            enter_bt.Content = "Save change";
            cityToBeEdit = true;

            cityToBeAdded = city;           

            cityName_tb.Text = city.Name;
            for (int i = 0; i < 6; i++)
            {
                if (Math.Round(city.Categories[i].Score, 1) == 0)
                {
                    allCheckBox[i].IsChecked = true;
                }
                else
                {
                    allCheckBox[i].IsChecked = false;
                    allSliders[i].Value = city.Categories[i].Score;
                    allLabelSliders[i].Content = Math.Round(allSliders[i].Value, 1);
                }
            }
            for (int j = 0; j < catTextBoxListOfList.Count; j++)
            {
                for (int i = 0; i < city.Categories[j].SubCategories.Count; i++)
                {
                    catTextBoxListOfList[j][i].Text = city.Categories[j].SubCategories[i].Value.ToString();
                }
            }
        }     

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;

            //Check Name No 1
            if (!cityToBeEdit)
            {
                for (int i = 0; i < ((MainWindow)Application.Current.MainWindow).cityList.Count; i++)   //mit schleife durch alle namen der bisherigen liste gehen
                {
                    if (((MainWindow)Application.Current.MainWindow).cityList[i].Name == cityName_tb.Text)  //if it is same as in nametextbox -> frage mit result
                    {
                        MessageBoxResult result = MessageBox.Show("This City Name already exists, do you still want to add the city?", "City Name Already Exists", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                break;
                            case MessageBoxResult.No:
                                error = true;
                                break;
                        }
                    }
                }
            }

            //Check Name No 2
            if (CheckIfNameIsEmpty(cityName_tb.Text))
            {
                MessageBox.Show("Please type in a valid city name.");
                error = true;
            }
            //Check Name No 3
            else if (!CheckIfNameIsInTitleCase(cityName_tb.Text))
            {
                MessageBoxResult result = MessageBox.Show("The City Name was in a wrong format, should this automatically be fixed?", "City Name to Title Case", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ConvertToTitleCase(cityName_tb.Text);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                error = true;
            }
            //Check Name No 4
            else if (!CheckIfContainsNoSymbols(cityName_tb.Text)) error = true; //if name contains symbols which are not letters:
            //Check Subcats No 1
            else if (!CheckIfContainsOnlyNumbers()) error = true;
            //Check Subcats No 2
            else if (CheckIfSubcatTextboxesContainInvalidSymbols(cityName_tb.Text)) error = true;
            if(!error)
            {
                if (!cityToBeEdit)
                    cityToBeAdded.Index = cityToBeAdded.GetHashCode();
                cityToBeAdded = CityToList(cityToBeAdded.Index);
                DialogResult = true;
                Close();
            }      
        }

        //3 Checking Methods for Name Textbox:
        public bool CheckIfNameIsEmpty(string cityName)
        {
            if (cityName == "") return true;
            else return false;
        }

        public bool CheckIfContainsNoSymbols(string cityName)
        {
            //accepting: '-' '()' ',' '.' 
           char[] symbols = new char[] { '?', '!', '°', '^', '"', '§', '$', '%', '&', '/', '=', '`', '´', '+', '*', '~', '}', ']', '[', '{', '#', '_', ':', ';', '<', '>', '}' };

           for(int i=0; i<cityName.Length; i++)
           {
                for(int c=0; c<symbols.Length; c++)
                {
                    if (symbols[c] == cityName[i])
                    {                       
                        MessageBox.Show("Oh, your City Name contained a not-valid Symbol: " + cityName[i]);
                        return false;
                    }
                }
           }
           return true;
        }

        public bool CheckIfNameIsInTitleCase(string cityName)
        {
            if (cityName.Contains(". ")) //Bsp: Mt. Cook
            {
                string[] splittedName = cityName.Split(". ");
                foreach (string partOfName in splittedName)
                {
                    if (!CheckIfNameIsInTitleCase(partOfName)) return false;
                }
                return true;
            }
            else if (cityName.Contains(' ')) //Bsp: Bad Neustadt 
            {
                string[] splittedName = cityName.Split(' ');
                foreach (string partOfName in splittedName)
                {
                    if (!CheckIfNameIsInTitleCase(partOfName)) return false;
                    //else return true;
                }
                return true;
            }
            else if (cityName.Contains('-')) //Bsp: Bad-Neustadt
            {
                string[] splittedName = cityName.Split('-');
                foreach (string partOfName in splittedName)
                {
                    if (!CheckIfNameIsInTitleCase(partOfName)) return false;
                }
                return true;
            }
            else if (cityName.Contains("(")) //Bsp: Halle (Saale)
            {
                cityName = cityName.Replace("(", "");
                cityName = cityName.Replace(")", "");
                if (!CheckIfNameIsInTitleCase(cityName)) return false;                
                else return true;
            }
            else
            {
                bool firstLetterIsUp;
                bool otherLettersAreLow = false;
                if (cityName_tb.Text.Length == 1) otherLettersAreLow = true;

                if (char.IsUpper(cityName[0]))
                    firstLetterIsUp = true;
                else firstLetterIsUp = false;

                for (int i = 1; i < cityName.Length; i++)
                {
                    if (char.IsLower(cityName[i])) otherLettersAreLow = true;
                    else
                    {
                        otherLettersAreLow = false;
                        break;
                    }
                }
                if (firstLetterIsUp && otherLettersAreLow) return true;
                else return false;
            }
        }

        public void ConvertToTitleCase(string cityName)
        {
            string nameInTitleCase = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(cityName.ToLower());
            cityName_tb.Text = nameInTitleCase;
            MessageBox.Show("The City Name has been changed to Title Case.");
        }

        //2 Checking Methods for Subcategory Textboxes:
        public bool CheckIfContainsOnlyNumbers()
        {
            for (int j = 0; j < catTextBoxListOfList.Count; j++)
            {
                for (int i = 0; i < catTextBoxListOfList[j].Count; i++)
                {
                    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜabcdefghijklmnopqrstuvwxyzäöüß";
                    for (int k = 0; k < alphabet.Length; k++)
                    {
                        if (catTextBoxListOfList[j][i].Text.Contains(alphabet[k]))
                        {
                            MessageBox.Show("Values for Subcategories can only contain numbers. But it is: " + catTextBoxListOfList[j][i].Text);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool CheckIfSubcatTextboxesContainInvalidSymbols(string cityName)
        {
            //j counts from 0-5 to go through the 6 big Categories =indexOfCat
            for (int j = 0; j < catTextBoxListOfList.Count; j++)
            {
                //.Count is how many subcategories (11,4,4,4,3,7) the current cat has
                //the [i] goes through the 11 subcategories and adds them to the city dataset
                for (int i = 0; i < catTextBoxListOfList[j].Count; i++)
                {
                    if (catTextBoxListOfList[j][i].Text == "")
                    {
                        MessageBox.Show("Nice try, du Schlingel. \nSomething went wrong: Subcategory Textboxes shouldn't \n-be empty.");
                        return true;
                    }

                    if (catTextBoxListOfList[j][i].Text.Contains(",,"))
                    {
                        MessageBox.Show("Nice try, du Schlingel. \nSomething went wrong: Subcategory Textboxes shouldn't \n-contain invalid symbols.");
                        return true;
                    }

                    if (catTextBoxListOfList[j][i].Text.Contains(".."))
                    {
                        MessageBox.Show("Nice try, du Schlingel. \nSomething went wrong: Subcategory Textboxes shouldn't \n-contain invalid symbols.");
                        return true;
                    }

                    if (catTextBoxListOfList[j][i].Text.Contains('.'))
                    {
                        catTextBoxListOfList[j][i].Text = catTextBoxListOfList[j][i].Text.Replace('.', ',');                   
                    }
                }
            }
            return false;
        }

        private City CityToList(int index)
        {
            City AddCity = new City
            {
                Index = index
            };

            AddCity.Name = cityName_tb.Text;
            for (int i = 0; i < 6; i++)
            {
                AddCity.Categories[i].Score = allSliders[i].Value;
            }

            //j counts from 0-5 to go through the 6 big Categories =indexOfCat
            for (int j = 0; j < catTextBoxListOfList.Count; j++)
            {
                //.Count is how many subcategories (11,4,4,4,3,7) the current cat has
                //the [i] goes through the 11 subcategories and adds them to the city dataset
                for (int i = 0; i < catTextBoxListOfList[j].Count; i++)
                {
                    AddSubcategory(AddCity, j, catLabelListOfList[j][i].Content.ToString().Remove(catLabelListOfList[j][i].Content.ToString().Length - 1), double.Parse(catTextBoxListOfList[j][i].Text));     //so complicated because 'Inflation::' -> 'Inflation:'
                }
            }
            return AddCity;

        }

        public void AddSubcategory(City city, int indexOfCcat, string label, double value)
        {
            SubCategory tmp_subcat = new SubCategory(label)
            {
                Value = value
            };

            city.Categories[indexOfCcat].SubCategories.Add(tmp_subcat);
        }

        public void SliderValueChanged(object slider, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider actSlider = (Slider)slider;

            for (int i = 0; i < allSliders.Count; i++)
            {
                if (actSlider == allSliders[i])               
                    allLabelSliders[i].Content =  Math.Round(allSliders[i].Value, 1);   
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < allCheckBox.Count; i++)
            {
                if ((bool)allCheckBox[i].IsChecked)
                {
                    allSliders[i].IsEnabled = false;
                    allSliders[i].Value = 0;
                    break;
                }
                else allSliders[i].IsEnabled = true;
            }
        }       
    }
}
