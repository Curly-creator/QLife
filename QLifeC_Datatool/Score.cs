using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class Score
    {
        private string _Color;
        private string _Name;
        private double _ScoreOutOf10;

        public Score()
        {
            
        }

        public string Color { get => _Color; set => _Color = value; }
        public string Name { get => _Name; set => _Name = value; }
        public double ScoreOutOf10 { get => _ScoreOutOf10; set => _ScoreOutOf10 = value; }
    }
}
