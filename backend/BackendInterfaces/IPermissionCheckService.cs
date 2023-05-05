using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BackendInterfaces {
	public interface IPermissionCheckService {
		public Task CheckPermissionForManagerByRestaraunt(Guid restarauntId, Guid managerId);
		public Task CheckPermissionForManagerByMenu(Guid menuId, Guid managerId);
		public Task CheckPermissionForCook(Guid orderId, Guid cookId);
		public Task CheckPermissionForCourier(Guid orderId, Guid courierId);
		public Task CheckPermissionForCustomer(Guid orderId, Guid customerId);

	}
}
