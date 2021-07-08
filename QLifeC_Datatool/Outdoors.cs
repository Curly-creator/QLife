using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class Outdoors
    {
        private double _elevation;
        private double _medianPeakInM;
        private double _presenceOfHillsInCity;
        private double _presenceOfMountainsInCity;
        private double _urbanAreaElevation;
        private double _waterAccess;

        public double Elevation { get => _elevation; set => _elevation = value; }
        public double MedianPeakInM { get => _medianPeakInM; set => _medianPeakInM = value; }
        public double PresenceOfHillsInCity { get => _presenceOfHillsInCity; set => _presenceOfHillsInCity = value; }
        public double PresenceOfMountainsInCity { get => _presenceOfMountainsInCity; set => _presenceOfMountainsInCity = value; }
        public double UrbanAreaElevation { get => _urbanAreaElevation; set => _urbanAreaElevation = value; }
        public double WaterAccess { get => _waterAccess; set => _waterAccess = value; }

        //public Outdoors(double elevation, double medianPeakInM, bool presenceOfHillsInCity, bool presenceOfMountainsInCity, double urbanAreaElevation, double waterAccess)
        //{
        //    Elevation = elevation;
        //    MedianPeakInM = medianPeakInM;
        //    PresenceOfHillsInCity = presenceOfHillsInCity;
        //    PresenceOfMountainsInCity = presenceOfMountainsInCity;
        //    UrbanAreaElevation = urbanAreaElevation;
        //    WaterAccess = waterAccess;
        //}

        
    }
}
