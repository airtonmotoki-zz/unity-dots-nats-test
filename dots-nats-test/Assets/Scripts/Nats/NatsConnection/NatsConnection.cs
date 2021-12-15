using NATS.Client;
using Unity.Entities;

public class NatsConnection : IComponentData
{
	public IConnection connection;
}
