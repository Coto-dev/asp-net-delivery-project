using AdminPanelMVC.Models;
using Common.AdminPanelInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanelMVC.Controllers {

	public class AccountController : Controller {
        private readonly IAccountService _accountService; 

        public AccountController(IAccountService accountService) {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login() {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginCredentials model) {
            if (ModelState.IsValid) {
                try {
                    await _accountService.Login(model);
                    return RedirectToAction("Index", "Restaraunt");
                }
				catch (ArgumentException ex) {
					ModelState.AddModelError("Errors", ex.Message);
				}
				catch (Exception ex) {
                    ModelState.AddModelError("Errors", ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _accountService.Logout();
            return RedirectToAction("Index", "Restaraunt");
        }

    }
}
