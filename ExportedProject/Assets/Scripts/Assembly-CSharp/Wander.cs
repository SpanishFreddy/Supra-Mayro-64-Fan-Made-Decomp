using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour
{
	public float directionTime = 10f;

	public Vector3 direction;

	public float speed = 10f;

	[SerializeField]
	public NavMeshAgent rb;

	private void Start()
	{
		ChangeDirection();
	}

	private void Update()
	{
		directionTime -= Time.deltaTime;
		if (directionTime <= 0f)
		{
			ChangeDirection();
			directionTime = 5f;
		}
		rb.velocity = direction * speed;
	}

	private void ChangeDirection()
	{
		direction.x = Random.Range(-1f, 1f);
		direction.z = Random.Range(-1f, 1f);
		direction = direction.normalized;
	}
}
