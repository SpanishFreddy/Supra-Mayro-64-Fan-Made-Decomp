using UnityEngine;
using UnityEngine.UI;

public class BlueCoin : MonoBehaviour
{
	public float speed;

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
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		Vector3 vector = new Vector3(axis, 0f, axis2);
		rb.AddForce(vector * speed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("BlueCoin"))
		{
			other.gameObject.SetActive(false);
			count += 5;
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
