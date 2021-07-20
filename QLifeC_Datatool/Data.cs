﻿using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class Data
    {

        private string _Id;
        private double _NumberValue;
        private string _StringValue;
        private string _Label;
        private string _Type;

        public Data()
        {

        }
        public Data(string label)
        {
            Label = label;
        }

        public string Id { get => _Id; set => _Id = value; }
        public string Label { get => _Label; set => _Label = value; }
        public string Type { get => _Type; set => _Type = value; }
        public double NumberValue { get => _NumberValue; set => _NumberValue = value; }
        public string StringValue { get => _StringValue; set => _StringValue = value; }

        public override string ToString()
        {
            string value;

            if (Type == "string" || Type == "url") value = StringValue;
            else value = Math.Round(_NumberValue, 2).ToString();

            return Label + ": " + value + "\n";
        }
    }
}
