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

        }

        //constructor1: only name
        public City(string name)
        {
            _Name = name;
            //_Categories[0].Score.ScoreOutOf10 = scoreinput;
        }


        public string Name { get => _Name; set => _Name = value; }
        public string Url { get => _Url; set => _Url = value; }
        public Category[] Categories { get => _Categories; set => _Categories = value; }
    }
}

