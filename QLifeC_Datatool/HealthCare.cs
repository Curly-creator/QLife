using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class HealthCare
    {
        //hc = Health Care

        private double _hcExpendScore;
        private double _hcQualityScore;
        private double _lifeExpectancyInYears;
        private double _lifeExpectancyScore;

        //public HealthCare(double hcExpendScore, double hcQualityScore, double lifeExpectancyInYears, double lifeExpectancyScore)
        //{
        //    HcExpendScore = hcExpendScore;
        //    HcQualityScore = hcQualityScore;
        //    LifeExpectancyInYears = lifeExpectancyInYears;
        //    LifeExpectancyScore = lifeExpectancyScore;
        //}

        public double HcExpendScore { get => _hcExpendScore; set => _hcExpendScore = value; }
        public double HcQualityScore { get => _hcQualityScore; set => _hcQualityScore = value; }
        public double LifeExpectancyInYears { get => _lifeExpectancyInYears; set => _lifeExpectancyInYears = value; }
        public double LifeExpectancyScore { get => _lifeExpectancyScore; set => _lifeExpectancyScore = value; }
    }
}
