using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Check
{
    public class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();

            p.getCheckList();
        }

        public void getCheckList()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excel = new ExcelPackage())
            {

                excel.Workbook.Worksheets.Add("CheckList");

                List<string[]> checklist = new List<string[]>();


                var text = File.ReadAllLines("325_2021-04-06_16-09-05-766 .log", Encoding.UTF8);

                string? date = "", prodAdd = "", docId = "";

                foreach (string str in text)
                {
                    if ((str.Contains("DocID")) || (str.Contains("ProdAdd")))
                    {

                        if (str.Contains("DocID"))
                        {
                            docId = str.Substring(91);
                        }
                        if (str.Contains("ProdAdd"))
                        {
                            date = str.Substring(0, 19);
                            prodAdd = str.Substring(29);

                        }

                        string[] check = new string[3];

                        check[0] = date;
                        check[1] = prodAdd;
                        check[2] = docId;
                        checklist.Add(check);
                    }
                }
                Console.WriteLine(checklist);

                foreach (string[] ch in checklist)
                {

                    foreach (var c in ch)
                    {
                        Console.WriteLine(c);
                    }
                }

                int x = 1;
                foreach (string[] ch in checklist)
                {

                    var headerRow = new List<string[]>()
                    {
                        ch
                    };

                    string headerRange = $"A{x}:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + x;

                    var worksheet = excel.Workbook.Worksheets["CheckList"];

                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    x++;
                }

                FileInfo excelFile = new FileInfo("Check.xlsx");
                excel.SaveAs(excelFile);
            }
        }
    }
}
