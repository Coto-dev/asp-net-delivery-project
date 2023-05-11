using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BackendInterfaces {
	public interface IRabbitMQService {
		void SendMessage<T>(T message);

	}
}
