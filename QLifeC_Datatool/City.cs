using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class City
    {
        private string _Name; 
        private string _Url;
    
        Category[] _Categories;
        public City()
        {
            Categories = new Category[] {
            new Category("Cost of Living"),
            new Category("Healthcare"),
            new Category("Internet Access"),
            new Category("Environmental Quality"),
            new Category("Travel Connectivity"),
            new Category("Outdoors"),
            };           
        }
        //public City(string name) : base()
        //{
        //    Name = name;
        //}

        public string Name { get => _Name; set => _Name = value; }
        public string Url { get => _Url; set => _Url = value; }
        public Category[] Categories { get => _Categories; set => _Categories = value; }
    }
}

