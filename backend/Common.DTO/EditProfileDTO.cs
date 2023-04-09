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
        
        [MinLength(1)]
        [MaxLength(64)]
        [DisplayName("ФИО")]
        public string FullName { get; set; }
        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime BitrhDate { get; set; }
        [DisplayName("Пол")]
        public Genders Gender { get; set; }
        [DisplayName("Адрес")]
        public string? Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}
