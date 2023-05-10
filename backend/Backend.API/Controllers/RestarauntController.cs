using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers {
	/// <summary>
	/// Controller for get inforamation about restaraunt
	/// </summary>
	[Route("api/restaraunt")]
	[ApiController]
	public class RestarauntController : ControllerBase {
		private readonly ILogger<RestarauntController> _logger;
		private readonly IRestarauntService _restarauntService;


		public RestarauntController(ILogger<RestarauntController> logger, IRestarauntService restarauntService) {
			_logger = logger;
			_restarauntService = restarauntService;
		}

		/// <summary>
		/// Get all restaurant info about restaraunts
		/// </summary>
		/// <response code = "400" > Bad Request</response>
		/// <response code = "404" >Not Found</response>
		/// <response code = "500" >InternalServerError</response>
		[HttpGet]
		public async Task<ActionResult<RestarauntPagedList>> GetRestaraunts([FromQuery] string? filter,[FromQuery] int page = 1) {
			return await _restarauntService.GetAllRestaraunts(filter,page);

		}


	}
}
