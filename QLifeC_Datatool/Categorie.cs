using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class Categorie
    {
        List<Data> _Data;
        Score _Score;
        private string _Id;
        private string _Label;
        private string _Tooltip;
        

        public Categorie()
        {
            Data = new List<Data>();
            Score = new Score();
        }

        public override string ToString()
        { 
            return Math.Round(Score.ScoreOutOf10, 2).ToString(); ;
        }

        public string Id { get => _Id; set => _Id = value; }
        public string Label { get => _Label; set => _Label = value; }
        public Score Score { get => _Score; set => _Score = value; }
        public List<Data> Data { get => _Data; set => _Data = value; }

        public string Tooltip
        {
            get
            {
                return "helloworld";
            }
            //set => _Tooltip = "Helloworld";
            //{
            //    //foreach (var item in Data)
            //    //{
            //    //    _Tooltip += Data.ToString();
            //    //}
            //}
        }
    }
}

