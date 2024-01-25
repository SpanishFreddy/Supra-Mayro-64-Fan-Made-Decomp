using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
	public void InstantiateCaller(GameObject prefab)
	{
		Object.Instantiate(prefab);
	}
}
