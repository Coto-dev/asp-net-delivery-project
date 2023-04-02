using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {
    [Route("api/order")]
    [ApiController]
    public class OrderControllerr : ControllerBase {


        /// <summary>
        /// get information about orders history  
        /// </summary>
        /// <remarks>
        /// can be used by courier , customer , cook  
        /// </remarks>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        [HttpGet]
        [Route("history")]
        public async Task GetOrderHistory() {

        }


        /// <summary>
        /// get information about current order 
        /// </summary>
        /// <remarks>
        /// can be used by courier , customer , cook    
        /// </remarks>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        [HttpGet]
        [Route("current")]
        public async Task GetCurrentOrder() {

        }
    }
}
