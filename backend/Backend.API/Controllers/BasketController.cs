using Azure;
using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response = Common.DTO.Response;

namespace Backend.API.Controllers {
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase {

        ///<summary>
        ///get basket
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<List<BasketDTO>>> GetBasket(Guid dishId) {
            throw new NotImplementedException();
        }
        ///<summary>
        ///add dish to cart
        /// </summary>
        [HttpPost]
        [Route("dish/{dishId}")]
        public async Task<ActionResult<Response>> AddDishToBasket(Guid dishId) {
            throw new NotImplementedException();
        }
        ///<summary>
        ///if "CompletelyDelete" = false then decrease number of dish , else remove dish completely
        /// </summary>
        [HttpDelete]
        [Route("dish/{dishId}")]
        public async Task<ActionResult<Response>> RemoveDish(Guid dishId, bool CompletelyDelete) {
            throw new NotImplementedException();
        }
    }
}
