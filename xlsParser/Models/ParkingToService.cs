using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace xlsParser.Models
{
    public class ParkingToService
    {
        [DataType("PrimaryKey")]
        public int Id { get; set; }

        [ForeignKey("Service")]
        [DataType("ForeignKey")]
        [DisplayName("Услуга")]
        public int? ServiceId { get; set; }

        [DataType("Reference")]
        public virtual Service Service { get; set; }

        [ForeignKey("Parking")]
        [DataType("ForeignKey")]
        [DisplayName("Стоянка")]
        public int? ParkingId { get; set; }

        [DataType("Reference")]
        public virtual Parking Parking { get; set; }
    }
}