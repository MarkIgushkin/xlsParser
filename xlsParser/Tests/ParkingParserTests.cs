using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Linq;
using xlsParser.Models.Context;
using xlsParser.Parser;

namespace xlsParser.Tests
{
    [TestFixture]
    public class ParkingParserTests
    {
        [Test, Description("Cчитывание данных")]
        [TestCase("ParkingTestData.xlsx", "компания")]
        public void TestCase1(string path, string openingWor)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + path;
            var parser = new ParkingParser(path, openingWor);
            TestHelper.ClearDb(parser.context, false);
            parser.ParseAndSaveToDb();
            Assert.IsTrue(ParkingsCorrect(exprectedParkingsTestCase1, parser.context));
            Assert.IsTrue(UsersCorrect(exprectedUsersTestCase1, parser.context));
            Assert.IsTrue(ParkingToServiceCorrect(exprectedParkingsToServiceTestCase1, parser.context));
        }

        [Test, Description("Обновление записанных в бд данных")]
        [TestCase("ParkingTestData1.xlsx", "компания")]
        public void TestCase2(string path, string openingWor)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + path;
            var parser = new ParkingParser(path, openingWor);
            parser.ParseAndSaveToDb();
            Assert.IsTrue(ParkingsCorrect(exprectedParkingsTestCase2, parser.context));
            Assert.IsTrue(UsersCorrect(exprectedUsersTestCase2, parser.context));
            Assert.IsTrue(ParkingToServiceCorrect(exprectedParkingsToServiceTestCase2, parser.context));
        }

        private bool ParkingsCorrect(string[][] exprected, DataBaseContext context)
        {
            var actualParkings = context.Parkings.OrderBy(x => x.Id).ToList();
            if (actualParkings.Count != exprected.GetLength(0))
                return false;
            for (var i = 0; i < exprected.GetLength(0); i++)
            {
                if (actualParkings[i].Title != exprected[i][0])
                    return false;
                if (actualParkings[i].Inn != exprected[i][1])
                    return false;
                if (actualParkings[i].Address != exprected[i][2])
                    return false;
            }
            return true;
        }

        private bool UsersCorrect(string[][] expected, DataBaseContext context)
        {
            var users = context.Users
                    .Include(x => x.Carrier)
                    .Include(x => x.Parking)
                    .ToList();
            if (users.Where(x => x.Parking != null).Count() != expected.GetLength(0))
                return false;
            for (var i = 0; i < expected.GetLength(0); i++)
            {
                var user = users.FirstOrDefault(x => x.Email == expected[i][0]);
                if (user == null)
                    return false;
                if (user.Carrier != null)
                    return false;
                if (user.Password != expected[i][1])
                    return false;
                if (user.Parking.Inn != expected[i][2])
                    return false;
            }
            return true;
        }

        private bool ParkingToServiceCorrect(string[][] expected, DataBaseContext context)
        {
            var parkingToService = context.ParkingToServices
                .Include(x => x.Service)
                .Include(x => x.Parking)
                .ToList();
            if (parkingToService.Count != expected.GetLength(0))
                return false;
            for (var i = 0; i < expected.GetLength(0); i++)
            {
                parkingToService.FirstOrDefault(x => x.Service.Title.ToLower() == expected[i][0].ToLower() && x.Parking.Inn == expected[i][1]);
                if (parkingToService == null)
                    return false;
            }
            return true;
        }

        //TestCase1
        string[][] exprectedParkingsTestCase1 = new[]
        {
            new[]{ "ООО \"Петр\"", "777/888", "трасса Москва-Спб, 25 км" },
            new[]{ "ООО \"Петр\"", "111/222", "трасса Москва-Спб, 50 км" },
            new[]{ "ООО \"крик\"", "333/444", "Трасса москва -наб.челны" },
        };

        string[][] exprectedUsersTestCase1 = new[]
        {
            new[]{ "st@mail.ru", "123", "777/888"},
            new[]{ "pr@mail.ru", "321", "111/222" },
            new[]{ "jk@mail.ru", "343", "333/444" }
        };

        string[][] exprectedParkingsToServiceTestCase1 = new[]
{
            new[]{ "мойка", "777/888" },
            new[]{ "стоянка", "111/222" },
            new[]{ "Шиномонтаж", "111/222" },
            new[]{ "еда", "111/222" },
            new[]{ "Шиномонтаж", "333/444" }
        };

        //TestCase2
        string[][] exprectedParkingsTestCase2 = new[]
       {
            new[]{ "ООО \"ППетр\"", "777/888", "трасса Москва-Спб, 125 км" },
            new[]{ "ООО \"ППетр\"", "111/222", "трасса Москва-Спб, 150 км" },
            new[]{ "ООО \"ккрик\"", "333/444", "Трасса москва -наб.челны" },
        };

        string[][] exprectedUsersTestCase2 = new[]
        {
            new[]{ "st@mail.ru", "321", "777/888"},
            new[]{ "pr@mail.ru", "123", "111/222" },
            new[]{ "jk@mail.ru", "343", "333/444" }
        };

        string[][] exprectedParkingsToServiceTestCase2 = new[]
        {
            new[]{ "стоянка", "777/888" },
            new[]{ "шиномонтаж", "777/888" },
            new[]{ "еда", "777/888" },
            new[]{ "отель", "777/888" },

            new[]{ "мойка", "111/222" },
            new[]{ "отель", "111/222" },

            new[]{ "мойка", "333/444" },
            new[]{ "стоянка", "333/444" },
            new[]{ "Шиномонтаж", "333/444" },
            new[]{ "отель", "333/444" }
        };
    }

    public class TestHelper
    {
        public static void ClearDb(DataBaseContext context, bool deleteServices = true)
        {
            foreach (var pts in context.ParkingToServices)
                context.ParkingToServices.Remove(pts);
            context.SaveChanges();
            foreach (var cr in context.Carriers)
                context.Carriers.Remove(cr);
            context.SaveChanges();
            foreach (var u in context.Users)
                context.Users.Remove(u);
            context.SaveChanges();
            foreach (var p in context.Parkings)
                context.Parkings.Remove(p);
            context.SaveChanges();
            if (deleteServices)
            {
                foreach (var s in context.Services)
                    context.Services.Remove(s);
                context.SaveChanges();
            }
        }
    }

}