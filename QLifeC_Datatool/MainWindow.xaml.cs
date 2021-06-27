using Nancy.Json;
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
        public MainWindow()
        {
            InitializeComponent();

            var cityurl = "https://api.teleport.org/api/urban_areas";
            List<string> citylist = new List<string>();
            WebRequest requestCity = WebRequest.Create(cityurl);
            WebResponse responseCity = requestCity.GetResponse();
            using (Stream dataStreamCity = responseCity.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStreamCity);
                string responseString = reader.ReadToEnd();
                dynamic jsonObj = new JavaScriptSerializer().Deserialize<Object>(responseString);
                var cities = jsonObj["_links"]["ua:item"];
                foreach (var item in cities)
                {
                    string name = item["href"];
                    citylist.Add(name);
                }
            }


            for (int i = 0; i < 20; i++)
            {
                var url = citylist[i] + "details/";
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseString = reader.ReadToEnd();
                    dynamic jsonObj = new JavaScriptSerializer().Deserialize<Object>(responseString);
                    double taxi = jsonObj["categories"][3]["data"][9]["currency_dollar_value"];
                }
            }
        }
    }
}
