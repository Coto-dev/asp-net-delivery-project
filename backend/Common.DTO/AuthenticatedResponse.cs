using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class AuthenticatedResponse {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        [DisplayName("role")] public List<string>? Role { get; set; }
        [DisplayName("email")] public string? Email { get; set; }
    }
}
