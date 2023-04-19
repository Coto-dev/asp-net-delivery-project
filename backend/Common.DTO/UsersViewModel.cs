using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.DTO {
    public class UsersViewModel {
        public Guid Id { get; set; }
        public string? Search { get; set; } = string.Empty;
		public string Email { get; set; }
		[MinLength(1)]
        [MaxLength(64)]     
        public string FullName { get; set; }
        [DisplayName("Дата рождения")]
        [Required]
        public DateTime BirthDate { get; set; }
        [DisplayName("Пол")]
        [Required]
        public Genders Gender { get; set; }
        [DisplayName("Адрес")]
        public string? Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Номер телефона")]
        [Required]
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
