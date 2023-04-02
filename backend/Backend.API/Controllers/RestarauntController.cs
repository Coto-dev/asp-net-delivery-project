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
        public async Task GetRestaraunts() {

        }



    }
}
