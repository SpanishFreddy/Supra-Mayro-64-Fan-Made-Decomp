using UnityEngine;

public class FPSControllerShoot : MonoBehaviour
{
	public float velocity = 1000f;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.transform.position = Camera.main.transform.position;
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 10f;
			gameObject.transform.LookAt(Camera.main.ScreenToWorldPoint(mousePosition));
			rigidbody.AddRelativeForce((Camera.main.ScreenToWorldPoint(mousePosition) - base.transform.position).normalized * velocity);
		}
	}
}
