using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace QLifeC_Datatool
{
    public class CsvFile
    {
        private string _Filename;
        private Stream _CSVStream;
        private List<City> _SourceForCsv;
        private bool _ExportResult = false;

        public CsvFile()
        {
            Filename = _Filename;
            CSVStream = _CSVStream;
            SourceForCsv = _SourceForCsv;
        }


        public string Filename { get => _Filename; set => _Filename = value; }

        public Stream CSVStream { get => _CSVStream; set => _CSVStream = value; }
        public List<City> SourceForCsv { get => _SourceForCsv; set => _SourceForCsv = value; }
        public bool ExportResult { get => _ExportResult; set => _ExportResult = value; }
        

        public void WriteToCSV()
        {
            try
            {
                SourceForCsv.Any();
                //Any()-Method to see if the list to be exported is null, exception will be caught
                
                    using StreamWriter exportCSV = new StreamWriter(CSVStream);

                    exportCSV.WriteLine("City_Name, Category_Name, Overall_Category_Score, SubCategory_Label, SubCategory_Score");
                    foreach (City city in SourceForCsv)
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
                    ExportResult = true;
                    MessageBox.Show("Your file can be found here: \n" + Filename, "CSV download complete", MessageBoxButton.OK, MessageBoxImage.Information);
                
                
            }

            catch (Exception ex)
            {
                ExportResult = false;
                MessageBox.Show("Attention! You are trying to export a list that is not initialized. No data will be exported to this csv file! \n Error: " + ex.Message, "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

                

        }
    }
}
