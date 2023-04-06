using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
    public class EditProfileDTO {
        [MinLength(5)]
        [DisplayName("ФИО")]
        public string FullName { get; set; }
        [DisplayName("Дата рождения")]
        public string Birthday { get; set; }
        [DisplayName("Пол")]
        public Genders Gender { get; set; }
        [DisplayName("Адрес")]
        public string? address { get; set; }
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Номер телефона")]
        public string phoneNumber { get; set; }
    }
}
