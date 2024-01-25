using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class SendMessageResponse : ResponseComponent
	{
		public enum ParameterType
		{
			Int = 0,
			Float = 1,
			String = 2
		}

		public enum ReferenceType
		{
			Null = 0,
			GameObjectReference = 1,
			GameObjectName = 2,
			CollisionGameObject = 3
		}

		public GameObject messageTarget;

		public string messageTargetName;

		public string messageFunctionName;

		public ParameterType parameterType;

		public string parameterValue;

		public ReferenceType referenceType;

		public override bool requiresCollisionObjectData
		{
			get
			{
				return true;
			}
		}

		public override void Validation()
		{
			if (referenceType == ReferenceType.Null)
			{
				if (!messageTarget && !string.IsNullOrEmpty(messageTargetName))
				{
					referenceType = ReferenceType.GameObjectName;
				}
				else
				{
					referenceType = ReferenceType.GameObjectReference;
				}
			}
			if ((bool)messageTarget && string.IsNullOrEmpty(messageFunctionName))
			{
				ShowWarningMessage("You have selected a object for the message to be sent to but haven't specified which function to call!");
			}
			if (!messageTarget && !string.IsNullOrEmpty(messageFunctionName))
			{
				ShowWarningMessage("You have entered a function to call but haven't specified the object to send it to!");
			}
			if ((bool)messageTarget && !string.IsNullOrEmpty(messageFunctionName) && string.IsNullOrEmpty(parameterValue))
			{
				ShowWarningMessage("You have entered a function and gameobject to send a message to but the message has no value!");
			}
			if ((bool)messageTarget && !string.IsNullOrEmpty(messageTargetName))
			{
				ShowWarningMessage("You cannot input a gameobject reference and a gameobject name. Please remove one or the other.");
			}
			float result2;
			if (parameterType == ParameterType.Int && !string.IsNullOrEmpty(parameterValue))
			{
				int result;
				if (!int.TryParse(parameterValue, out result))
				{
					ShowWarningMessage("The message value you have entered is not a valid int. Please make sure you enter a valid int for the message value.");
				}
			}
			else if (parameterType == ParameterType.Float && !string.IsNullOrEmpty(parameterValue) && !float.TryParse(parameterValue, out result2))
			{
				ShowWarningMessage("The message value you have entered is not a valid float. Please make sure you enter a valid float for the message value.");
			}
		}

		public override bool ExecuteAction(GameObject collisionGameObject)
		{
			switch (referenceType)
			{
			case ReferenceType.Null:
				if (!messageTarget && !string.IsNullOrEmpty(messageTargetName))
				{
					messageTarget = GameObject.Find(messageTargetName);
				}
				break;
			case ReferenceType.CollisionGameObject:
				messageTarget = collisionGameObject;
				break;
			case ReferenceType.GameObjectName:
				messageTarget = GameObject.Find(messageTargetName);
				break;
			}
			if ((bool)messageTarget)
			{
				if (parameterValue != string.Empty)
				{
					switch (parameterType)
					{
					case ParameterType.Int:
						messageTarget.SendMessage(messageFunctionName, int.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
						break;
					case ParameterType.Float:
						messageTarget.SendMessage(messageFunctionName, float.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
						break;
					case ParameterType.String:
						messageTarget.SendMessage(messageFunctionName, parameterValue, SendMessageOptions.DontRequireReceiver);
						break;
					}
				}
				else
				{
					messageTarget.SendMessage(messageFunctionName, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				Debug.Log("Unable to execute Send Message Response. Gameobject not found!");
			}
			return true;
		}
	}
}
