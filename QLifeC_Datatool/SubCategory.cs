using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class SubCategory
    {
        private double _Value;
        private string _Label;
        private string _Type;

        public SubCategory()
        {

        }
        public SubCategory(string label)
        {
            Label = label;
        }

        public string Label { get => _Label; set => _Label = value; }
        public string Type { get => _Type; set => _Type = value; }
        public double Value { get => _Value; set => _Value = value; }

        public string ToolTip()
        {
            return Label;
        }

        public string GetTooltip()
        {
            string value = Math.Round(Value, 2).ToString(); ;

            return Label + ": " + value + "\n";
        }
    }
}
