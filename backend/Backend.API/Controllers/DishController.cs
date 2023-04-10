using Common.AuthInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {
    /// <summary>
    /// Controller for dish managment
    /// </summary>
    [Route("api/dish")]
    [ApiController]
    public class DishController : ControllerBase {

        private readonly ILogger<DishController> _logger;
        private readonly IDishService _dishService;

        public DishController(ILogger<DishController> logger, IDishService scheduleService) {
            _logger = logger;
            _dishService = scheduleService;

        }

        /// <summary>
        /// Get all dishes from menu
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        [HttpGet]
        public async Task<ActionResult<DishesPagedListDTO>> GetGishes([FromQuery] DishFilterModelDTO model) {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get dish details by id 
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<DishDetailsDTO>> GetGishDetails() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// add rating to dish  
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        [HttpPost]
        [Route("{id}/rating")]
        public async Task<ActionResult<RatingDTO>> AddRatingToDish(Guid dishId) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// create new dish  
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        /// <param name="MenuId">if null then saved to hidden menu</param>
        [HttpPost]
        [Route("create/{restarauntId}")]
        public async Task<ActionResult<DishDetailsDTO>> CreateDish(Guid menuId) {
            throw new NotImplementedException();
        }
    }
}
