using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QLifeC_Datatool
{
    public class CategoryComparer : IComparer<City>
    {
        private int _Index;
        public int Index { get => _Index; set => _Index = value; }

        int IComparer<City>.Compare(City x, City y)
        {
            City city1 = (City)x;
            City city2 = (City)y;

            return city2.Categories[Index].Score.CompareTo(city1.Categories[Index].Score);
        }
    }
}
