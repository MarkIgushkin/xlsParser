using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace xlsParser.Models
{
    public class Service
    {
        [DataType("PrimaryKey")]
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required(ErrorMessage = "обязательное поле")]
        public string Title { get; set; }

        [ForeignKey("AccountingType")]
        [DataType("ForeignKey")]
        [DisplayName("Вид учёта")]
        public int? AccountingTypeId { get; set; }

        [DataType("Reference")]
        public virtual AccountingType AccountingType { get; set; }

        [DisplayName("Стоимость")]
        public decimal? Price { get; set; }
    }
}