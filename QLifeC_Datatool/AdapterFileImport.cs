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
        //City _city;
        private List<City> _cityList;
        public List<City> cityList { get => _cityList; set => _cityList = value; }

        //Name of the file to be imported.
        private string _FileName;
        public string ImportFileName { get => _FileName; set => _FileName = value; }

        //Extensions allowed for the user to import.
        public string[] FileTypeAllowed { get; set; } = { ".xml", ".csv" };

        //Extension of the file user is trying to upload.
        private string _ImportFilePath;
        public string ImportFilePath { get => _ImportFilePath; set => _ImportFilePath = value; }

        private string _ImportFileExt;
        public string ImportFileExt { get => _ImportFileExt; set => _ImportFileExt = value; }

        private Stream _ImportFileStream;
        public Stream ImportFileStream { get => _ImportFileStream; set => _ImportFileStream = value; }

        private bool _MethodStatus;
        public bool MethodStatus { get => _MethodStatus; set => _MethodStatus = value; }

        private string _StatusNotification;
        public string StatusNotification { get => _StatusNotification; set => _StatusNotification = value; }

        public AdapterFileImport()
        {

        }

        //public AdapterFileImport(City cityadaptee)
        //{
        //    this._city = cityadaptee;
        //}

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
            MethodStatus = true;
            //if (MethodStatus == false) StatusNotification = "CSV Parse Failed";
        }

        public void DeserializeXML(string Importfilepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<City>));
            
            using (FileStream stream = File.Open(Importfilepath, FileMode.Open))
            {
                cityList = (List<City>)serializer.Deserialize(stream);
            }
            MethodStatus = true;
            //if (MethodStatus == false) StatusNotification = "XML Parse Failed";
        }

        public void ValidateXML(string Importfilepath)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.schemas?view=net-5.0
            try
            {
                XmlReaderSettings validationsettings = new XmlReaderSettings();
                validationsettings.Schemas.Add("http://www.w3.org/2001/XMLSchema", "ExportedXML_Test.xsd");
                validationsettings.ValidationType = ValidationType.Schema;
                validationsettings.ValidationEventHandler += new ValidationEventHandler(CityArraySettingsValidationEventHandler);

                //Reading the XML File. XML reader reads the document one line at a time given the file stream and settings.
                XmlReader reader = XmlReader.Create(Importfilepath, validationsettings);

                while (reader.Read()) { }

                StatusNotification = "Validation successful.";
                MethodStatus = true;

                reader.Close();
            }
            catch (Exception ex)
            {
                StatusNotification = "Error validation: " + ex.Message;
                MethodStatus = false;
            }
            finally
            {
                //
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
