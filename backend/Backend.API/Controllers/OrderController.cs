using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers {
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase {


        /// <summary>
        /// get information about customer orders history  
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        [HttpGet]
        [Route("customer/ordersHistory")]
        public async Task<ActionResult<OrderPagedList>> GetCustomerOrder([FromQuery] OrderFilterCustomer model) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get information about current order 
        /// </summary>
        /// <response code = "400" > Bad Request</response>
        /// <response code = "404" >Not Found</response>
        /// <response code = "500" >InternalServerError</response>
        [HttpGet]
        [Route("customer/current")]
        public async Task<ActionResult<OrderDTO>> GetCurrentOrder() {
            throw new NotImplementedException(); //если у заказа статус от created до deliveried(не включая)
        }

        /// <summary>
        /// cancel order if only status created or delivery(courier)
        /// </summary>
        /// <remarks>
        /// if address will be null then address wil be taken from user profile
        /// </remarks>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Response>> CreateOrder(string? address, DateTime deliveryTime) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get info about all ready to delivery orders for courier
        /// </summary>
        [HttpPost]
        [Route("customer/repeat/{orderId}")]
        public async Task<ActionResult<Response>> GetCourierOrders(Guid orderId) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change order status
        /// </summary>
        /// <remarks>
        /// for cook “created” to “kitchen” to “readyToDelivery”
        /// for courier “readyToDelivery” to “Delivery” to “Delivered”
        /// </remarks>
        [HttpPut]
        [Route("status/change{orderId}")]
        public async Task<ActionResult<Response>> ChangeOrderStatus(Guid orderId) {
            throw new NotImplementedException();// 
        }
        /// <summary>
        /// cancel order if only status created or delivery(courier)
        /// </summary>
        /// <remarks>
        /// for customer created to cancelled
        /// for courier delivery to cancelled
        /// </remarks>
        [HttpDelete]
        [Route("cancel")]
        public async Task<ActionResult<Response>> CancelOrder() {
            throw new NotImplementedException();
        }
       

        

        /// <summary>
        /// get info about all ready to delivery orders for courier
        /// </summary>
        [HttpGet]
        [Route("courier/orders")]
        public async Task<ActionResult<OrderPagedList>> GetCourierOrders() {
            throw new NotImplementedException();
        }

        
    }
}
