using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.BackendInterfaces;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Notifcations.BL {
	public class RabbitMQService : IRabbitMQService {
		public void SendMessage<T>(T message) {

			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel()) {
				channel.QueueDeclare(queue: "MyQueue",
							   durable: false,
							   exclusive: false,
							   autoDelete: false,
							   arguments: null);

				var json = JsonConvert.SerializeObject(message);
				var body = Encoding.UTF8.GetBytes(json);

				channel.BasicPublish(exchange: "",
							   routingKey: "MyQueue",
							   basicProperties: null,
							   body: body);
			}
		}
		/*public void SendMessage<T>(T message) {
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel()) {
				channel.ExchangeDeclare(exchange: "notifier", type: ExchangeType.Fanout);
				var queueName = channel.QueueDeclare();
				channel.QueueBind(queue: "MyQueue",
								  exchange: "notifier",
								  routingKey: string.Empty);

				var json = JsonConvert.SerializeObject(message);
				var body = Encoding.UTF8.GetBytes(json);
				channel.BasicPublish(exchange: "notifier",
									routingKey: "",
									basicProperties: null,
									body: body);

			}
			
		}*/
	}
}
