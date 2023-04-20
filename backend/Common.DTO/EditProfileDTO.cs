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
		[Required(ErrorMessage = "Дата рождения обязательна для заполнения")]
		[DataType(DataType.Date)]
		[Display(Name = "Дата рождения")]
		[Range(typeof(DateTime), "1/1/1900", "1/1/2023")]
		public DateTime BitrhDate { get; set; }
        [DisplayName("Пол")]
        public Genders Gender { get; set; }
        [DisplayName("Адрес")]
        public string? Address { get; set; }
        [Phone]
		[Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
		[DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}
