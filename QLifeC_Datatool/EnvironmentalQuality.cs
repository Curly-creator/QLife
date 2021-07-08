using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class EnvironmentalQuality
    {
        private double _airQualityScore;
        private double _cleanlinessScore;
        private double _drinkingWaterQualityScore;
        private double _urbanGreeneryScore;

        //public EnvironmentalQuality(double airQualityScore, double cleanlinessScore, double drinkingWaterQualityScore, double urbanGreeneryScore)
        //{
        //    AirQualityScore = airQualityScore;
        //    CleanlinessScore = cleanlinessScore;
        //    DrinkingWaterQualityScore = drinkingWaterQualityScore;
        //    UrbanGreeneryScore = urbanGreeneryScore;
        //}

        public double AirQualityScore { get => _airQualityScore; set => _airQualityScore = value; }
        public double CleanlinessScore { get => _cleanlinessScore; set => _cleanlinessScore = value; }
        public double DrinkingWaterQualityScore { get => _drinkingWaterQualityScore; set => _drinkingWaterQualityScore = value; }
        public double UrbanGreeneryScore { get => _urbanGreeneryScore; set => _urbanGreeneryScore = value; }
    }
}
