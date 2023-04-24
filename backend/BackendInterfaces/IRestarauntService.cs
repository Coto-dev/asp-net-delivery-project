using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Common.BackendInterfaces {
	public interface IRestarauntService {
		public Task<RestarauntPagedList> GetAllRestaraunts(string? NameFilter, int Page = 1);
		public Task<ActionResult<OrderPagedList>> GetCreatedOrders(OrderFilterCookCreated model, Guid restarauntId);
		public Task<ActionResult<OrderPagedList>> GetOrdersHistoryCook(OrderFilterCook model, Guid restarauntId);
		public Task<ActionResult<OrderPagedList>> GetOrdersCurrentCook(OrderFilterCook model, Guid restarauntId);
		public Task<ActionResult<OrderPagedList>> GetManagerOrders(OrderFilterManager model, Guid restarauntId);

		}
	}
