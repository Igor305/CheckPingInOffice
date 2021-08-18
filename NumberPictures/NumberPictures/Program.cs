using ExcelLibrary.SpreadSheet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace NumberPictures
{
    class Program
    {
        static void Main(string[] args)
        {
            string oldNamePhoto = "";
            string newNamePhoto = "";

            var workbook = Workbook.Load(@"C:\Users\i.talavyria\Desktop\project\NumberPictures\NumberPictures\Фото\Фото товара 60к - 65373\images.xls");
            var worksheet = workbook.Worksheets[0];
            var cells = worksheet.Cells;

            foreach (var cell in cells)
            {
                if (cell.Left.Left > 0)
                {
                    if (cell.Left.Right == 0)
                    {
                        newNamePhoto = cell.Right.StringValue;
                    }

                    if (cell.Left.Right == 1)
                    {

                        oldNamePhoto = cell.Right.StringValue;

                        string oldPath = $@"C:\Users\i.talavyria\Desktop\project\NumberPictures\NumberPictures\Фото\Фото товара 60к - 65373\{oldNamePhoto}";
                        string newPath = $@"C:\Users\i.talavyria\Desktop\project\NumberPictures\NumberPictures\Фото\Фото товара 60к - 65373\{newNamePhoto}.jpg";
                        FileInfo fileInf = new FileInfo(oldPath);
                        fileInf.MoveTo(newPath);

                    }
                }
            }   
        }
    }
}
