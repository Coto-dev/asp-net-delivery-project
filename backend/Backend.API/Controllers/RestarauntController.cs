using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {
    /// <summary>
    /// Controller for get inforamation about restaraunt
    /// </summary>
    [Route("api/restaraunt")]
    [ApiController]
    public class RestarauntController : ControllerBase {


        /// <summary>
        /// Get all restaurant names
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        [HttpGet]
        public async Task<ActionResult<RestarauntDTO>> GetRestaraunts() {
            throw new NotImplementedException();

        }

        /// <summary>
        /// get info about created orders for cook
        /// </summary>
        [HttpGet]
        [Route("cook/createdOrders")]
        public async Task<ActionResult<OrderPagedList>> GetCreatedOrders([FromQuery] OrderFilterCookCreated model) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get info about history orders for certain cook
        /// </summary>
        [HttpGet]
        [Route("cook/history")]
        public async Task<ActionResult<OrderPagedList>> GetOrdersHistoryCook([FromQuery] OrderFilterCook model) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get info about all orders in single restaraunt for manager
        /// </summary>
        [HttpGet]
        [Route("manager/allOrders")]
        public async Task<ActionResult<OrderPagedList>> GetManagerOrders([FromQuery] OrderFilterManager model) {
            throw new NotImplementedException();
        }


    }
}
