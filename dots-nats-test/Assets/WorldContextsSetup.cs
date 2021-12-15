using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldContextSetup", menuName = "WorldContext/Setup")]
public class WorldContextsSetup : ScriptableObject
{
	public WorldContext[] Worlds;

	public static WorldContextsSetup LoadAsset(string path)
	{
		return AssetDatabase.LoadAssetAtPath<WorldContextsSetup>(path);
	}
}
