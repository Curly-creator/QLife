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
            //zahlen oder sonderzeichen abfangen

            cityToBeAdded.Categories[0].Score.ScoreOutOf10 = col_sd.Value; //double.Parse(col_tb.Text);
            //checkbox to be added
            cityToBeAdded.Categories[1].Score.ScoreOutOf10 = h_sd.Value;
            cityToBeAdded.Categories[2].Score.ScoreOutOf10 = ia_sd.Value;
            cityToBeAdded.Categories[3].Score.ScoreOutOf10 = eq_sd.Value;
            cityToBeAdded.Categories[4].Score.ScoreOutOf10 = tc_sd.Value;
            cityToBeAdded.Categories[5].Score.ScoreOutOf10 = o_sd.Value;

            ((MainWindow)Application.Current.MainWindow).cityList.Add(cityToBeAdded); //here the city is manually added to your testCityList
        //TODO: unterkategorien auch werte hinzufügen
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

        private void h_cb_Checked(object sender, RoutedEventArgs e)
        {
            h_sd.IsEnabled = false;
            h_sd.Value = 0;
        }

        private void ia_cb_Checked(object sender, RoutedEventArgs e)
        {
            ia_sd.IsEnabled = false;
            ia_sd.Value = 0;
        }

        private void eq_cb_Checked(object sender, RoutedEventArgs e)
        {
            eq_sd.IsEnabled = false;
            eq_sd.Value = 0;
        }

        private void tc_cb_Checked(object sender, RoutedEventArgs e)
        {
            tc_sd.IsEnabled = false;
            tc_sd.Value = 0;
        }
        private void o_cb_Checked(object sender, RoutedEventArgs e)
        {
            o_sd.IsEnabled = false;
            o_sd.Value = 0;
        }
    }
}
