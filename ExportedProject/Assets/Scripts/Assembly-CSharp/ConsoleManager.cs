using UnityEngine;
using UnityEngine.EventSystems;

public class ConsoleManager : MonoBehaviour
{
	public GameObject pausable;

	public Canvas pauseCanvas;

	private bool isPaused;

	private Animator anim;

	private Component[] pausableInterfaces;

	private Component[] quittableInterfaces;

	private void Start()
	{
		if (Object.FindObjectOfType<EventSystem>() == null)
		{
			GameObject gameObject = new GameObject("EventSystem", typeof(EventSystem));
			gameObject.AddComponent<StandaloneInputModule>();
		}
		pausableInterfaces = pausable.GetComponents(typeof(IPausable));
		quittableInterfaces = pausable.GetComponents(typeof(IQuittable));
		anim = pauseCanvas.GetComponent<Animator>();
		pauseCanvas.enabled = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (isPaused)
			{
				OnUnPause();
			}
			else
			{
				OnPause();
			}
		}
		pauseCanvas.enabled = isPaused;
		anim.SetBool("IsPaused", isPaused);
	}

	public void OnQuit()
	{
		Debug.Log("PauseManager.OnQuit");
		Component[] array = quittableInterfaces;
		foreach (Component component in array)
		{
			IQuittable quittable = (IQuittable)component;
			if (quittable != null)
			{
				quittable.OnQuit();
			}
		}
	}

	public void OnUnPause()
	{
		Debug.Log("PauseManager.OnUnPause");
		isPaused = false;
		Component[] array = pausableInterfaces;
		foreach (Component component in array)
		{
			IPausable pausable = (IPausable)component;
			if (pausable != null)
			{
				pausable.OnUnPause();
			}
		}
	}

	public void OnPause()
	{
		Debug.Log("PauseManager.OnPause");
		isPaused = true;
		Component[] array = pausableInterfaces;
		foreach (Component component in array)
		{
			IPausable pausable = (IPausable)component;
			if (pausable != null)
			{
				pausable.OnPause();
			}
		}
	}
}
