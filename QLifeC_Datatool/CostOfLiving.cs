using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class CostOfLiving
    {
        //all decimals are Currency Values

        private double _a5kmTaxiRide;
        private double _aCappucchino;
        private double _aBeer;
        private double _aKgApples;
        private double _bread;
        private double _inflationScore;
        private double _lunch;
        private double _mthlyFitnessClubMmbship;
        private double _mthlyPublicTransport;
        private double _movieTicket;
        private double _mealInRestaurant;

        //public CostOfLiving(double inflationScore, decimal aKgApples, decimal bread, decimal aCappucchino, decimal movieTicket, decimal mthlyFitnessClubMmbship, decimal aBeer, decimal mthlyPublicTransport, decimal lunch, decimal a5kmTaxiRide, decimal mealInRestaurant)
        //{ 
        //    InflationScore = inflationScore;
        //    AKgApples = aKgApples;
        //    Bread = bread;
        //    ACappucchino = aCappucchino;
        //    MovieTicket = movieTicket;
        //    MthlyFitnessClubMmbship = mthlyFitnessClubMmbship;
        //    ABeer = aBeer;
        //    MthlyPublicTransport = mthlyPublicTransport;
        //    Lunch = lunch;
        //    A5kmTaxiRide = a5kmTaxiRide;
        //    MealInRestaurant = mealInRestaurant;   
        //}

        public double A5kmTaxiRide { get => _a5kmTaxiRide; set => _a5kmTaxiRide = value; }
        public double ACappucchino { get => _aCappucchino; set => _aCappucchino = value; }
        public double ABeer { get => _aBeer; set => _aBeer = value; }
        public double AKgApples { get => _aKgApples; set => _aKgApples = value; }
        public double Bread { get => _bread; set => _bread = value; }
        public double InflationScore { get => _inflationScore; set => _inflationScore = value; }
        public double Lunch { get => _lunch; set => _lunch = value; }
        public double MthlyFitnessClubMmbship { get => _mthlyFitnessClubMmbship; set => _mthlyFitnessClubMmbship = value; }
        public double MthlyPublicTransport { get => _mthlyPublicTransport; set => _mthlyPublicTransport = value; }
        public double MovieTicket { get => _movieTicket; set => _movieTicket = value; }
        public double MealInRestaurant { get => _mealInRestaurant; set => _mealInRestaurant = value; }
    }

    
}
