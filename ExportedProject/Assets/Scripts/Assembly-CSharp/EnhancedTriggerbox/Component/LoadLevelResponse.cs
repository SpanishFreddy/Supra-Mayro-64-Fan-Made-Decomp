using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class LoadLevelResponse : ResponseComponent
	{
		public enum ResponseType
		{
			LoadScene = 0,
			UnloadScene = 1
		}

		public ResponseType responseType;

		public string loadLevelName;

		public int loadLevelNum;

		public bool async;

		public bool additive;

		public override bool ExecuteAction()
		{
			if (responseType == ResponseType.LoadScene)
			{
				if (async)
				{
					if (additive)
					{
						SceneManager.LoadSceneAsync(loadLevelName, LoadSceneMode.Additive);
					}
					else
					{
						SceneManager.LoadSceneAsync(loadLevelName, LoadSceneMode.Single);
					}
				}
				else if (additive)
				{
					SceneManager.LoadScene(loadLevelName, LoadSceneMode.Additive);
				}
				else
				{
					SceneManager.LoadScene(loadLevelName, LoadSceneMode.Single);
				}
			}
			else
			{
				SceneManager.UnloadSceneAsync(loadLevelName);
			}
			return true;
		}
	}
}
