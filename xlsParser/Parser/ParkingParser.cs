using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using xlsParser.Models;
using xlsParser.Models.Context;
using xlsParser.Parser.Extensions;

namespace xlsParser.Parser
{
    public class ParkingParser : Parser
    {
        #region
        const int serviceStartsAtIndex = 5;

        int numberOfServices { get { return HeaderLength - serviceStartsAtIndex; } }

        public ParkingParser(string filePath, string openingWord) : base(filePath, openingWord) { }

        #endregion

        protected override void ProcessLines()
        {
            while (true)
            {
                if (currentCompanyName == "")
                    break;
                var user = context.Users.FirstOrDefault(x => x.Email == currentEmail);
                if (user == null)
                    CreateNewUserAndParking();
                else
                    UpdateUser(user);
                CurrentRow++;
            }
        }

        private void CreateNewUserAndParking()
        {
            var parking = new Parking() { Title = currentCompanyName, Inn = currentInn, Address = currentAddress };
            var user = new Users() { Email = currentEmail, Parking = parking, Password = currentPassword };
            context.Users.Add(user);
            context.SaveChanges();
            SetServicesToParking(user.Parking);
        }

        private void UpdateUser(Users user)
        {
            user.Password = currentPassword;
            if (user.Parking != null)
                UpdateParking(user.Parking);
            else
            {
                var parking = new Parking() { Title = currentCompanyName, Inn = currentInn, Address = currentAddress };
                user.Parking = parking;
                context.SaveChanges();
            }
            RemoveCarrierFromUser(user);
            SetServicesToParking(user.Parking);
        }

        private void UpdateParking(Parking parking)
        {
            parking.Title = currentCompanyName;
            parking.Inn = currentInn;
            parking.Address = currentAddress;
            context.SaveChanges();
        }

        private void SetServicesToParking(Parking parking)
        {
            for (var i = 0; i < numberOfServices; i++)
            {
                var serviceName = HeaderOfTheTable[serviceStartsAtIndex + i].Trim();
                var serviceValue = currentServices[i];
                var service = context.Services.FirstOrDefault(c => c.Title.ToLower() == serviceName.ToLower());

                if (service != null)
                {
                    var parkingToService = context.ParkingToServices.FirstOrDefault(c => c.ParkingId == parking.Id && c.ServiceId == service.Id);
                    if (serviceValue == "1" && parkingToService == null)
                    {
                        context.ParkingToServices.Add(
                            new ParkingToService()
                            {
                                ParkingId = parking.Id,
                                ServiceId = service.Id
                            });
                    }
                    else if (serviceValue == "0" && parkingToService != null)
                    {
                        context.ParkingToServices.Remove(parkingToService);
                    }
                }
                context.SaveChanges();
            }
        }

        private void RemoveCarrierFromUser(Users user)
        {
            var carrier = user.Carrier;
            if (carrier != null)
            {
                foreach (var u in context.Users.Include(x => x.Carrier).Where(x => x.CarrierId == carrier.Id))
                    user.CarrierId = null;
                context.SaveChanges();
                context.Carriers.Remove(carrier);
                context.SaveChanges();
            }
        }

        #region
        private string currentCompanyName
        {
            get
            {
                return GetCurrentLine[0].Trim();
            }
        }

        private string currentInn
        {
            get
            {
                return GetCurrentLine[1].Trim();
            }
        }

        private string currentAddress
        {
            get
            {
                return GetCurrentLine[2].Trim();
            }
        }

        private string currentEmail
        {
            get
            {
                return GetCurrentLine[3].Trim();
            }
        }

        private string currentPassword
        {
            get
            {
                return GetCurrentLine[4].Trim();
            }
        }

        private List<string> currentServices
        {
            get
            {
                return GetCurrentLine.GetRange(serviceStartsAtIndex, numberOfServices);
            }
        }
        #endregion
    }
}