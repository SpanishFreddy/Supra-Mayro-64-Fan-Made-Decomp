using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
	private Button button;

	private void Start()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(QuitGame);
	}

	private void QuitGame()
	{
		Application.Quit();
	}
}
