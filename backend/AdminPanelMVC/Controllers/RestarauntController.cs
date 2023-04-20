using System.Diagnostics;
using AdminPanelMVC.Models;
using Common.AdminPanelInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AdminPanelMVC.Controllers {
	public class RestarauntController : Controller {
		private readonly ILogger<RestarauntController> _logger;
		private readonly ICrudService _crudService;

		public RestarauntController(ILogger<RestarauntController> logger, ICrudService crudservice) {
			_logger = logger;
			_crudService = crudservice;
		}
		[Route("")]
		[Authorize]
		[Route("Restaraunt")]
		public IActionResult Index() {

			var model = _crudService.GetRestarauntList();
			return View(model);
		}
		[HttpGet]
		[Authorize]
		[Route("Details/{id}")]
		public async Task<IActionResult> Details(Guid id) {
			try {

				//if (TempData["Errors"]!=null)
				var model = await _crudService.GetDetails(id);
				model.ViewModel = new AddUserViewModel { restarauntId = id };
				return View(model);
			}
			catch (Exception ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				return RedirectToAction("Index");
			}
		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> AddCook(AddUserViewModel model) {

			try {
				await _crudService.AddCook(model.Email, model.restarauntId);
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (KeyNotFoundException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (ArgumentException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (InvalidOperationException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (Exception ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");
				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}

		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> AddManager(AddUserViewModel model) {
			try {
				await _crudService.AddManager(model.Email, model.restarauntId);
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (KeyNotFoundException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (ArgumentException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (InvalidOperationException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (Exception ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> DeleteCook(AddUserViewModel model) {
			try {

				await _crudService.DeleteCook(model);
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (ArgumentException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (Exception ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> DeleteManager(AddUserViewModel model) {
			try {

				await _crudService.DeleteManager(model);
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (ArgumentException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (Exception ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> DeleteRest(ViewRestaraunt model) {
			try {
				await _crudService.Delete(model.Id);
				return RedirectToAction("Details", new { id = model.Id });
			}
			catch (ArgumentException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				ModelState.AddModelError("", ex.Message);
				return RedirectToAction("Details", new { id = model.Id });
			}

		}
		[HttpPost]
		[Authorize]

		public async Task<IActionResult> RecoverRest(ViewRestaraunt model) {
			try {
				await _crudService.RecoverRest(model.Id);
				return RedirectToAction("Details", new { id = model.Id });
			}
			catch (ArgumentException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				ModelState.AddModelError("", ex.Message);
				return RedirectToAction("Details", new { id = model.Id });
			}
		}
		[HttpGet]
		[Authorize]
		[Route("Recover/{id}")]
		public async Task<IActionResult> Recover(Guid id) {
			try {
				var rest = await _crudService.GetRestaraunt(id);
				return View("Recover",rest);
			}
			catch (KeyNotFoundException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				ModelState.AddModelError("", ex.Message);
				return RedirectToAction("Details", new { id = id });
			}

		}


		[HttpGet]
		[Authorize]
		[Route("Delete/{id}")]
		public async Task<IActionResult> Delete(Guid id) {
			try {
				var rest = await _crudService.GetRestaraunt(id);
				return View(rest);
			}
			catch (KeyNotFoundException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				ModelState.AddModelError("", ex.Message);
				return RedirectToAction("Details", new { id = id });
			}
			
		}
		[HttpPut]

		[HttpGet]
		[Authorize]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit(Guid id) {
			try {
				var book = await _crudService.GetForEdit(id);
				return View(book);
			}
			catch {
				return RedirectToAction("Index");
			}
		}
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit(EditRestarauntVIew model) {
			if (!ModelState.IsValid) {
				return View(model);
			}
			try {
				await _crudService.Edit(model);
				return RedirectToAction("Index");
			}
			catch (Exception ex) {
				TempData["Errors"] += ex.Message;
				return View(model);
			}
		}
		[Authorize]

		public async Task<IActionResult> Create(RestarauntViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            try {
                await _crudService.CreateRestaraunt(model);
                return RedirectToAction("Index");
            }
			catch (ArgumentException ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
				return View(model);
			}
			catch (Exception ex) {
				_logger.LogError(ex,
				  $"Message: {ex.Message} TraceId: {Activity.Current?.Id ?? HttpContext.TraceIdentifier}");

				TempData["Errors"] += ex.Message;
                return View(model);
            }

        }
        /*public IActionResult Privacy() {
            return View();
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}