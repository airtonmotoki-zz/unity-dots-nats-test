using NATS.Client;
using System.Text;
using Unity.Entities;
using UnityEngine;

public class TestSyncSubscriptionAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	[SerializeField]
	private string _subject;
	[SerializeField]
	private string _queue;

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddComponentData(entity, new TestSyncSubscription
		{
			Subject = _subject,
			Queue = _queue
		});
	}
}

public class TestSyncSubscription : NatsSyncSubscription { }

public class TestSyncSubscriptionSystem : SyncSubscriptionSystem<TestSyncSubscription> { }

public class TestReceiveSyncSubscriptionSystem : ReceiveSyncSubscriptionSystem<TestSyncSubscription>
{
	protected override void OnReceive(Entity entity, Msg msg)
	{
		UnityEngine.Debug.Log("Test receive: " + msg);
	}
}

public class TestSendSyncSubscriptionSystem : SystemBase
{
	protected override void OnUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			var entity = EntityManager.CreateEntity();
			EntityManager.AddComponentData(entity, new NatsPublish
			{
				Subject = "Teste",
				Data = Encoding.UTF8.GetBytes("Hello!!!")
			});
		}
	}
}