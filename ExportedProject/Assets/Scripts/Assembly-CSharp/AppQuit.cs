using UnityEngine;

public class AppQuit : MonoBehaviour, IQuittable
{
	public void OnQuit()
	{
		Debug.Log("AppQuit.Quit");
		Application.Quit();
	}
}
