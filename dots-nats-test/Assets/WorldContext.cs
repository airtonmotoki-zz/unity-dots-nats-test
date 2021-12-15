using UnityEngine;

[CreateAssetMenu(fileName = "WorldContextSetup", menuName = "WorldContext/Context")]
public class WorldContext : ScriptableObject
{
	public string Name;

	public GameObject[] GameObjects;
}
