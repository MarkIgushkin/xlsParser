using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xlsParser.Parser.Extensions
{
    public static class ParserExtensions
    {
        /// <param name="row">Номер строки</param>
        /// <param name="column">Номер колонки</param>
        /// <param name="range">Кол-во ячеек которое необходимо прочитать</param>
        /// <summary>Читает строку из excel файла</summary>
        public static List<string> ReadLine(this ExcelWorksheet worksheet, int row, int column, int range)
        {
            var line = new List<string>();
            for (var i = 0; i < range; i++)
            {
                line.Add(worksheet.Cells[row, column++].Text);
            }
            return line;
        }
    }
}