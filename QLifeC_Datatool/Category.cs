using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class Category
    {
        List<SubCategory> _SubCategory;
        Score _Score;
        private string _Id;
        private string _Label;
        
        

        public Category()
        {
            SubCategory = new List<SubCategory>();
            Score = new Score();
        }

        public override string ToString()
        { 
            return Math.Round(Score.ScoreOutOf10, 2).ToString(); ;
        }

        public string Id { get => _Id; set => _Id = value; }
        public string Label { get => _Label; set => _Label = value; }
        public Score Score { get => _Score; set => _Score = value; }
        public List<SubCategory> SubCategory { get => _SubCategory; set => _SubCategory = value; }

        public string Tooltip
        {
            get
            {
                string result = "";
                int i = 0;
                foreach (var item in SubCategory)
                {
                    result += SubCategory[i].GetTooltip();
                    i++;
                }
                return result;
            }
        }
    }
}

