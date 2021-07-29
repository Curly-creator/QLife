using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows;
using System.Linq;

namespace QLifeC_Datatool
{
    public class AdapterXML : ITarget
    {
        private CityList _cityList;
        public CityList cityList { get => _cityList; set => _cityList = value; }

        //Name of the file to be imported.
        private string _FileName;
        public string FileName { get => _FileName; set => _FileName = value; }

        //Path of the file user is trying to import.
        private string _FilePath;
        public string FilePath { get => _FilePath; set => _FilePath = value; }

        //Extension of the file user is trying to import.
        private string _FileExt;
        public string FileExt { get => _FileExt; set => _FileExt = value; }

        //Stream used for export.
        private Stream _FileStream;
        public Stream FileStream { get => _FileStream; set => _FileStream = value; }

        //Status of a method. This is used for unit tests. If the method is successfully implemented, MethodStatus returns TRUE.
        //In case of exception, MethodStatus returns FALSE.
        private bool _MethodStatus;
        public bool MethodStatus { get => _MethodStatus; set => _MethodStatus = value; }

        //This is the string variable that saved the exception.
        private string _StatusNotification;
        public string StatusNotification { get => _StatusNotification; set => _StatusNotification = value; }

        /// <summary>
        /// The empty constructor to initialize the class.
        /// </summary>
        public AdapterXML()
        {

        }

        /// <summary>
        /// This is the constructor which will be initialized for EXPORTING a XML file. It requires a stream and a list to be exported.
        /// </summary>
        /// <param name="qLifeStream"></param>
        /// <param name="ListForExport"></param>
        public AdapterXML(Stream qLifeStream, CityList ListForExport)
        {
            try
            {
                ListForExport.Any(); //could not find any other solution to get an exception in catch block as XML writes even not initialized lists into a file
                WriteToXML(qLifeStream, ListForExport);
                MessageBox.Show("Your export was successful.", "XML export complete", MessageBoxButton.OK, MessageBoxImage.Information);
                MethodStatus = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Attention! You are trying to export a list that is not initialized. No data will be exported to this xml file! \n Error: " + ex.Message, "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MethodStatus = false;
            }
        }

        /// <summary>
        /// This is the constructor which will be initialized for IMPORTING an .xml file. It requires just the file path.
        /// </summary>
        /// <param name="FilePath"></param>
        public AdapterXML(string FilePath)
        {
            //File Extension is .xml
            ValidateXML(FilePath);

            //If ValidateXML Method is successful, MethodStatus will be TRUE.
            if (MethodStatus == true)
            {
                MessageBox.Show(StatusNotification, "Import Window", MessageBoxButton.OK, MessageBoxImage.Information);
                //If validation is successful, deserialization will start.
                DeserializeXML(FilePath);
                if (MethodStatus == true)
                {
                    //Deserialization success.
                    MessageBox.Show(StatusNotification, "Import Window", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (MethodStatus == false)
                {
                    //Deserialization fail.
                    MessageBox.Show(StatusNotification + "\n\nYour file can't be imported.", "Import Window", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //If ValidateXML Method is not successful, the exception error will be caught here as MethodStatus will be FALSE.
            else if (MethodStatus == false)
            {
                MessageBox.Show(StatusNotification + "\n\nYour file can't be imported.", "Import Window", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// takes current list and uses the opened stream to write list data to file via xml Serializer
        /// </summary>
        /// <param name="xmlstream"></param>
        /// <param name="cityList"></param>
        public void WriteToXML(Stream xmlstream, CityList cityList)
        {
            XmlSerializer writer = new XmlSerializer(typeof(CityList));

            writer.Serialize(xmlstream, cityList);
        }

        /// <summary>
        /// This method deserializes an xml file into the citylist. 
        /// </summary>
        /// <param name="Importfilepath"></param>
        public void DeserializeXML(string Importfilepath)
        {
            //Initializing Serializer.
            XmlSerializer serializer = new XmlSerializer(typeof(CityList));

            try
            {
                //Creating file stream to deserialize into the citylist.
                using (FileStream stream = File.Open(Importfilepath, FileMode.Open))
                {
                    cityList = (CityList)serializer.Deserialize(stream);
                }
                StatusNotification = "XML Deserialization successful. Your file will now be imported.";
                MethodStatus = true;
            }
            catch (Exception ex)
            {
                StatusNotification = "XML Deserialize Error: " + ex.Message;
                MethodStatus = false;
            }
        }

        /// <summary>
        /// This method validates an xml file against xsd schema.
        /// </summary>
        /// <param name="Importfilepath"></param>
        public void ValidateXML(string Importfilepath)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.schemas?view=net-5.0

            //Reading the XML File. XMLreader reads the document one line at a time given the file stream and settings.
            XmlReader reader = null;

            try
            {
                //Defining the settings for XSD Schema.
                XmlReaderSettings validationsettings = new XmlReaderSettings();
                validationsettings.Schemas.Add("http://www.w3.org/2001/XMLSchema", "ExportedXML_Test.xsd");
                validationsettings.ValidationType = ValidationType.Schema;

                //This line below is not needed because the following code catches validation errors.
                //validationsettings.ValidationEventHandler += new ValidationEventHandler(CityArraySettingsValidationEventHandler);

                reader = XmlReader.Create(Importfilepath, validationsettings);
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
                if (reader != null) reader.Close();
            }
        }

        ///// <summary>
        ///// If there is an error in the validation settings event handler, this method will be called.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //static void CityArraySettingsValidationEventHandler(object sender, ValidationEventArgs e)
        //{
        //    if (e.Severity == XmlSeverityType.Warning)
        //    {
        //        MessageBox.Show("WARNING: " + e.Message);
        //    }
        //    else if (e.Severity == XmlSeverityType.Error)
        //    {
        //        MessageBox.Show("ERROR: " + e.Message);
        //    }
        //}
    }

}
