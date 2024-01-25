using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
	public int speed;

	public GameObject player;

	private void Update()
	{
		Vector3 normalized = (player.transform.position - base.transform.position).normalized;
		base.transform.Translate(normalized.x * Time.deltaTime * (float)speed, normalized.y * Time.deltaTime * (float)speed, normalized.z * Time.deltaTime * (float)speed);
	}
}
