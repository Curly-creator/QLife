using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class CityNameComparerDecending : IComparer<City>
    {
        int IComparer<City>.Compare(City x, City y)
        {
            City city1 = (City)x;
            City city2 = (City)y;

            return city1.Name.CompareTo(city2.Name);
        }
    }
}
