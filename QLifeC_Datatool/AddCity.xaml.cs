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
        public TestCity cityToBeAdded;
        public AddCity()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cityToBeAdded = new TestCity(cityName_tb.Text);
            ((MainWindow)Application.Current.MainWindow).testCityList.Add(cityToBeAdded); //here the city is manually added to your testCityList

            this.Close();
        }
    }
}
