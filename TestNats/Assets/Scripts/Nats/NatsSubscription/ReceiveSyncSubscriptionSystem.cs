using NATS.Client;
using Unity.Entities;

public abstract class ReceiveSyncSubscriptionSystem<T> : SystemBase where T : NatsSyncSubscription
{
	private static long _maxPendingMessages = 10;

	EntityQuery _query;

	protected override void OnCreate()
	{
		base.OnCreate();

		_query = GetEntityQuery(ComponentType.ReadWrite<T>(), ComponentType.ReadOnly<NatsSubscribed>());

		RequireForUpdate(_query);
	}

	protected override void OnUpdate()
	{
		var entities = _query.ToEntityArray(Unity.Collections.Allocator.Temp);

		for (var index = 0; index < entities.Length; index++)
		{
			var entity = entities[index];
			var t = EntityManager.GetComponentObject<T>(entity);
			
			var maxPendingMessage = _maxPendingMessages;
			while (t.Subscription.PendingMessages > 0 && maxPendingMessage > 0)
			{
				var msg = t.Subscription.NextMessage(1000);
				maxPendingMessage--;
				OnReceive(entity , msg);
			}
		}

		entities.Dispose();
	}

	protected abstract void OnReceive(Entity entity, Msg msg);
}

