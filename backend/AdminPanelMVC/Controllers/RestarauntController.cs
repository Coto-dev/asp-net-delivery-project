using System.Diagnostics;
using AdminPanelMVC.Models;
using Common.AdminPanelInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanelMVC.Controllers {
    public class RestarauntController : Controller {
        private readonly ILogger<RestarauntController> _logger;
        private readonly ICrudService _crudService;

        public RestarauntController(ILogger<RestarauntController> logger, ICrudService crudservice) {
            _logger = logger;
            _crudService = crudservice;
        }

        public IActionResult Index() {

            var model = _crudService.GetRestarauntList();
            return View(model);
        }
        [HttpGet]
        [Route("{id}")]
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
				return View();
			}
            catch(KeyNotFoundException ex) {
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