using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.BackendInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Notifcations.BL;

namespace Notifications.BL {
	public static class BackgroundServiceProvider {
		public static void AddBackGroundService(this IServiceCollection services) {
			services.AddHostedService<RabbitMqBackGroundListener>();

		}
	}
}
