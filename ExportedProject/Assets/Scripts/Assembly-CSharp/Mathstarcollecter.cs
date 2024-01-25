using UnityEngine;
using UnityEngine.UI;

public class Mathstarcollecter : MonoBehaviour
{
	public Text countText;

	public Text winText;

	private Rigidbody rb;

	private int count;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText();
		winText.text = string.Empty;
	}

	private void FixedUpdate()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Mathstar"))
		{
			other.gameObject.SetActive(false);
			count++;
			SetCountText();
		}
	}

	private void SetCountText()
	{
		countText.text = "Mathfsterz: " + count;
		if (count >= 7)
		{
			winText.text = "WOAHH! NAU GU TU DE KESTLE!";
		}
	}
}
