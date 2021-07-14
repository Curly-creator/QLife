using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class CategorieID
    {
        string[] _ID;
        string[] _Name;

        public CategorieID()
        {
            ID = new string[] { "COST-OF-LIVING", "HEALTHCARE", "NETWORK", "POLLUTION", "TRAVEL-CONNECTIVITY", "OUTDOORS"};
            Name = new string[] { "Cost of Living", "Healthcare", "Internet Access", "Environmental Quality", "Travel Connectivity", "Outdoors" };
        }
       
        public string[] ID { get => _ID; set => _ID = value; }
        public string[] Name { get => _Name; set => _Name = value; }
    }
}
