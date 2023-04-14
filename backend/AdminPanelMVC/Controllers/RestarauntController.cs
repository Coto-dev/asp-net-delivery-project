using System.Diagnostics;
using AdminPanelMVC.Models;
using Common.AdminPanelInterfaces;
using Common.DTO;
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
		[Route("Restaraunt")]
		public IActionResult Index() {

            var model = _crudService.GetRestarauntList();
            return View(model);
        }
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(Guid id) {
            try {
                var model = await _crudService.GetDetails(id);
                model.ViewModel = new AddUserViewModel { restarauntId= id };
                return View(model);
            }
            catch(Exception ex) {
                return RedirectToAction("Index");
            }
        }
		[HttpPost]
		public async Task<IActionResult> AddCook(AddUserViewModel model) {
			try {
				await _crudService.AddCook(model.Email , model.restarauntId);
				return RedirectToAction("Details",new {id = model.restarauntId });
			}
			catch (KeyNotFoundException ex) {
				ModelState.AddModelError("Email", ex.Message);
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (InvalidOperationException ex) {
				ModelState.AddModelError("Email", ex.Message);
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
			catch (Exception ex) {
				ModelState.AddModelError("Email", ex.Message);
				return RedirectToAction("Details", new { id = model.restarauntId });
			}
		}
		[HttpPost]
		public async Task<IActionResult> AddManager(AddUserViewModel model) {
			try {
				await _crudService.AddManager(model.Email, model.restarauntId);
				return RedirectToAction("Details");
			}
			catch (KeyNotFoundException ex) {
				ModelState.AddModelError("", ex.Message);
				return RedirectToAction("Index");
			}
			catch (InvalidOperationException ex) {
				ModelState.AddModelError("", ex.Message);
				return RedirectToAction("Index");
			}
			catch (Exception ex) {
				ModelState.AddModelError("", ex.Message);
				return RedirectToAction("Index");
			}
		}
		public async Task<IActionResult> Create(RestarauntViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            try {
                await _crudService.CreateRestaraunt(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

        }
        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}