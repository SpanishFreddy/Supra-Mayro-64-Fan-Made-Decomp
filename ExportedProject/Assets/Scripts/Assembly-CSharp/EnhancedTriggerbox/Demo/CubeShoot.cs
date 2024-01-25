using UnityEngine;

namespace EnhancedTriggerbox.Demo
{
	public class CubeShoot : MonoBehaviour
	{
		public void Start()
		{
			GetComponent<Rigidbody>().isKinematic = true;
		}

		public void ShootCube(float velocity)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			Vector3 position = Camera.main.transform.position;
			Vector3 force = (position - base.transform.position) * velocity;
			GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		}
	}
}
