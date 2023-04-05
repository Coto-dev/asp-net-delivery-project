using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO {
    public class TokenResponse {
        [DisplayName("token")] public string? Token { get; set; }
        [DisplayName("email")] public string? Email { get; set; }
        [DisplayName("role")] public IList<string>? Role { get; set; }
        
    }
}
