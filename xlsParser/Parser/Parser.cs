using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xlsParser.Models.Context;
using xlsParser.Parser.Extensions;
using xlsParser.Tests;

namespace xlsParser.Parser
{
    public abstract class Parser
    {
        public DataBaseContext context = new DataBaseContext();

        public readonly string OpeningWord;

        public readonly string FilePath;
        protected int CurrentRow { get; set; } = 1;
        protected ExcelWorksheet Worksheet { get; set; }
        protected List<string> HeaderOfTheTable { get; set; }
        protected int HeaderLength { get { return HeaderOfTheTable.Count; } }

        public Parser(string path, string openingWord)
        {
            this.FilePath = path;
            this.OpeningWord = openingWord;
            this.HeaderOfTheTable = new List<string>();
        }
        public void ParseAndSaveToDb()
        {
            using (var fs = System.IO.File.OpenRead(FilePath))
            {
                using (var excelPackage = new ExcelPackage(fs))
                {
                    var workBook = excelPackage.Workbook;
                    this.Worksheet = workBook.Worksheets.First();
                    SetRowAtStartPosition(OpeningWord);
                    SetHeaderOfTheTable();
                    CurrentRow++;
                    ProcessLines();
                }
            }
        }
        protected void SetRowAtStartPosition(string startingWord)
        {
            while (Worksheet.Cells[CurrentRow, 1].Text.ToLower().Trim() != startingWord.ToLower().Trim())
                CurrentRow++;
        }

        protected void SetHeaderOfTheTable()
        {
            var index = 1;
            while (true)
            {
                var currentCell = Worksheet.Cells[CurrentRow, index++].Text;
                if (currentCell == "")
                    break;
                HeaderOfTheTable.Add(currentCell);
            }
        }

        protected abstract void ProcessLines();

        protected List<string> GetCurrentLine
        {
            get
            {
                return Worksheet.ReadLine(CurrentRow, 1, HeaderLength);
            }
        }
    }
}