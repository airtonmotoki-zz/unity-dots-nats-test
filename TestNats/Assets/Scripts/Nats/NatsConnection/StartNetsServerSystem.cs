using NATS.Client;
using Unity.Entities;

public class StartNetsServerSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities
			.WithNone<NatsConnection>()
			.ForEach((Entity entity, ref NatsServerData connectionData) =>
			{
				var connectionFactory = new ConnectionFactory();

				var connection = connectionFactory.CreateConnection();

				EntityManager.AddComponentObject(entity, new NatsConnection
				{
					connection = connection
				});
			})
			.WithStructuralChanges()
			.Run();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		Entities
			.ForEach((Entity entity, NatsConnection connection, ref NatsServerData connectionData) =>
			{
				connection.connection.Close();

				EntityManager.RemoveComponent<NatsConnection>(entity);
			})
			.WithStructuralChanges()
			.Run();
	}
}