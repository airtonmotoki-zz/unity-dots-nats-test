using NATS.Client;
using Unity.Entities;

public abstract class NatsSyncSubscription : IComponentData
{
	public string Subject;
	public string Queue;
	public ISyncSubscription Subscription;
}

