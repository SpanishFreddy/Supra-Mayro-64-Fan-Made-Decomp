using UnityEngine;
using UnityEngine.UI;

public class Discord : MonoBehaviour
{
	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
	}

	public void OpenLink()
	{
		Application.OpenURL("https://discord.gg/aq9PBfv7xJ");
		Application.Quit();
	}
}
