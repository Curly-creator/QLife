using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class CostOfLiving
    {
        //all decimals are Currency Values

        private decimal _a5kmTaxiRide;
        private decimal _aCappucchino;
        private decimal _aBeer;
        private decimal _aKgApples;
        private decimal _bread;
        private double _inflationScore;
        private decimal _lunch;
        private decimal _mthlyFitnessClubMmbship;
        private decimal _mthlyPublicTransport;
        private decimal _movieTicket;
        private decimal _mealInRestaurant;

        public CostOfLiving(decimal a5kmTaxiRide, decimal aCappucchino, decimal aBeer, decimal aKgApples, decimal bread, double inflationScore, decimal lunch, decimal mthlyFitnessClubMmbship, decimal mthlyPublicTransport, decimal movieTicket, decimal mealInRestaurant)
        {
            A5kmTaxiRide = a5kmTaxiRide;
            ACappucchino = aCappucchino;
            ABeer = aBeer;
            AKgApples = aKgApples;
            Bread = bread;
            InflationScore = inflationScore;
            Lunch = lunch;
            MthlyFitnessClubMmbship = mthlyFitnessClubMmbship;
            MthlyPublicTransport = mthlyPublicTransport;
            MovieTicket = movieTicket;
            MealInRestaurant = mealInRestaurant;
        }

        public decimal A5kmTaxiRide { get => _a5kmTaxiRide; set => _a5kmTaxiRide = value; }
        public decimal ACappucchino { get => _aCappucchino; set => _aCappucchino = value; }
        public decimal ABeer { get => _aBeer; set => _aBeer = value; }
        public decimal AKgApples { get => _aKgApples; set => _aKgApples = value; }
        public decimal Bread { get => _bread; set => _bread = value; }
        public double InflationScore { get => _inflationScore; set => _inflationScore = value; }
        public decimal Lunch { get => _lunch; set => _lunch = value; }
        public decimal MthlyFitnessClubMmbship { get => _mthlyFitnessClubMmbship; set => _mthlyFitnessClubMmbship = value; }
        public decimal MthlyPublicTransport { get => _mthlyPublicTransport; set => _mthlyPublicTransport = value; }
        public decimal MovieTicket { get => _movieTicket; set => _movieTicket = value; }
        public decimal MealInRestaurant { get => _mealInRestaurant; set => _mealInRestaurant = value; }
    }

    
}
