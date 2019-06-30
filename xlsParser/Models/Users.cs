using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace xlsParser.Models
{
    public class Users
    {
        [DataType("PrimaryKey")]
        public int Id { get; set; }

        [Required(ErrorMessage = "обязательное поле")]
        public string Email { get; set; }

        [Required(ErrorMessage = "обязательное поле")]
        public string Password { get; set; }

        [ForeignKey("Carrier")]
        [DataType("ForeignKey")]
        [DisplayName("Грузоперевозчик")]
        public int? CarrierId { get; set; }

        [DataType("Reference")]
        public virtual Carrier Carrier { get; set; }


        [ForeignKey("Parking")]
        [DataType("ForeignKey")]
        [DisplayName("Точка")]
        public int? ParkingId { get; set; }

        [DataType("Reference")]
        public virtual Parking Parking { get; set; }
    }
}