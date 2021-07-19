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

        Category COL = new Category();
        Category H = new Category();
        Category IA = new Category();
        Category EQ = new Category();
        Category TC = new Category();
        Category O = new Category();

        public City()
        {
            Categories = new Category[] { COL, H, IA, EQ, TC, O };
        }

        public City(string name) :base()
        {
            Name = name;
            Categories = new Category[] { COL, H, IA, EQ, TC, O }; 
        }


        public string Name { get => _Name; set => _Name = value; }
        public string Url { get => _Url; set => _Url = value; }
        public Category[] Categories { get => _Categories; set => _Categories = value; }
    }
}

