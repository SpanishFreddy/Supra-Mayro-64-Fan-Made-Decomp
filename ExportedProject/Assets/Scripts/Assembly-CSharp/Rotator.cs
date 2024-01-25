using UnityEngine;

public class Rotator : MonoBehaviour
{
	private void Update()
	{
		base.transform.Rotate(new Vector3(15f, 30f, 45f) * Time.deltaTime);
	}
}
