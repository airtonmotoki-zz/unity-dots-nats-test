using Unity.Entities;
using UnityEngine.LowLevel;

public class Bootstrapper : ICustomBootstrap
{
	public bool Initialize(string defaultWorldName)
	{
		var setup = WorldContextsSetup.LoadAsset("Assets/Setup/WorldContextSetup.asset");

		var cachedWorld = World.DefaultGameObjectInjectionWorld;
		foreach (var worldContext in setup.Worlds)
		{
			var world = new World($"{worldContext.Name}");

			World.DefaultGameObjectInjectionWorld = world;

			var systems = DefaultWorldInitialization.GetAllSystems(WorldSystemFilterFlags.Default);
			DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(world, systems);

			var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
			ScriptBehaviourUpdateOrder.AddWorldToPlayerLoop(world, ref playerLoop);
			PlayerLoop.SetPlayerLoop(playerLoop);

			foreach (var prefab in worldContext.GameObjects)
			{
				UnityEngine.Object.Instantiate(prefab);
			}
		}
		World.DefaultGameObjectInjectionWorld = cachedWorld;

		return false;
	}
}
