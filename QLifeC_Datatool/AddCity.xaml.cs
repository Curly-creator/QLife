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
            Category tmpCat0 = new Category();
            Category tmpCat1 = new Category();
            Category tmpCat2 = new Category();
            Category tmpCat3 = new Category();
            Category tmpCat4 = new Category();
            Category tmpCat5 = new Category();

            cityToBeAdded = new City(cityName_tb.Text);

            cityToBeAdded.Categories.Add(tmpCat0);
            cityToBeAdded.Categories.Add(tmpCat1);
            cityToBeAdded.Categories.Add(tmpCat2);
            cityToBeAdded.Categories.Add(tmpCat3);
            cityToBeAdded.Categories.Add(tmpCat4);
            cityToBeAdded.Categories.Add(tmpCat5);

            cityToBeAdded.Categories[0].Score.ScoreOutOf10 = double.Parse(col_tb.Text);
            cityToBeAdded.Categories[1].Score.ScoreOutOf10 = double.Parse(h_tb.Text);
            cityToBeAdded.Categories[2].Score.ScoreOutOf10 = double.Parse(ia_tb.Text);
            cityToBeAdded.Categories[3].Score.ScoreOutOf10 = double.Parse(eq_tb.Text);
            cityToBeAdded.Categories[4].Score.ScoreOutOf10 = double.Parse(tc_tb.Text);
            cityToBeAdded.Categories[5].Score.ScoreOutOf10 = double.Parse(o_tb.Text);

            ((MainWindow)Application.Current.MainWindow).cityList.Add(cityToBeAdded); //here the city is manually added to your testCityList

            this.Close();
        }
    }
}
