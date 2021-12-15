using Unity.Entities;
using UnityEngine;

public class ServerDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddComponent<NatsServerData>(entity);
	}
}
