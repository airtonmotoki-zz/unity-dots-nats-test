using System.Text;
using Unity.Entities;

public abstract class SyncSubscriptionSystem<T> : SystemBase where T : NatsSyncSubscription
{
	private EntityQuery _query;

	private EntityQuery _natsConnectionQuery;

	protected override void OnCreate()
	{
		base.OnCreate();

		RequireSingletonForUpdate<NatsConnection>();
		_natsConnectionQuery = EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NatsConnection>());

		_query = GetEntityQuery(ComponentType.ReadWrite<T>(), ComponentType.Exclude<NatsSubscribed>());

		RequireForUpdate(_query);

	}

	protected override void OnUpdate()
	{
		var natsConnectionEntity = _natsConnectionQuery.GetSingletonEntity();

		var natsConnection = EntityManager.GetComponentData<NatsConnection>(natsConnectionEntity);

		var entities = _query.ToEntityArray(Unity.Collections.Allocator.Temp);

		for (var index = 0; index < entities.Length; index++)
		{
			var t = EntityManager.GetComponentObject<T>(entities[index]);
			t.Subscription = natsConnection.connection.SubscribeSync(t.Subject, t.Queue);
		}

		EntityManager.AddComponent<NatsSubscribed>(_query);

		entities.Dispose();
	}
}

