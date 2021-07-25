using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QLifeC_Datatool
{
    public class AdapterFileImport : ITarget
    {
        private readonly City _city;

        //Name of the file to be imported.
        private string _FileName;
        public string FileName { get => _FileName; set => _FileName = value; }

        //Extensions allowed for the user to import.
        public string[] FileTypeAllowed { get; set; } = { ".xml", ".csv" };

        //Extension of the file user is trying to upload.
        private string _ImportFilePath;
        public string ImportFilePath { get => _ImportFilePath; set => _ImportFilePath = value; }

        private string _ImportFileExt;
        public string ImportFileExt { get => _ImportFileExt; set => _ImportFileExt = value; }


        //Path where the file will be stored.
        //private string _StorePath;

        public AdapterFileImport()
        {

        }

        public AdapterFileImport(City cityadaptee)
        {
            this._city = cityadaptee;
        }

        public string GetRequest()
        {
            return $"This is '{this._city.GetSpecificRequest()}'";
        }

    }
}
