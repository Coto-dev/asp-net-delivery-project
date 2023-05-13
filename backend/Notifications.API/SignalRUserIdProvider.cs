using Auth.DAL.Data.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Notifications.API {
	#region SignalRUserIdProvider
	public class SignalRUserIdProvider : IUserIdProvider {
		public string GetUserId(HubConnectionContext connection) {
			return (new Guid (connection.User.FindFirst(ClaimTypes.NameIdentifier).Value)).ToString();
		}
	}
	#endregion


}
