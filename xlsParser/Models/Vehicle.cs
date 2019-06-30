using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace xlsParser.Models
{
    public class Vehicle
    {
        [DataType("PrimaryKey")]
        public int Id { get; set; }

        [DisplayName("Транспортное средство")]
        [Required(ErrorMessage = "обязательное поле")]
        public string Title { get; set; }

        [DisplayName("Номер ТС")]
        [Required(ErrorMessage = "обязательное поле")]
        public string VehicleNumber { get; set; }

        [DisplayName("Пароль ТС")]
        [Required(ErrorMessage = "обязательное поле")]
        public string VehiclePassword { get; set; }


        [ForeignKey("Carrier")]
        [DataType("ForeignKey")]
        [DisplayName("Грузоперевозчик")]
        public int? CarrierId { get; set; }

        [DataType("Reference")]
        public virtual Carrier Carrier { get; set; }
    }
}