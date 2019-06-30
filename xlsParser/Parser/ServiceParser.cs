using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xlsParser.Models;

namespace xlsParser.Parser
{
    public class ServiceParser : Parser
    {
        public ServiceParser(string filePath, string openingWord) : base(filePath, openingWord) { }

        protected override void ProcessLines()
        {
            while (true)
            {
                if (currentServiceName == "")
                    break;
                var service = context.Services.FirstOrDefault(c => c.Title.ToLower() == currentServiceName.ToLower());
                if (service == null)
                    CreateService();
                else
                    UpdateService(service);
                CurrentRow++;
            }
        }

        private void CreateService()
        {
            var service = new Service();
            context.Services.Add(service);
            UpdateService(service);
        }

        private void UpdateService(Service service)
        {
            var accountingType = context.AccountingTypes.FirstOrDefault(x => x.Title.ToLower() == currentAccountingType.ToLower());
            decimal price;
            if (accountingType != null)
            {
                service.Title = currentServiceName;
                service.AccountingType = accountingType;
                if (decimal.TryParse(currentPrice, out price))
                    service.Price = price;
                context.SaveChanges();
            }
        }

        #region
        private string currentServiceName
        {
            get
            {
                return GetCurrentLine[0].Trim();
            }
        }

        private string currentAccountingType
        {
            get
            {
                return GetCurrentLine[1].Trim();
            }
        }

        private string currentPrice
        {
            get
            {
                return GetCurrentLine[2].Trim();
            }
        }
        #endregion
    }
}