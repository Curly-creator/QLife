using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QLifeC_Datatool
{
    public class ComparerCity : IComparer<Category>
    {
        int IComparer<Category>.Compare(Category x, Category y)
        {
            Category category1 = (Category)x;
            Category category2 = (Category)y;

            return category1.Score.CompareTo(category2.Score);
        }
    }
}
