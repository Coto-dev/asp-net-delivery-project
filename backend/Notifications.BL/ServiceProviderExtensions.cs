using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.BackendInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Notifcations.BL;

namespace Notifications.BL {

		public static class ServiceProviderExtensions {
			public static void AddNotifactionsServices(this IServiceCollection services) {
				services.AddScoped<IRabbitMQService, RabbitMQService>();
		}
	}
	

}
