
namespace Notifications.BL {
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
	using Microsoft.Extensions.Logging;

	public class RabbitMqBackGroundListenerr : BackgroundService {
		private IConnection _connection;
		private IModel _channel;
		private IServiceScopeFactory _scope { get; }

		public RabbitMqBackGroundListenerr(IServiceScopeFactory scopeFactory) {
			var factory = new ConnectionFactory { HostName = "localhost" };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_scope = scopeFactory;
			_channel.QueueDeclare(queue: "MyQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken) {
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (ch, ea) =>
			{
				var content = Encoding.UTF8.GetString(ea.Body.ToArray());
				using (var scope = _scope.CreateScope()) {
					var _hub = scope.ServiceProvider.GetRequiredService<IHubContext>();
				//	var _hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotifcationsHub>();

					_hub.Clients.All.SendAsync("ReceiveMessage", content);
				}

				Debug.WriteLine($"Получено сообщение: {content}");

				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume("MyQueue", false, consumer);

			return Task.CompletedTask;
		}

		public override void Dispose() {
			_channel.Close();
			_connection.Close();
			base.Dispose();
		}
	}
}
