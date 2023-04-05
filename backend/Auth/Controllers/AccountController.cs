using Common.AuthInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers {
    [Route("api/account")]
    public class AccountController : ControllerBase {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService) {
            _logger = logger;
            _accountService = accountService;

        }
        /// <summary>
        /// register new user
        /// </summary>
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] RegisterModelDTO RegisterModel) {
            return null;
        }

        /// <summary>
        /// log in the system
        /// </summary>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<Response>> Login([FromBody] LoginCredentials Login ) {
            return null;
        }
        /// <summary>
        /// log out system user
        /// </summary>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<Response>> Logout() {
            return null;
        }
    }
}
