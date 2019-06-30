using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Linq;
using xlsParser.Models.Context;
using xlsParser.Parser;
namespace xlsParser.Tests
{
    [TestFixture]
    public class CarrierParserTests
    {
        [Test, Description("Cчитывание данных")]
        [TestCase("CarrierTestData1.xlsx", "Логин компании")]
        public void TestCase1(string path, string openingWor)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + path;
            var parser = new CarrierParser(path, openingWor);
            TestHelper.ClearDb(parser.context, false);
            parser.ParseAndSaveToDb();
            Assert.IsTrue(UsersCorrect(exprectedUsersTestCase1, parser.context));
            Assert.IsTrue(CarriersCorrect(exprectedCarriersTestCase1, parser.context));
        }

        private bool UsersCorrect(string[][] expected, DataBaseContext context)
        {
            var users = context.Users
                    .Include(x => x.Carrier)
                    .Include(x => x.Parking)
                    .ToList();
            if (users.Where(x => x.Carrier != null).Count() != expected.GetLength(0))
                return false;
            for (var i = 0; i < expected.GetLength(0); i++)
            {
                var user = users.FirstOrDefault(x => x.Email == expected[i][0]);
                if (user == null)
                    return false;
                if (user.Parking != null)
                    return false;
                if (user.Password != expected[i][1])
                    return false;
                if (user.Carrier.Inn != expected[i][2])
                    return false;
            }
            return true;
        }

        private bool CarriersCorrect(string[][] exprected, DataBaseContext context)
        {
            var actualCarriers = context.Carriers.ToList();
            if (actualCarriers.Count != exprected.GetLength(0))
                return false;
            for (var i = 0; i < exprected.GetLength(0); i++)
            {
                var carrier = actualCarriers.FirstOrDefault(x => x.Title == exprected[i][0]);
                if (carrier == null)
                    return false;
                if (carrier.Inn != exprected[i][1])
                    return false;
            }
            return true;
        }


        //TestCase1
        string[][] exprectedUsersTestCase1 = new[]
        {
            new[]{ "1@mail.ru", "123", "777/123"},
            new[]{ "pavel@mail.ru", "321", "888/321"},
            new[]{ "oleg@mail.ru", "233", "999/888"}
        };

        string[][] exprectedCarriersTestCase1 = new[]
        {
            new[]{ "ООО \"Пупкин\"", "777/123" },
            new[]{ "ООО \"вето\"", "888/321" },
            new[]{ "ООО \"хрун\"", "999/888" }
        };
    }
}
