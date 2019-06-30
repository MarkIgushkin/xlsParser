using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using xlsParser.Models;

namespace xlsParser.Parser.Extensions
{
    public class VehicleParser : Parser
    {
        public VehicleParser(string path, string openingWord) : base(path, openingWord) { }
        protected override void ProcessLines()
        {
            while (true)
            {
                if (currentVehicleNumber == "")
                    break;
                var user = context.Users.FirstOrDefault(x => x.Email == currentEmail);
                if (user == null || user.Carrier == null)
                    continue;
                var vehicle = context.Vehicles.FirstOrDefault(x => x.VehicleNumber == currentVehicleNumber);
                if (vehicle == null)
                    CreateVehicleForCarrier(user.Carrier);
                else
                    UpdateVehicle(vehicle);
                CurrentRow++;
            }
        }

        private void CreateVehicleForCarrier(Carrier carrier)
        {
            var vehicle = new Vehicle() { Title = "tmp", VehicleNumber = currentVehicleNumber, VehiclePassword = currentPassword };
            vehicle.CarrierId = carrier.Id; 
            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        private void UpdateVehicle(Vehicle vehicle)
        {
            vehicle.VehiclePassword = currentPassword;
        }
        private string currentVehicleNumber
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

        private string currentEmail
        {
            get
            {
                return GetCurrentLine[2].Trim();
            }
        }
    }
}
