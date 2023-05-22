

namespace Notifications.API {
	using RabbitMQ.Client.Events;
	using RabbitMQ.Client;
	using System.Threading.Tasks;
	using System.Threading;
	using Microsoft.Extensions.Hosting;
	using System.Text;
	using System.Diagnostics;
	using System;
	using Microsoft.AspNetCore.SignalR;
	using Microsoft.Extensions.DependencyInjection;
	using Notifications.API.Hubs;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Notifications.BL;

	public class RabbitMqBackGroundListener : BackgroundService {
		private IConnection _connection;
		private IModel _channel;
		private IServiceScopeFactory _scope { get; }

		public RabbitMqBackGroundListener(IServiceScopeFactory scopeFactory) {
			var factory = new ConnectionFactory { HostName = Config.HostName };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_scope = scopeFactory;
			_channel.QueueDeclare(queue: Config.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken) {
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += async (ch, ea) =>
			{
				var content = Encoding.UTF8.GetString(ea.Body.ToArray());
				try {
					dynamic jsonObject = JObject.Parse(content);
					var userId = jsonObject.userId.ToString();

					using (var scope = _scope.CreateScope()) {
						var _hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationsHub>>();
						var group = _hub.Clients.Group(userId) as IClientProxy;
						await group.SendAsync("ReceiveMessage",content);
					}
				}
				catch (JsonException ex) {
					
				}
				
				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume(Config.QueueName, false, consumer);

			return Task.CompletedTask;
		}

		public override void Dispose() {
			_channel.Close();
			_connection.Close();
			base.Dispose();
		}
	}
}
