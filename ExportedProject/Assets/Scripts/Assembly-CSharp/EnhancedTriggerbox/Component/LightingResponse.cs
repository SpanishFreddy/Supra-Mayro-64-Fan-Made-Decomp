using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class LightingResponse : ResponseComponent
	{
		public enum EditType
		{
			SingleLightSource = 0,
			SceneLighting = 1
		}

		public enum ChangeColourType
		{
			RemainTheSame = 0,
			EditColor = 1
		}

		public EditType editType;

		public Light targetLight;

		public ChangeColourType changeColourType;

		public Color setColour;

		public string setIntensity;

		public string setBounceIntensity;

		public string setRange;

		public Material setSkybox;

		public ChangeColourType changeAmbientLightColour;

		public Color ambientLightColour;

		public override void Validation()
		{
			float result;
			if (!float.TryParse(setIntensity, out result) && !string.IsNullOrEmpty(setIntensity))
			{
				ShowWarningMessage("Set intensity must be a valid float value.");
			}
			else if (result > 8f || result < 0f)
			{
				ShowWarningMessage("Set intensity must be between 0 and 8.");
			}
			if (!float.TryParse(setBounceIntensity, out result) && !string.IsNullOrEmpty(setBounceIntensity))
			{
				ShowWarningMessage("Set bounce intensity must be a valid float value.");
			}
			else if (result > 8f || result < 0f)
			{
				ShowWarningMessage("Set bounce intensity must be between 0 and 8.");
			}
			if (!float.TryParse(setRange, out result) && !string.IsNullOrEmpty(setRange) && (targetLight.type == LightType.Point || targetLight.type == LightType.Spot))
			{
				ShowWarningMessage("Set range must be a valid float value.");
			}
		}

		public override bool ExecuteAction()
		{
			if (editType == EditType.SingleLightSource)
			{
				if ((bool)targetLight)
				{
					if (duration != 0f)
					{
						activeCoroutines.Add(StartCoroutine(ChangeLightColourOverTime()));
					}
					else
					{
						if (changeColourType == ChangeColourType.EditColor)
						{
							targetLight.color = setColour;
						}
						if (!string.IsNullOrEmpty(setIntensity))
						{
							float result;
							if (float.TryParse(setIntensity, out result))
							{
								targetLight.intensity = result;
							}
							else
							{
								Debug.Log("Unable to parse the Set Intensity value to a float. Please make sure it's a valid float.");
							}
						}
						if (!string.IsNullOrEmpty(setBounceIntensity))
						{
							float result2;
							if (float.TryParse(setBounceIntensity, out result2))
							{
								targetLight.bounceIntensity = result2;
							}
							else
							{
								Debug.Log("Unable to parse the Set Bounce Intensity value to a float. Please make sure it's a valid float.");
							}
						}
						if (!string.IsNullOrEmpty(setRange) && (targetLight.type == LightType.Point || targetLight.type == LightType.Spot))
						{
							float result3;
							if (float.TryParse(setRange, out result3))
							{
								targetLight.range = result3;
							}
							else
							{
								Debug.Log("Unable to parse the Set Range value to a float. Please make sure it's a valid float.");
							}
						}
					}
				}
				else
				{
					Debug.Log("Unable to modify a light because the Target Light reference hasn't been set.");
				}
			}
			else if (duration != 0f)
			{
				activeCoroutines.Add(StartCoroutine(ChangeSceneLightingOverTime()));
			}
			else
			{
				if (setSkybox != null)
				{
					RenderSettings.skybox = setSkybox;
				}
				if (changeAmbientLightColour == ChangeColourType.EditColor)
				{
					RenderSettings.ambientLight = ambientLightColour;
				}
			}
			return false;
		}

		private IEnumerator ChangeLightColourOverTime()
		{
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / duration;
			float targetIntensity;
			float.TryParse(setIntensity, out targetIntensity);
			float targetBounceIntensity;
			float.TryParse(setBounceIntensity, out targetBounceIntensity);
			float targetRange;
			float.TryParse(setRange, out targetRange);
			while (progress < 1f)
			{
				if (changeColourType == ChangeColourType.EditColor)
				{
					targetLight.color = Color.Lerp(targetLight.color, setColour, progress);
				}
				if (!string.IsNullOrEmpty(setIntensity))
				{
					targetLight.intensity = Mathf.Lerp(targetLight.intensity, targetIntensity, progress);
				}
				if (!string.IsNullOrEmpty(setBounceIntensity))
				{
					targetLight.bounceIntensity = Mathf.Lerp(targetLight.bounceIntensity, targetBounceIntensity, progress);
				}
				if (!string.IsNullOrEmpty(setRange) && (targetLight.type == LightType.Point || targetLight.type == LightType.Spot))
				{
					targetLight.range = Mathf.Lerp(targetLight.range, targetRange, progress);
				}
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}

		private IEnumerator ChangeSceneLightingOverTime()
		{
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / (duration * 10f);
			while (progress < 1f)
			{
				if (setSkybox != null)
				{
					RenderSettings.skybox.Lerp(RenderSettings.skybox, setSkybox, progress);
				}
				if (changeAmbientLightColour == ChangeColourType.EditColor)
				{
					RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, ambientLightColour, progress);
				}
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}
	}
}
