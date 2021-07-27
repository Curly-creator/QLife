using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class Category
    {

        List<SubCategory> _SubCategories;
        private string _Label;
        private double _Score;

        public Category(string label)
        {
            Label = label;
            SubCategories = new List<SubCategory>();
        }

        public string Label { get => _Label; set => _Label = value; }
        public List<SubCategory> SubCategories { get => _SubCategories; set => _SubCategories = value; }
        public double Score { get => _Score; set => _Score = value; }

        public override string ToString()
        {
            return Math.Round(Score, 2).ToString(); ;
        }

        public string Tooltip
        {
            get
            {
                string ToolTip = "";

                foreach (var SubCategory in SubCategories)
                {
                    ToolTip += SubCategory.GetTooltip();
                }
                return ToolTip;
            }
        }

        
    }
}

