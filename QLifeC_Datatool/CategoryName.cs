using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class CategoryName
    {       
        string[] _Name;

        public CategoryName()
        {          
            Name = new string[] { "Cost of Living", "Healthcare", "Internet Access", "Environmental Quality", "Travel Connectivity", "Outdoors" };
        }
         
        public string[] Name { get => _Name; set => _Name = value; }
    }
}
