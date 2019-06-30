using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace xlsParser.Models
{
    public class Parking
    {
        [DataType("PrimaryKey")]
        public int Id { get; set; }

        [DisplayName("Название компании")]
        [Required(ErrorMessage = "обязательное поле")]
        public string Title { get; set; }

        [DisplayName("ИНН/КПП")]
        public string Inn { get; set; }

        [DisplayName("Адрес")]
        public string Address { get; set; }
    }
}