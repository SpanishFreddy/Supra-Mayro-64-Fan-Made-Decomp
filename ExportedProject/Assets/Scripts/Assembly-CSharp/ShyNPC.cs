using UnityEngine;

public class ShyNPC : MonoBehaviour
{
	public Transform Player;

	private int MoveSpeed = 4;

	private int MaxDist = 60;

	private int MinDist = 15;

	private void Start()
	{
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, Player.position) >= (float)MinDist)
		{
			base.transform.position += base.transform.forward * MoveSpeed * Time.deltaTime;
			if (!(Vector3.Distance(base.transform.position, Player.position) <= (float)MaxDist))
			{
			}
		}
	}
}
