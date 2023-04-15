using Common.AdminPanelInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanelMVC.Controllers {
	public class UserManagerController : Controller {
		private readonly ILogger<UserManagerController> _logger;
		private readonly IUserManagerService _userManagerService;

		public UserManagerController(ILogger<UserManagerController> logger, IUserManagerService userManagerService) {
			_logger = logger;
			_userManagerService = userManagerService;
		}
		public IActionResult Index() {
			return View();
		}
	}
}
