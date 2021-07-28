﻿using System;
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
        //In case of exception, MethodStatus returns FALSE.
        private bool _MethodStatus;
        public bool MethodStatus { get => _MethodStatus; set => _MethodStatus = value; }

        //This is the string variable that saved the exception.
        private string _StatusNotification;
        public string StatusNotification { get => _StatusNotification; set => _StatusNotification = value; }

        public AdapterXML()
        {

        }
        /// <summary>
        /// Method that gets called through the Interface and is responsible for validating and deserializing xml file.
        /// </summary>
        /// <param name="FileExtension"></param>
        /// <param name="FilePath"></param>
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
                //Creating file stream to deserialize into the citylist.
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
            XmlReader reader = null;    

            try
            {
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

        static void CityArraySettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
                if (e.Severity == XmlSeverityType.Warning)
                {
                    MessageBox.Show("WARNING: " + e.Message);
                }
                else if (e.Severity == XmlSeverityType.Error)
                {
                    MessageBox.Show("ERROR: " + e.Message);
                }
        }

    }
}