using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Notifications.API {
	#region SignalRUserIdProvider
	public class SignalRUserIdProvider : IUserIdProvider {
		public string GetUserId(HubConnectionContext connection) {
			return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!; ;
		}
	}
	#endregion


}
