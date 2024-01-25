using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class ModifyGameobjectResponse : ResponseComponent
	{
		public enum ModifyType
		{
			Destroy = 0,
			Disable = 1,
			Enable = 2,
			DisableComponent = 3,
			EnableComponent = 4
		}

		public enum ReferenceType
		{
			Null = 0,
			GameObjectReference = 1,
			GameObjectName = 2,
			CollisionGameObject = 3
		}

		public GameObject obj;

		public string gameObjectName;

		public ModifyType modifyType;

		public int selectedComponentIndex;

		public UnityEngine.Component selectedComponent;

		public int componentCount;

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
				if (!obj && !string.IsNullOrEmpty(gameObjectName))
				{
					referenceType = ReferenceType.GameObjectName;
				}
				else
				{
					referenceType = ReferenceType.GameObjectReference;
				}
			}
			if ((modifyType == ModifyType.DisableComponent || modifyType == ModifyType.EnableComponent) && referenceType != ReferenceType.GameObjectReference)
			{
				ShowWarningMessage("You cannot enable or disable a component on a gameobject without supplying a gameobject reference.");
			}
			if ((modifyType == ModifyType.DisableComponent || modifyType == ModifyType.EnableComponent) && referenceType == ReferenceType.GameObjectReference && componentCount == 0)
			{
				ShowWarningMessage("The  gameobject you've chosen to enable or disable a component on hasn't got any components attached to it that can be enabled or disabled.");
			}
		}

		public override bool ExecuteAction(GameObject collisionGameObject)
		{
			switch (referenceType)
			{
			case ReferenceType.Null:
				if (!obj && !string.IsNullOrEmpty(gameObjectName))
				{
					obj = GameObject.Find(gameObjectName);
				}
				break;
			case ReferenceType.CollisionGameObject:
				obj = collisionGameObject;
				break;
			case ReferenceType.GameObjectName:
				obj = GameObject.Find(gameObjectName);
				break;
			}
			if (!obj)
			{
				return false;
			}
			switch (modifyType)
			{
			case ModifyType.Destroy:
				Object.Destroy(obj);
				break;
			case ModifyType.Disable:
				obj.SetActive(false);
				break;
			case ModifyType.Enable:
				obj.SetActive(true);
				break;
			case ModifyType.DisableComponent:
				if (referenceType == ReferenceType.GameObjectReference)
				{
					PropertyInfo property2 = selectedComponent.GetType().GetProperty("enabled");
					if (property2 != null)
					{
						property2.SetValue(selectedComponent, false, null);
					}
					else
					{
						Debug.Log("ETB Error: Unable to disable component because the 'enabled' property could not be found.");
					}
				}
				break;
			case ModifyType.EnableComponent:
				if (referenceType == ReferenceType.GameObjectReference)
				{
					PropertyInfo property = selectedComponent.GetType().GetProperty("enabled");
					if (property != null)
					{
						property.SetValue(selectedComponent, true, null);
					}
					else
					{
						Debug.Log("ETB Error: Unable to enable component because the 'enabled' property could not be found.");
					}
				}
				break;
			}
			return true;
		}

		private List<UnityEngine.Component> GetObjectComponents()
		{
			List<UnityEngine.Component> list = new List<UnityEngine.Component>();
			UnityEngine.Component[] components = obj.GetComponents<UnityEngine.Component>();
			foreach (UnityEngine.Component component in components)
			{
				if (component.GetType().GetProperty("enabled") != null && !component.GetType().ToString().Contains("EnhancedTriggerbox.Component"))
				{
					list.Add(component);
				}
			}
			return list;
		}
	}
}
