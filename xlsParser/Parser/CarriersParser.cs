using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xlsParser.Models;
using System.Data.Entity;

namespace xlsParser.Parser
{
    public class CarrierParser : Parser
    {
        public CarrierParser(string path, string openingWord) : base(path, openingWord) { }
        protected override void ProcessLines()
        {
            while (true)
            {
                if (currentEmail == "")
                    break;
                var user = context.Users.FirstOrDefault(x => x.Email == currentEmail);
                if (user == null)
                    CreateNewUserAndCarrier();
                else
                    UpdateUser(user);
                CurrentRow++;
            }
        }

        private void CreateNewUserAndCarrier()
        {
            var carrier = new Carrier() { Title = currentCompanyName, Inn = currentInn };
            var user = new Users() { Email = currentEmail, Carrier = carrier, Password = currentPassword };
            context.Users.Add(user);
            context.SaveChanges();
        }

        private void UpdateUser(Users user)
        {
            user.Password = currentPassword;
            if (user.Carrier != null)
                UpdateCarrier(user.Carrier);
            else
            {
                var carrier = new Carrier() { Title = currentCompanyName, Inn = currentInn };
                user.Carrier = carrier;
                context.SaveChanges();
            }
            RemoveParkingFromUser(user);
        }

        private void UpdateCarrier(Carrier carrier)
        {
            carrier.Title = currentCompanyName;
            carrier.Inn = currentInn;
            context.SaveChanges();
        }

        private void RemoveParkingFromUser(Users user)
        {
            var parking = user.Parking;
            if (parking != null)
            {
                foreach (var u in context.Users.Include(x => x.Parking).Where(x => x.ParkingId == parking.Id))
                    user.Parking = null;
                context.SaveChanges();
                context.Parkings.Remove(parking);
                context.SaveChanges();
            }
        }

        #region
        private string currentEmail
        {
            get
            {
                return GetCurrentLine[0].Trim();
            }
        }

        private string currentPassword
        {
            get
            {
                return GetCurrentLine[1].Trim();
            }
        }

        private string currentCompanyName
        {
            get
            {
                return GetCurrentLine[2].Trim();
            }
        }

        private string currentInn
        {
            get
            {
                return GetCurrentLine[3].Trim();
            }
        }
        #endregion
    }
}