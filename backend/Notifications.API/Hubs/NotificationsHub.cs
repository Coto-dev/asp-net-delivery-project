using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Notifications.API.Hubs {
	[Authorize]

	public class NotificationsHub : Hub {

		public override async Task OnConnectedAsync() {

			var _client = new HttpClient();
				await Clients.Caller.SendAsync("ReceiveMessage", "подключение установлено");
				await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier);
				

			await base.OnConnectedAsync();
		}
	}

}
