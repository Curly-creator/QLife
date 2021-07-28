using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class CityNameComparer : IComparer<City>
    {
        private bool _Acending;

        public bool Acending { get => _Acending; set => _Acending = value; }

        int IComparer<City>.Compare(City x, City y)
        {
            City city1 = (City)x;
            City city2 = (City)y;

            if (Acending)
                return city2.Name.CompareTo(city1.Name);
            else 
                return city1.Name.CompareTo(city2.Name);     
        }
    }
}
