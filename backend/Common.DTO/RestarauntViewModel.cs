using System.ComponentModel.DataAnnotations;

namespace Common.DTO {
    public class RestarauntViewModel {
        
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [MinLength(6)]
        public string PhotoUrl { get; set; }
        public AddUserViewModel? ViewModel { get; set; }
        public List<string>? CookEmails { get; set; }
		public List<string>? ManagerEmails { get; set; }

	}
}
