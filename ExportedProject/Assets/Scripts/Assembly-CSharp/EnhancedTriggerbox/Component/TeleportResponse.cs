using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class TeleportResponse : ResponseComponent
	{
		public enum ReferenceType
		{
			GameObjectReference = 0,
			GameObjectName = 1,
			CollisionGameObject = 2
		}

		public GameObject targetObject;

		public string targetGameObjectName;

		public Transform destination;

		public bool copyRotation;

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
			switch (referenceType)
			{
			case ReferenceType.GameObjectReference:
				if ((bool)targetObject && !destination)
				{
					ShowWarningMessage("You have added a gameobject but haven't supplied where to move it to.");
				}
				if (!targetObject && (bool)destination)
				{
					ShowWarningMessage("You have added a destination but you haven't supplied the gameobject that will be moved there.");
				}
				break;
			case ReferenceType.GameObjectName:
				if (!string.IsNullOrEmpty(targetGameObjectName) && !destination)
				{
					ShowWarningMessage("You have added a gameobject name but haven't supplied where to move it to.");
				}
				if (string.IsNullOrEmpty(targetGameObjectName) && (bool)destination)
				{
					ShowWarningMessage("You have added a destination but you haven't supplied the gameobject name that will be moved there.");
				}
				break;
			}
			ShowWarningMessage("This component has been deprecated and replaced with the ModifyTransform component. Please use that instead.");
		}

		public override bool ExecuteAction(GameObject collisionGameObject)
		{
			switch (referenceType)
			{
			case ReferenceType.CollisionGameObject:
				targetObject = collisionGameObject;
				break;
			case ReferenceType.GameObjectName:
				targetObject = GameObject.Find(targetGameObjectName);
				break;
			}
			if ((bool)targetObject && (bool)destination)
			{
				targetObject.transform.position = destination.position;
				if (copyRotation)
				{
					targetObject.transform.rotation = destination.rotation;
				}
			}
			return true;
		}
	}
}
