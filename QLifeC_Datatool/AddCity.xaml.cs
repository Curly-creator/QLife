using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QLifeC_Datatool
{
    /// <summary>
    /// Interaktionslogik für AddCity.xaml
    /// </summary>
    public partial class AddCity : Window
    {
        public City cityToBeAdded;
        public AddCity()
        {
            InitializeComponent();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddCityToList();
            this.Close();
        }
        private void AddCityToList()
        {
            cityToBeAdded = new City(cityName_tb.Text);
            //zahlen oder sonderzeichen abfangen und dann noch wegen unittests fragen -unittest für addData-Funktion?

            List<Slider> allSliders = new List<Slider> { col_sd, h_sd, ia_sd, eq_sd, tc_sd, o_sd };
            for(int i=0; i<6; i++)
            {
                cityToBeAdded.Categories[i].Score.ScoreOutOf10 = allSliders[i].Value;
            }
            
            //Adding Numbers in Data/Subcategories for Cat[0]/col.
            List<Label> cat0labellist = new List<Label> { col1_lb, col2_lb, col3_lb, col4_lb, col5_lb, col6_lb, col7_lb, col8_lb, col9_lb, col10_lb, col11_lb };
            List<TextBox> cat0tblist = new List<TextBox> { col1_tb, col2_tb, col3_tb, col4_tb, col5_tb, col6_tb, col7_tb, col8_tb, col9_tb, col10_tb, col11_tb };
            for (int i=0; i<11; i++)
            {
                AddSubcategory(0, cat0labellist[i].Content.ToString().Remove(cat0labellist[i].Content.ToString().Length-1), double.Parse(cat0tblist[i].Text));     //so complicated because 'Inflation::' -> 'Inflation:'
            }

            //Adding Numbers in Data/Subcategories for Cat[1]/col.
            List<Label> cat1labellist = new List<Label> {h1_lb, h2_lb, h3_lb, h4_lb };
            List<TextBox> cat1tblist = new List<TextBox> { h1_tb, h2_tb, h3_tb, h4_tb };
            for (int i = 0; i < 4; i++)
            {
                AddSubcategory(1, cat1labellist[i].Content.ToString().Remove(cat1labellist[i].Content.ToString().Length - 1), double.Parse(cat1tblist[i].Text));     //so complicated because 'Inflation::' -> 'Inflation:'
            }
            
            //Adding Numbers in Data/Subcategories for Cat[2]/ia.
            List<Label> cat2labellist = new List<Label> { ia1_lb, ia2_lb, ia3_lb, ia4_lb };
            List<TextBox> cat2tblist = new List<TextBox> { ia1_tb, ia2_tb, ia3_tb, ia4_tb };
            for (int i = 0; i < 4; i++)
            {
                AddSubcategory(2, cat2labellist[i].Content.ToString().Remove(cat2labellist[i].Content.ToString().Length - 1), double.Parse(cat2tblist[i].Text));     //so complicated because 'Inflation::' -> 'Inflation:'
            }
            
            //Adding Numbers in Data/Subcategories for Cat[3]/eq.
            List<Label> cat3labellist = new List<Label> { eq1_lb, eq2_lb, eq3_lb, eq4_lb };
            List<TextBox> cat3tblist = new List<TextBox> {eq1_tb, eq2_tb, eq3_tb, eq4_tb };
            for (int i = 0; i < 4; i++)
            {
                AddSubcategory(3, cat3labellist[i].Content.ToString().Remove(cat3labellist[i].Content.ToString().Length - 1), double.Parse(cat3tblist[i].Text));     //so complicated because 'Inflation::' -> 'Inflation:'
            }
            
            //Adding Numbers in Data/Subcategories for Cat[4]/tc.
            List<Label> cat4labellist = new List<Label> { tc1_lb, tc2_lb, tc3_lb };
            List<TextBox> cat4tblist = new List<TextBox> { tc1_tb, tc2_tb, tc3_tb };
            for (int i = 0; i < 3; i++)
            {
                AddSubcategory(4, cat4labellist[i].Content.ToString().Remove(cat4labellist[i].Content.ToString().Length - 1), double.Parse(cat4tblist[i].Text));     //so complicated because 'Inflation::' -> 'Inflation:'
            }
            
            //Adding Numbers in Data/Subcategories for Cat[5]/o.
            List<Label> cat5labellist = new List<Label> { o1_lb, o2_lb, o3_lb, o4_lb, o5_lb, o6_lb, o7_lb };
            List<TextBox> cat5tblist = new List<TextBox> { o1_tb, o2_tb, o3_tb, o4_tb, o5_tb, o6_tb, o7_tb };
            for (int i = 0; i < 7; i++)
            {
                AddSubcategory(5, cat5labellist[i].Content.ToString().Remove(cat5labellist[i].Content.ToString().Length - 1), double.Parse(cat5tblist[i].Text));     //so complicated because 'Inflation::' -> 'Inflation:'
            }

            ((MainWindow)Application.Current.MainWindow).cityList.Add(cityToBeAdded); //here the city is manually added to your testCityList
        }

        public void AddSubcategory(int indexOfCcat, string label, double value)
        {
            Data tmp_subcat = new Data(label);
            tmp_subcat.NumberValue = value;
            cityToBeAdded.Categories[indexOfCcat].Data.Add(tmp_subcat);
        }

        private void col_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            col_lb.Content = Math.Round(col_sd.Value, 1);
        }

        private void h_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            h_lb.Content = Math.Round(h_sd.Value, 1);
        }

        private void ia_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ia_lb.Content = Math.Round(ia_sd.Value, 1);
        }

        private void eq_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            eq_lb.Content = Math.Round(eq_sd.Value, 1);
        }

        private void tc_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tc_lb.Content = Math.Round(tc_sd.Value, 1);
        }

        private void o_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            o_lb.Content = Math.Round(o_sd.Value, 1);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            col_sd.IsEnabled = false;
            col_sd.Value = 0;
        }
        private void col_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            col_sd.IsEnabled = true;
        }

        private void h_cb_Checked(object sender, RoutedEventArgs e)
        {
            h_sd.IsEnabled = false;
            h_sd.Value = 0;
        }
        private void h_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            h_sd.IsEnabled = true;
        }

        private void ia_cb_Checked(object sender, RoutedEventArgs e)
        {
            ia_sd.IsEnabled = false;
            ia_sd.Value = 0;
        }
        private void ia_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            ia_sd.IsEnabled = true;
        }

        private void eq_cb_Checked(object sender, RoutedEventArgs e)
        {
            eq_sd.IsEnabled = false;
            eq_sd.Value = 0;
        }
        private void eq_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            eq_sd.IsEnabled = true;
        }

        private void tc_cb_Checked(object sender, RoutedEventArgs e)
        {
            tc_sd.IsEnabled = false;
            tc_sd.Value = 0;
        }
        private void tc_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            tc_sd.IsEnabled = true;
        }

        private void o_cb_Checked(object sender, RoutedEventArgs e)
        {
            o_sd.IsEnabled = false;
            o_sd.Value = 0;
        }
        private void o_cb_Unchecked(object sender, RoutedEventArgs e)
        {
            o_sd.IsEnabled = true;
        }

    }
}
