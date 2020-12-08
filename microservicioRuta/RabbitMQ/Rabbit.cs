using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using microservicioRuta.Repository;

namespace microservicioRuta.RabbitMQ
{
	public class Rabbit
	{ 
		public Rabbit() { }

		public void consumidor()
		{
			String cua = "MicroserveiRutes";
			var factory = new ConnectionFactory() { HostName = "localhost" };

			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(cua, false, false, false, null);

					var consumer = new EventingBasicConsumer(channel);

					consumer.Received += (model, ea) =>
					{
						var message = Encoding.UTF8.GetString(ea.Body.ToArray());
						Console.WriteLine(" [x] Received {0}", message);
					};

					channel.BasicConsume(queue: cua, autoAck: true, consumer: consumer);

					while (true) { }
				}
			}
		}
	}
}
