using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.BackendInterfaces;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using Notifications.BL;
using RabbitMQ.Client;

namespace Notifcations.BL {
	public class RabbitMQService : IRabbitMQService {
		public void SendMessage<T>(T message) {

			var factory = new ConnectionFactory() { HostName = Config.HostName };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel()) {
				channel.QueueDeclare(queue: Config.QueueName,
							   durable: false,
							   exclusive: false,
							   autoDelete: false,
							   arguments: null);

				var json = JsonConvert.SerializeObject(message);
				var body = Encoding.UTF8.GetBytes(json);

				channel.BasicPublish(exchange: "",
							   routingKey: Config.QueueName,
							   basicProperties: null,
							   body: body);
			}
		}
		
	}
}
