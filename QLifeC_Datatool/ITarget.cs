using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public interface ITarget
    {
        //These are signature lines. Signature method and signature properties.
        string GetRequest();
        string FileName { get; set; }
        string ImportFilePath { get; set; }
        string ImportFileExt { get; set; }
    }
}
