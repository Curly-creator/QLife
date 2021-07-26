using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;


namespace QLifeC_Datatool
{
    public interface ITarget
    {
        //These are signature lines. Signature method and signature properties. https://refactoring.guru/design-patterns/adapter/csharp/example
        List<City> cityList { get; set; }
        string ImportFileName { get; set; }
        string ImportFilePath { get; set; }
        string ImportFileExt { get; set; }
        Stream ImportFileStream { get; set; }
        bool MethodStatus { get; set; }
        string[] FileTypeAllowed { get; set; }
        string StatusNotification { get; set; }
        void ValidateXML(string Importfilepath);
        void ReadParseCSV(string Importfilepath);
        void DeserializeXML(string Importfilepath);
    }
}
