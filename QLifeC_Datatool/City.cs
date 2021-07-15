using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class City
    {
        private string _Name;
        private string _Url;
        
        List<Category> _Categories;
        
        public City()
        {
            Categories = new List<Category>();        
        }
       
        public string Name { get => _Name; set => _Name = value; }
        public string Url { get => _Url; set => _Url = value; }
        public List<Category> Categories { get => _Categories; set => _Categories = value; }

    }
}

