using UnityEngine;
using UnityEngine.AI;

public class FixedChase : MonoBehaviour
{
	[SerializeField]
	private Transform Player;

	private NavMeshAgent navmeshAgent;

	private void Awake()
	{
		navmeshAgent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		navmeshAgent.destination = Player.position;
	}
}
