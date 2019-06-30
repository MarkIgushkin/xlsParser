using NUnit.Framework;
using xlsParser.Parser;
using System.Linq;
using System.Data.Entity;
using System;

namespace xlsParser.Tests
{
    [TestFixture]
    public class ServiceParserTests
    {
        [Test, Description("Корректное считывание данных")]
        [TestCase("ServiceTestData.xlsx", "виды услуг")]
        [TestCase("ServiceTestData1.xlsx", "виды услуг")]
        public void TestCase1(string path, string openingWor)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + path;
            var parser = new ServiceParser(path, openingWor);
            TestHelper.ClearDb(parser.context);
            parser.ParseAndSaveToDb();
            var actual = parser.context.Services.Include(x => x.AccountingType).ToList();
            Assert.AreEqual(7, actual.Count);
            for (var i = 0; i < 7; i++)
            {
                Assert.AreEqual(exprectedTestCase1[i][0], actual[i].Title.ToLower());
                Assert.AreEqual(exprectedTestCase1[i][1], actual[i].AccountingType.Title);
                Assert.AreEqual(exprectedTestCase1[i][2], actual[i].Price?.ToString());
            }
        }

        [Test, Description("Обновление записанных в бд данных")]
        [TestCase("ServiceTestData2.xlsx", "виды услуг")]
        public void TestCase2(string path, string openingWor)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + path;
            var parser = new ServiceParser(path, openingWor);
            parser.ParseAndSaveToDb();
            var actual = parser.context.Services.Include(x => x.AccountingType).ToList();
            Assert.AreEqual(7, actual.Count);
            for (var i = 0; i < 7; i++)
            {
                Assert.AreEqual(expectedTestCase2[i][0], actual[i].Title.ToLower());
                Assert.AreEqual(expectedTestCase2[i][1], actual[i].AccountingType.Title);
                Assert.AreEqual(expectedTestCase2[i][2], actual[i].Price?.ToString());
            }
        }

        string[][] exprectedTestCase1 = new[]
        {
            new[]{"мойка", "факт","100" },
            new[]{"стоянка", "факт","200" },
            new[]{ "душ", "факт","150" },
            new[]{ "прачечная", "деньги", null },
            new[]{ "шиномонтаж", "деньги", null },
            new[]{ "еда", "деньги", null },
            new[]{ "отель", "деньги", null }
        };

        string[][] expectedTestCase2 = new[]
{
            new[]{ "мойка", "факт", "200" },
            new[]{ "стоянка", "деньги", "300" },
            new[]{ "душ", "факт", "400" },
            new[]{ "прачечная", "деньги", "100" },
            new[]{ "шиномонтаж", "деньги","200" },
            new[]{ "еда", "факт", null },
            new[]{ "отель", "деньги", "250" }
        };
        
        //static ServiceTests()
        //{
        //    Database.SetInitializer(new DbInitialiser());
        //}
    }
}