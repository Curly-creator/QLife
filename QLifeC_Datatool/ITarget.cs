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
        CityList cityList { get; set; }
        string FileName { get; set; }
        string FilePath { get; set; }
        string FileExt { get; set; }
        Stream FileStream { get; set; }
        bool MethodStatus { get; set; }
        string StatusNotification { get; set; }
    }
}
