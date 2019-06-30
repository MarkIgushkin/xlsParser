﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xlsParser.Parser.Extensions
{
    public static class ParserExtensions
    {
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