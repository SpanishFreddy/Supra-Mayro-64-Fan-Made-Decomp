using UnityEngine;
using UnityEngine.UI;

public class RedCoin : MonoBehaviour
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
		if (other.gameObject.CompareTag("RedCoin"))
		{
			other.gameObject.SetActive(false);
			count += 2;
			SetCountText();
		}
	}

	private void SetCountText()
	{
		countText.text = "Koynz: " + count;
		if (count >= 12)
		{
			winText.text = "You Win!";
		}
	}
}
