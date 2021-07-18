using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class Category
    {
        List<Data> _Data;
        private Score _Score;
        private string _Id;
        private string _Label;

        public Category()
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
                string result = "";
                int i = 0;
                foreach (var item in Data)
                {
                    result += Data[i].ToString();
                    i++;
                }
                return result;
            }
        }
    }
}

