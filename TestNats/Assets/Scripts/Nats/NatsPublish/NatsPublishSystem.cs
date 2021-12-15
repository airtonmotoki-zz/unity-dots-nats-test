using Unity.Entities;

public class NatsPublishSystem : SystemBase
{
	EntityQuery _query;

	protected override void OnCreate()
	{
		base.OnCreate();

		RequireSingletonForUpdate<NatsConnection>();

		_query = GetEntityQuery(ComponentType.ReadWrite<NatsPublish>());

		RequireForUpdate(_query);
	}

	protected override void OnUpdate()
	{
		var natsConnectionEntity = GetSingletonEntity<NatsConnection>();
		var natsConnection = EntityManager.GetComponentObject<NatsConnection>(natsConnectionEntity);

		var entities = _query.ToEntityArray(Unity.Collections.Allocator.Temp);

		for (var index = 0; index < entities.Length; index++)
		{
			var t = EntityManager.GetComponentObject<NatsPublish>(entities[index]);
			natsConnection.connection.Publish(t.Subject, t.Data);
		}
		EntityManager.DestroyEntity(_query);

		entities.Dispose();
	}
}

