using System.Diagnostics;
using Common.AdminPanelInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Packaging;

namespace AdminPanelMVC.Controllers {
	public class UserManagerController : Controller {
		private readonly ILogger<UserManagerController> _logger;
		private readonly IUserManagerService _userManagerService;

		public UserManagerController(ILogger<UserManagerController> logger, IUserManagerService userManagerService) {
			_logger = logger;
			_userManagerService = userManagerService;
		}
		public async Task<IActionResult> Index(string? search) {
			
			try {
				
				var users = await _userManagerService.GetUsers(search);
				var searchedList = new UsersSearchedViewModel {
					Users= users,
					Search = search
				};
				return View(searchedList);
			}
			catch (Exception ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
				return View(ex);
			}
		}
		[HttpPost]

		public async Task<IActionResult> EditUser(UsersViewModel model) {
			if (ModelState.IsValid) {
				try {

					await _userManagerService.EditUser(model);
					return RedirectToAction("index");

				}
				catch (InvalidOperationException ex) {
					_logger.LogError(ex,
					  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
					return RedirectToAction("index");
				}
				catch (Exception ex) {
					_logger.LogError(ex,
					  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
					return RedirectToAction("index");
				}
			}
			var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
			string errorString = string.Join(" ", errors);
			TempData["Errors"] = errorString;
			return RedirectToAction("index");
		}
		[HttpPost]
		public async Task<IActionResult> BanUser(UsersViewModel model) {
			if (ModelState.IsValid) {
				try {

					await _userManagerService.BanUser(model.Id);
					return RedirectToAction("index");

				}
				catch (InvalidOperationException ex) {
					_logger.LogError(ex,
					  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
					return RedirectToAction("index");
				}
				catch (Exception ex) {
					_logger.LogError(ex,
					  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
					return RedirectToAction("index");
				}
			}
			var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
			string errorString = string.Join(" ", errors);
			TempData["Errors"] = errorString;
			return RedirectToAction("index");
		}

	}
}
