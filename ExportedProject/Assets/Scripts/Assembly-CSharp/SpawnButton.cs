using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
	public Button button;

	public GameObject prefab;

	public Transform destination;

	private void Start()
	{
		button.onClick.AddListener(SpawnPrefab);
	}

	private void SpawnPrefab()
	{
		GameObject gameObject = Object.Instantiate(prefab, destination.position, destination.rotation);
		gameObject.transform.SetParent(destination);
	}
}
