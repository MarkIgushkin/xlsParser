using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Linq;
using xlsParser.Parser.Extensions;

namespace xlsParser.Tests
{
    public class VehicleParserTests
    {
        [Test, Description("Корректное считывание данных")]
        [TestCase("VehicleTestData.xlsx", "Номер ТС")]
        public void TestCase1(string path, string openingWor)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + path;
            var parser = new VehicleParser(path, openingWor);
            parser.ParseAndSaveToDb();
        }
    }
}

//Х 638 ТУ 178 	1542	1@mail.ru
//Х 649 ТУ 178 	1560	pavel @mail.ru

//Х 653 ТУ 178 	1578	pavel @mail.ru
//Х 657 ТУ 178 	1596	pavel @mail.ru
//Х 660 ТУ 178 	1614	pavel @mail.ru
//Х 663 ТУ 178 	1632	pavel @mail.ru
//Х 668 ТУ 178 	1650	pavel @mail.ru
//Х 673 ТУ 178 	1668	pavel @mail.ru
//Х 675 ТУ 178 	1686	oleg @mail.ru
//Х 676 ТУ 178 	1704	oleg @mail.ru
//Х 677 ТУ 178 	1722	pavel @mail.ru
//Х 679 ТУ 178 	1740	pavel @mail.ru
