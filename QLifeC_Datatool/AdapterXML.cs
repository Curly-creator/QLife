using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows;

namespace QLifeC_Datatool
{
    public class AdapterXML : ITarget
    {
        private List<City> _cityList;
        public List<City> cityList { get => _cityList; set => _cityList = value; }

        //private Stream _XMLStream;
        //private List<City> _SourceForXml;

        //Name of the file to be imported.
        private string _FileName;
        public string FileName { get => _FileName; set => _FileName = value; }

        //Extensions allowed for the user to import. This is an array, we can extend it based on other types in future.
        //public string[] FileTypeAllowed { get; set; } = { ".xml", ".csv" };

        //Path of the file user is trying to import.
        private string _FilePath;
        public string FilePath { get => _FilePath; set => _FilePath = value; }

        //Extension of the file user is trying to import.
        private string _FileExt;
        public string FileExt { get => _FileExt; set => _FileExt = value; }

        private Stream _FileStream;
        public Stream FileStream { get => _FileStream; set => _FileStream = value; }

        //Status of a method. This is used for unit tests. If the method is successfully implemented, MethodStatus returns TRUE.
        //In case of exception MethodStatus returns FALSE.
        private bool _MethodStatus;
        public bool MethodStatus { get => _MethodStatus; set => _MethodStatus = value; }

        //This is the string variable that saved the exception.
        private string _StatusNotification;
        public string StatusNotification { get => _StatusNotification; set => _StatusNotification = value; }

        public AdapterXML()
        {

        }

        public void CallExportAdapter(Stream stream, List<City> cityList)
        {
            try
            {
                //the follwoing if else code is only for UnitTest reasons. Actually I am already checking the List in MainWindow Method CheckIfListCanBeExportedAtAll so that the file is not exported at all, but I cannot write UnitTest for Methods in MainWindow
                if (cityList != null)
                {
                WriteToXML(stream, cityList);
                MessageBox.Show("Your export was successful.", "XML export complete", MessageBoxButton.OK, MessageBoxImage.Information);
                MethodStatus = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Attention! You are trying to export a list that is not initialized. No data will be exported to this xml file! \n Error: " + ex.Message, "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MethodStatus = false;
            }
        }

        public void WriteToXML(Stream xmlstream, List<City> cityList)
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<City>));

            writer.Serialize(xmlstream, cityList);
        }

        //ExportResult = true;
        //MessageBox.Show("Your file can be found here: \n" + Filename, "XML download complete", MessageBoxButton.OK, MessageBoxImage.Information);

        //ExportResult = false;
        //MessageBox.Show("Attention! You are trying to export a list that is not initialized. No data will be exported to this xml file! \n Error: " + ex.Message, "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);


        public void CallImportAdapter(string FileExtension, string FilePath)
        {
            //File Extension is .xml
            ValidateXML(FilePath);
            DeserializeXML(FilePath);
            StatusNotification = "XML Validation & Import successful.";
        }

        public void DeserializeXML(string Importfilepath)
        {
            //Initializing Serializer.
            XmlSerializer serializer = new XmlSerializer(typeof(List<City>));

            try
            {
                using (FileStream stream = File.Open(Importfilepath, FileMode.Open))
                {
                    cityList = (List<City>)serializer.Deserialize(stream);
                }
                MethodStatus = true;
            }
            catch (Exception ex)
            {
                StatusNotification = "XML Deserialize Error: " + ex.Message;
                MethodStatus = false;
            }
        }

        public void ValidateXML(string Importfilepath)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.schemas?view=net-5.0

            XmlReaderSettings validationsettings = new XmlReaderSettings();
            validationsettings.Schemas.Add("http://www.w3.org/2001/XMLSchema", "ExportedXML_Test.xsd");
            validationsettings.ValidationType = ValidationType.Schema;
            validationsettings.ValidationEventHandler += new ValidationEventHandler(CityArraySettingsValidationEventHandler);

            //Reading the XML File. XMLreader reads the document one line at a time given the file stream and settings.
            XmlReader reader = XmlReader.Create(Importfilepath, validationsettings);
            try
            {
                while (reader.Read()) { }

                StatusNotification = "Validation successful.";
                MethodStatus = true;
            }
            catch (Exception ex)
            {
                StatusNotification = "Validation Error: " + ex.Message;
                MethodStatus = false;
            }
            finally
            {
                reader.Close();
            }
        }

        static void CityArraySettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("WARNING: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: ");
                Console.WriteLine(e.Message);
            }
        }

    }
}
