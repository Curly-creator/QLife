using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class SubCategory
    {
        private double _Value;
        private string _Lable;
        private string _Type;

        public SubCategory()
        {

        }

        public string Label { get => _Lable; set => _Lable = value; }
        public string Type { get => _Type; set => _Type = value; }
        public double Value { get => _Value; set => _Value = value; }

        public string ToolTip()
        {
            return Label + ": " + Math.Round(Value, 2).ToString() + "\n";
        }
    }
}
