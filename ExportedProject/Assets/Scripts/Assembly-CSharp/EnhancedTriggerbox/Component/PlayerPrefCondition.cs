using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class PlayerPrefCondition : ConditionComponent
	{
		public enum ParameterType
		{
			Int = 0,
			Float = 1,
			String = 2
		}

		public enum PrefCondition
		{
			GreaterThan = 0,
			GreaterThanOrEqualTo = 1,
			EqualTo = 2,
			LessThanOrEqualTo = 3,
			LessThan = 4
		}

		public PrefCondition playerPrefCondition;

		public string playerPrefKey;

		public ParameterType playerPrefType;

		public string playerPrefVal;

		public bool refreshEveryFrame = true;

		private float playerPrefFloat;

		private int playerPrefInt;

		private string playerPrefString;

		private float playerPrefValFloat;

		private int playerPrefValInt;

		public override void Validation()
		{
			if (string.IsNullOrEmpty(playerPrefKey))
			{
				ShowWarningMessage("You have set up a player pref condition but haven't entered a player pref key!");
			}
			if (playerPrefType == ParameterType.String && playerPrefCondition != PrefCondition.EqualTo)
			{
				ShowWarningMessage("You can only use the equal to Condition Type when the parameter is a string.");
			}
			if (string.IsNullOrEmpty(playerPrefVal))
			{
				ShowWarningMessage("You have set up a player pref condition but haven't entered a value to be compared against the player pref!");
			}
		}

		public override void OnAwake()
		{
			GetUpdatedPlayerPrefs();
			switch (playerPrefType)
			{
			case ParameterType.Float:
				float.TryParse(playerPrefVal, out playerPrefValFloat);
				break;
			case ParameterType.Int:
				int.TryParse(playerPrefVal, out playerPrefValInt);
				break;
			}
		}

		public override bool ExecuteAction()
		{
			if (refreshEveryFrame)
			{
				GetUpdatedPlayerPrefs();
			}
			if (!string.IsNullOrEmpty(playerPrefVal))
			{
				switch (playerPrefType)
				{
				case ParameterType.String:
					if (playerPrefVal == playerPrefString)
					{
						return true;
					}
					return false;
				case ParameterType.Float:
					switch (playerPrefCondition)
					{
					case PrefCondition.EqualTo:
						if (playerPrefFloat == playerPrefValFloat)
						{
							return true;
						}
						return false;
					case PrefCondition.GreaterThan:
						if (playerPrefFloat > playerPrefValFloat)
						{
							return true;
						}
						return false;
					case PrefCondition.GreaterThanOrEqualTo:
						if (playerPrefFloat >= playerPrefValFloat)
						{
							return true;
						}
						return false;
					case PrefCondition.LessThan:
						if (playerPrefFloat < playerPrefValFloat)
						{
							return true;
						}
						return false;
					case PrefCondition.LessThanOrEqualTo:
						if (playerPrefFloat <= playerPrefValFloat)
						{
							return true;
						}
						return false;
					}
					break;
				case ParameterType.Int:
					switch (playerPrefCondition)
					{
					case PrefCondition.EqualTo:
						if (playerPrefInt == playerPrefValInt)
						{
							return true;
						}
						return false;
					case PrefCondition.GreaterThan:
						if (playerPrefInt > playerPrefValInt)
						{
							return true;
						}
						return false;
					case PrefCondition.GreaterThanOrEqualTo:
						if (playerPrefInt >= playerPrefValInt)
						{
							return true;
						}
						return false;
					case PrefCondition.LessThan:
						if (playerPrefInt < playerPrefValInt)
						{
							return true;
						}
						return false;
					case PrefCondition.LessThanOrEqualTo:
						if (playerPrefInt <= playerPrefValInt)
						{
							return true;
						}
						return false;
					}
					break;
				}
			}
			return false;
		}

		private void GetUpdatedPlayerPrefs()
		{
			if (!string.IsNullOrEmpty(playerPrefVal) && !string.IsNullOrEmpty(playerPrefKey))
			{
				switch (playerPrefType)
				{
				case ParameterType.Float:
					playerPrefFloat = PlayerPrefs.GetFloat(playerPrefKey);
					break;
				case ParameterType.Int:
					playerPrefInt = PlayerPrefs.GetInt(playerPrefKey);
					break;
				case ParameterType.String:
					playerPrefString = PlayerPrefs.GetString(playerPrefKey);
					break;
				}
			}
		}
	}
}
