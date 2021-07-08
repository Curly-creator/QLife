using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    class Categorie
    {
        List<Data> _Data;
        Score _Score;
        private string _Id;
        private string _Lable;

        public Categorie()
        {
            Data = new List<Data>();       
        }

        public string Id { get => _Id; set => _Id = value; }
        public string Lable { get => _Lable; set => _Lable = value; }
        public Score Score { get => _Score; set => _Score = value; }
        internal List<Data> Data { get => _Data; set => _Data = value; }
    }
}
