using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace QLifeC_Datatool
{
    public class XmlFile
    {
        private string _Filename;
        private Stream _XMLStream;
        private List<City> _SourceForXml;
        private bool _ExportResult = false;

        public XmlFile()
        {
            Filename = _Filename;
            XMLStream = _XMLStream;
            SourceForXml = _SourceForXml;
        }

        public string Filename { get => _Filename; set => _Filename = value; }
        public Stream XMLStream { get => _XMLStream; set => _XMLStream = value; }
        public List<City> SourceForXml{ get => _SourceForXml; set => _SourceForXml = value; }
        public bool ExportResult { get => _ExportResult; set => _ExportResult = value; }

        public void WriteToXML()
        {
            
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<City>));

            try
            {
                SourceForXml.Any();
                writer.Serialize(XMLStream, SourceForXml);
                ExportResult = true;
                MessageBox.Show("Your file can be found here: \n" + Filename, "XML download complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ExportResult = false;
                MessageBox.Show("Attention! You are trying to export a list that is not initialized. No data will be exported to this xml file! \n Error: " + ex.Message, "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
