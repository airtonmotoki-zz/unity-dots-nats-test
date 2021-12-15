using Unity.Entities;

public class NatsPublish : IComponentData
{
	public string Subject;
	public byte[] Data;
}

