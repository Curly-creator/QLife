using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class City
    {
        private CostOfLiving _costOfLiving;
        private EnvironmentalQuality _environmentalQuality;
        private TravelConnectivity _travelConnectivity;
        private HealthCare _healthCare;
        private InternetAccess _internetAccess;
        private Outdoors _outdoors;


        //getter, setter
        public CostOfLiving CostOfLiving { get => _costOfLiving; set => _costOfLiving = value; }
        public EnvironmentalQuality EnvironmentalQuality { get => _environmentalQuality; set => _environmentalQuality = value; }
        public TravelConnectivity TravelConnectivity { get => _travelConnectivity; set => _travelConnectivity = value; }
        public HealthCare HealthCare { get => _healthCare; set => _healthCare = value; }
        public InternetAccess InternetAccess { get => _internetAccess; set => _internetAccess = value; }
        public Outdoors Outdoors { get => _outdoors; set => _outdoors = value; }


        //constructor
        public City(CostOfLiving costOfLiving, EnvironmentalQuality environmentalQuality, TravelConnectivity travelConnectivity, HealthCare healthCare, InternetAccess internetAccess, Outdoors outdoors)
        {
            CostOfLiving = costOfLiving;
            EnvironmentalQuality = environmentalQuality;
            TravelConnectivity = travelConnectivity;
            HealthCare = healthCare;
            InternetAccess = internetAccess;
            Outdoors = outdoors;
        }
    }
}
