using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class TravelConnectivity
    {
        //ic = InterCity

        private double _airportHub;
        private double _airportHubScore;
        private double _icTrainConnectScore;

        public TravelConnectivity(double airportHub, double airportHubScore, double icTrainConnectScore)
        {
            AirportHub = airportHub;
            AirportHubScore = airportHubScore;
            IcTrainConnectScore = icTrainConnectScore;
        }

        public double AirportHub { get => _airportHub; set => _airportHub = value; }
        public double AirportHubScore { get => _airportHubScore; set => _airportHubScore = value; }
        public double IcTrainConnectScore { get => _icTrainConnectScore; set => _icTrainConnectScore = value; }
    }
}
