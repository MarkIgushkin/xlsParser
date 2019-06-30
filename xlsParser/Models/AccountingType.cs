using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace xlsParser.Models
{
    public class AccountingType
    {
        [DataType("PrimaryKey")]
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required(ErrorMessage = "обязательное поле")]
        public string Title { get; set; }
    }
}