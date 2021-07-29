using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using System.Windows;

namespace QLifeC_Datatool
{
    public class AdapterCSV : ITarget
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
        //In case of exception MethodStatus returns FALSE.
        private bool _MethodStatus;
        public bool MethodStatus { get => _MethodStatus; set => _MethodStatus = value; }

        //This is the string variable that saved the exception.
        private string _StatusNotification;
        public string StatusNotification { get => _StatusNotification; set => _StatusNotification = value; }

        /// <summary>
        /// The empty constructor to initialize the class.
        /// </summary>
        public AdapterCSV()
        {

        }

        /// <summary>
        /// This is the constructor which will be initialized for EXPORTING a CSV file. It requires a stream and a list to be exported.
        /// </summary>
        /// <param name="qLifeStream"></param>
        /// <param name="ListForExport"></param>
        public AdapterCSV(Stream qLifeStream, CityList ListForExport)
        {
            //the follwoing try catch is only for UnitTest reasons. Actually I am already checking the List in MainWindow Method CheckIfListCanBeExportedAtAll so that the file is not exported at all if null, but I cannot write UnitTest for Methods in MainWindow
            try
            {
                WriteToCSV(qLifeStream, ListForExport);
                MessageBox.Show("Your export was successful.", "CSV export complete", MessageBoxButton.OK, MessageBoxImage.Information);
                MethodStatus = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Attention! You are trying to export a list that is not initialized. No data will be exported to this csv file! \n Error: " + ex.Message, "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MethodStatus = false;
            }
        }

        /// <summary>
        /// This is the constructor which will be initialized for IMPORTING a CSV file. It requires just the file path.
        /// </summary>
        /// <param name="FilePath"></param>
        public AdapterCSV(string FilePath)
        {
            ReadParseCSV(FilePath);
            StatusNotification = "CSV Import successful";
        }

        /// <summary>
        /// turns the data of chosen list into a comma separated value format and parses it via streamwriter into a file
        /// </summary>
        /// <param name="csvstream"></param>
        /// <param name="cityList"></param>
        public void WriteToCSV(Stream csvstream, CityList cityList)
        {
            using StreamWriter exportCSV = new StreamWriter(csvstream);

            //adding headers into the file
            exportCSV.WriteLine("City_Name, Category_Name, Overall_Category_Score, SubCategory_Label, SubCategory_Score");
            foreach (City city in cityList)
            {
                for (int i = 0; i < city.Categories.Length; i++)
                {
                    for (int j = 0; j < city.Categories[i].SubCategories.Count(); j++)
                    {
                        string cityNameForCsv = city.Name.ToString().Replace(",", "");
                        string categoryNameCsv = city.Categories[i].Label;
                        decimal scoreAsDecimal = (decimal)Math.Round(city.Categories[i].Score, 2);
                        string scoreForCsv = scoreAsDecimal.ToString("F2").Replace(",", ".");//*1
                        string subcatLabelCsv = city.Categories[i].SubCategories[j].Label.ToString();
                        string subcatScoreCsv = city.Categories[i].SubCategories[j].Value.ToString("F2").Replace(",", ".");
                        exportCSV.WriteLine(cityNameForCsv + "," + categoryNameCsv + "," + scoreForCsv + "," + subcatLabelCsv + "," + subcatScoreCsv);

                    }
                }
            }
        }

        /// <summary>
        /// This method reads and parses a CSV file into the city list.
        /// </summary>
        /// <param name="Importfilepath"></param>
        public void ReadParseCSV(string Importfilepath)
        {
            try
            {
                //Reading the CSV Line by Line
                string[] csvlines = System.IO.File.ReadAllLines(Importfilepath);

                //Initializing an array to parse the CSV.
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
                cityList = new CityList();

                //Parsing the CSV array into City List.
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

                //If the parse is successful, this variable will be set to TRUE.
                MethodStatus = true;
            }
            catch (Exception ex)
            {
                StatusNotification = "CSV Parse Error: " + ex.Message;

                //If the parse is unsuccessful, this variable will be set to FALSE.
                MethodStatus = false;
            }
        }
    }

}
