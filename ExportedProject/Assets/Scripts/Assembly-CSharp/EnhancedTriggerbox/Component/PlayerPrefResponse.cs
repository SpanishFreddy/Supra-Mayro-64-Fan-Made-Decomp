using System;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class PlayerPrefResponse : ResponseComponent
	{
		public enum ParameterType
		{
			Int = 0,
			Float = 1,
			String = 2
		}

		public string setPlayerPrefKey;

		public ParameterType setPlayerPrefType;

		public string setPlayerPrefVal;

		public override void Validation()
		{
			if (string.IsNullOrEmpty(setPlayerPrefKey) && !string.IsNullOrEmpty(setPlayerPrefVal))
			{
				ShowWarningMessage("You have entered a value to save to a player pref but you haven't specified which player pref to save it to!");
			}
			else if (!string.IsNullOrEmpty(setPlayerPrefKey) && string.IsNullOrEmpty(setPlayerPrefVal))
			{
				ShowWarningMessage("You have set the player pref key but the value to save in it is empty!");
			}
			if (string.IsNullOrEmpty(setPlayerPrefVal) || !(setPlayerPrefVal != "++") || !(setPlayerPrefVal != "--"))
			{
				return;
			}
			switch (setPlayerPrefType)
			{
			case ParameterType.Float:
			{
				float result2;
				if (!float.TryParse(setPlayerPrefVal, out result2))
				{
					ShowWarningMessage("Unable to parse player pref value to a float. Make sure you have entered a valid float.");
				}
				break;
			}
			case ParameterType.Int:
			{
				int result;
				if (!int.TryParse(setPlayerPrefVal, out result))
				{
					ShowWarningMessage("Unable to parse player pref value to a integer. Make sure you have entered a valid integer.");
				}
				break;
			}
			}
		}

		public override bool ExecuteAction()
		{
			if (!string.IsNullOrEmpty(setPlayerPrefKey))
			{
				switch (setPlayerPrefType)
				{
				case ParameterType.String:
					PlayerPrefs.SetString(setPlayerPrefKey, setPlayerPrefVal);
					break;
				case ParameterType.Int:
					if (setPlayerPrefVal == "++")
					{
						int @int = PlayerPrefs.GetInt(setPlayerPrefKey);
						@int++;
						PlayerPrefs.SetInt(setPlayerPrefKey, @int);
					}
					else if (setPlayerPrefVal == "--")
					{
						int int2 = PlayerPrefs.GetInt(setPlayerPrefKey);
						int2--;
						PlayerPrefs.SetInt(setPlayerPrefKey, int2);
					}
					else
					{
						PlayerPrefs.SetInt(setPlayerPrefKey, Convert.ToInt32(setPlayerPrefVal));
					}
					break;
				case ParameterType.Float:
					if (setPlayerPrefVal == "++")
					{
						float @float = PlayerPrefs.GetFloat(setPlayerPrefKey);
						@float += 1f;
						PlayerPrefs.SetFloat(setPlayerPrefKey, @float);
					}
					else if (setPlayerPrefVal == "--")
					{
						float float2 = PlayerPrefs.GetFloat(setPlayerPrefKey);
						float2 -= 1f;
						PlayerPrefs.SetFloat(setPlayerPrefKey, float2);
					}
					else
					{
						PlayerPrefs.SetFloat(setPlayerPrefKey, Convert.ToInt32(setPlayerPrefVal));
					}
					break;
				}
			}
			return true;
		}
	}
}
