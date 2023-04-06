using System.Diagnostics;
using System.Net;
using Common.AuthInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
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
            try {
                return Ok( await _accountService.Register(RegisterModel));
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (ArgumentNullException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 400, title: e.Message);
            }
            catch (ArgumentException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 409, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        /// <summary>
        /// log in the system
        /// </summary>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginCredentials Login ) {
            try {
                return Ok(await _accountService.Login(Login));
            }
            catch (ArgumentException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 401, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        /// <summary>
        /// log out system user
        /// </summary>
        [Route("logout")]
        [HttpPost]
        public async Task<ActionResult<Response>> Logout() {
            return null;
        }
        /// <summary>
        /// add address to user and make him customer
        /// </summary>
        ///<returns></returns>
        /// <response code = "400" > Bad Request , null address</response>
        /// <response code = "409" >If user already have customer role </response>
        /// <response code = "500" > Internal Server Error</response>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("AddAddress")]
        [HttpPut]
        public async Task<ActionResult<Response>> EditUserToCustomer(string address) {
            try {
                return Ok(await _accountService.EditUserToCustomer(address, User.Identity.Name));
            }
            catch (InvalidOperationException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 403, title: e.Message);
            }
            catch (ArgumentNullException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 409, title: e.Message);
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
        /// <summary>
        /// get user profile
        /// </summary>
        ///<returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "500" > Internal Server Error</response>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("profile")]
        [HttpGet]
        public async Task<ActionResult<ProfileDTO>> GetProfile() {

            try {
                return Ok(await _accountService.GetProfile(User.Identity.Name));
            }
            
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }

        /// <summary>
        /// edit ser profile
        /// </summary>
        ///<returns></returns>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "500" > Internal Server Error</response>
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("profile/edit")]
        [HttpGet]
        public async Task<ActionResult<ProfileDTO>> EditProfile([FromBody] EditProfileDTO model) {

            try {
                return Ok(await _accountService.EditProfile(model, User.Identity.Name));
            }
            catch (ArgumentException e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
            catch (Exception e) {
                _logger.LogError(e,
                    $"Message: {e.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
                return Problem(statusCode: 500, title: "Something went wrong");
            }
        }
    }
}
