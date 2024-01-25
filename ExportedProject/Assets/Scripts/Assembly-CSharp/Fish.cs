using UnityEngine;

public class Fish : MonoBehaviour
{
	public Transform[] waypoints;

	public float moveSpeed = 5f;

	public float rotSpeed = 2f;

	public float minDistance = 0.1f;

	public bool isLooping = true;

	private int currentWaypointIndex;

	private float currentDistance;

	private void Update()
	{
		if (waypoints.Length == 0)
		{
			return;
		}
		Vector3 forward = waypoints[currentWaypointIndex].position - base.transform.position;
		Quaternion b = Quaternion.LookRotation(forward);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, Time.deltaTime * rotSpeed);
		currentDistance = Vector3.Distance(base.transform.position, waypoints[currentWaypointIndex].position);
		if (currentDistance < minDistance)
		{
			currentWaypointIndex++;
			if (currentWaypointIndex >= waypoints.Length)
			{
				if (isLooping)
				{
					currentWaypointIndex = 0;
				}
				else
				{
					base.enabled = false;
				}
			}
		}
		base.transform.position = Vector3.MoveTowards(base.transform.position, waypoints[currentWaypointIndex].position, Time.deltaTime * moveSpeed);
	}
}
