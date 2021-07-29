using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    class CityNameComparerAcending : IComparer<City>
    {
        int IComparer<City>.Compare(City x, City y)
        {
            City city1 = (City)x;
            City city2 = (City)y;

            return city2.Name.CompareTo(city1.Name);


            
        }
    }
}
