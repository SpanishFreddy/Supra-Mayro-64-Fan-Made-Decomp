using UnityEngine;
using UnityEngine.UI;

namespace SpeedTutorMainMenuSystem
{
	public class Init_LoadPreferences : MonoBehaviour
	{
		[Space(20f)]
		[SerializeField]
		private Brightness brightnessEffect;

		[SerializeField]
		private Text brightnessText;

		[SerializeField]
		private Slider brightnessSlider;

		[Space(20f)]
		[SerializeField]
		private Text volumeText;

		[SerializeField]
		private Slider volumeSlider;

		[Space(20f)]
		[SerializeField]
		private Text controllerText;

		[SerializeField]
		private Slider controllerSlider;

		[Space(20f)]
		[SerializeField]
		private Toggle invertYToggle;

		[Space(20f)]
		[SerializeField]
		private bool canUse;

		[SerializeField]
		private MenuController menuController;

		private void Awake()
		{
			Debug.Log("Loading player prefs test");
			if (!canUse)
			{
				return;
			}
			if (brightnessEffect != null)
			{
				if (PlayerPrefs.HasKey("masterBrightness"))
				{
					float @float = PlayerPrefs.GetFloat("masterBrightness");
					brightnessText.text = @float.ToString("0.0");
					brightnessSlider.value = @float;
					brightnessEffect.brightness = @float;
				}
				else
				{
					menuController.ResetButton("Brightness");
				}
			}
			if (PlayerPrefs.HasKey("masterVolume"))
			{
				float float2 = PlayerPrefs.GetFloat("masterVolume");
				volumeText.text = float2.ToString("0.0");
				volumeSlider.value = float2;
				AudioListener.volume = float2;
			}
			else
			{
				menuController.ResetButton("Audio");
			}
			if (PlayerPrefs.HasKey("masterSen"))
			{
				float float3 = PlayerPrefs.GetFloat("masterSen");
				controllerText.text = float3.ToString("0");
				controllerSlider.value = float3;
				menuController.controlSenFloat = float3;
			}
			else
			{
				menuController.ResetButton("Graphics");
			}
			if (PlayerPrefs.HasKey("masterInvertY"))
			{
				if (PlayerPrefs.GetInt("masterInvertY") == 1)
				{
					invertYToggle.isOn = true;
				}
				else
				{
					invertYToggle.isOn = false;
				}
			}
		}
	}
}
