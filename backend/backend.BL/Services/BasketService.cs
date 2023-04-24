using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Common.BackendInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.BL.Services {
	public class BasketService : IBasketService {
		private readonly ILogger<BasketService> _logger;
		private readonly BackendDbContext _context;
		public BasketService(ILogger<BasketService> logger, BackendDbContext context) {
			_logger = logger;
			_context = context;
		}

		public async Task<ActionResult<Response>> AddDishToBasket(Guid dishId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> ClearBasket(Guid dishId, bool CompletelyDelete) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<List<BasketDTO>>> GetBasket(Guid dishId) {
			throw new NotImplementedException();
		}

		public async Task<ActionResult<Response>> RemoveDish(Guid dishId, bool CompletelyDelete) {
			throw new NotImplementedException();
		}
	}
}
