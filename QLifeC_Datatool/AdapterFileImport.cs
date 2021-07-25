using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QLifeC_Datatool
{
    public class AdapterFileImport : ITarget
    {
        City _city;
        private List<City> _cityList;
        public List<City> cityList { get => _cityList; set => _cityList = value; }

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

        private Stream _ImportFileStream;
        public Stream ImportFileStream { get => _ImportFileStream; set => _ImportFileStream = value; }

        private bool _Validationstatus;
        public bool Validationstatus { get => _Validationstatus; set => _Validationstatus = value; }

        private string _ValidationstatusNotification;
        public string ValidationstatusNotification { get => _ValidationstatusNotification; set => _ValidationstatusNotification = value; }
        



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

        public void ReadParseCSV(string Importfilepath)
        {
            string[] csvlines = System.IO.File.ReadAllLines(Importfilepath);

            string[,] csvtable = new string[csvlines.Length - 1, 5];

            for (int i = 1; i < csvlines.Length; i++)
            {
                string[] data = csvlines[i].Split(",");

                for (int y = 0; y < data.Length; y++)
                {
                    csvtable[i - 1, y] = data[y];
                }
            }

            City city = new City();
            cityList = new List<City>();

            for (int i = 0; i < csvtable.GetLength(0); i++)
            {
                if (i != 0)
                {
                    if (csvtable[i, 0] != csvtable[i - 1, 0])
                    {
                        cityList.Add(city);
                        city = new City();
                    }
                }

                city.Name = csvtable[i, 0];

                foreach (var category in city.Categories)
                {
                    if (category.Label == csvtable[i, 1])
                    {
                        category.Score = double.Parse(csvtable[i, 2].Replace(".", ","));
                        SubCategory subcategory = new SubCategory();
                        subcategory.Label = csvtable[i, 3];
                        subcategory.Value = double.Parse(csvtable[i, 4].Replace(".", ","));
                        category.SubCategories.Add(subcategory);
                        break;
                    }
                }
            }
            cityList.Add(city);
        }

        public void DeserializeXML(string Importfilepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<City>));
            
            using (FileStream stream = File.Open(Importfilepath, FileMode.Open))
            {
                cityList = (List<City>)serializer.Deserialize(stream);
            }
        }

        public void ValidateXML(Stream stream)
        {
            //Reading the XML File. XML reader reads the document one line at a time given the file stream and settings.
            XmlReader xmlReader = null;

            try
            {
                //Defining the settings to parse the xml file.
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();

                //Several ways to parse xml. E.g. Well Formed, DTD & XSD. In this case, we validate against XSD Schema.
                xmlReaderSettings.ValidationType = ValidationType.Schema;
                //Finding the schema associated with the XML file.
                xmlReaderSettings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ProcessSchemaLocation;
                xmlReaderSettings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings;
                xmlReaderSettings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandle);

                //Actual validation of the file against the above defined settings.
                xmlReader = XmlReader.Create(stream, xmlReaderSettings);                

                //Iterating over the xml. If there is a validation error in the while loop, it will jump to catch.
                while (xmlReader.Read()) { }
                
                ValidationstatusNotification = "Validation was successful";
                Validationstatus = true;

            } 
            catch (Exception ex)
            {
                ValidationstatusNotification = "Error validating: " + ex.Message;
                Validationstatus = false;
            } 
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }

        }

        private void ValidationEventHandle(object sender, ValidationEventArgs args)
        {
            //If this method is evoked, something is wrong with the XML.
            string error = "Validation Error:" + args.Message;

            throw new Exception(error);
        }

        private void XmlReaderSettings_ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmldocument.validate?view=net-5.0
            string error;
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    error = "Error: {0}" + e.Message;
                    break;
                case XmlSeverityType.Warning:
                    error = "Warning {0}" + e.Message;
                    break;
            }
        }
    }
}
