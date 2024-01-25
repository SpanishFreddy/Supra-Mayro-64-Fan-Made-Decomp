using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSwim : MonoBehaviour
{
	public float swimSpeed = 5f;

	public float waterLevel;

	private bool isSwimming;

	private FirstPersonController fpsController;

	private void Start()
	{
		fpsController = GetComponent<FirstPersonController>();
	}

	private void Update()
	{
		if (base.transform.position.y < waterLevel)
		{
			isSwimming = true;
		}
		else
		{
			isSwimming = false;
		}
		if (isSwimming)
		{
			base.transform.Translate(Vector3.up * swimSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Water"))
		{
			waterLevel = other.gameObject.transform.position.y;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Water"))
		{
			waterLevel = 0f;
		}
	}
}
