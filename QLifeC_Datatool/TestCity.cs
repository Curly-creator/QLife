using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class TestCity
    {
        private string _Name;
        private string _Url;

        List<Categorie> _Categories;
        
        public TestCity()
        {
            Categories = new List<Categorie>();
        }

        public string Name { get => _Name; set => _Name = value; }
        public string Url { get => _Url; set => _Url = value; }
        internal List<Categorie> Categories { get => _Categories; set => _Categories = value; }
    }
}
