using UnityEngine;

public class LookAt : MonoBehaviour
{
	private Camera m_Camera;

	private void Start()
	{
		m_Camera = GameObject.Find("Camera").GetComponent<Camera>();
	}

	private void LateUpdate()
	{
		base.transform.LookAt(base.transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
	}
}
