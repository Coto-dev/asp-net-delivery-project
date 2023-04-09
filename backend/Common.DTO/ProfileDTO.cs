using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
    public class ProfileDTO {
       public Guid Id { get; set; }
        [DisplayName("ФИО")]
        public string FullName { get; set; }
        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)] 
        public string BirthDate { get; set; }
        [DisplayName("Пол")]
        public Genders Gender { get; set; }
        [DisplayName("Адресс")]
        public string? Address { get; set; }
        [DisplayName("Почта")]
        public string Email { get; set; }
        [DisplayName("Телефон")]
        public string PhoneNumber { get; set; }
        [DisplayName("Роли")]
        public List<String> Roles { get; set; }
    }
}
