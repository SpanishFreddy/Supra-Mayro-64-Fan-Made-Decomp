using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class ModifyMaterialResponse : ResponseComponent
	{
		public enum AffectOthers
		{
			Self = 0,
			SelfAndChildren = 1,
			SelfAndParents = 2,
			SelfChildrenAndParents = 3,
			Children = 4,
			Parents = 5,
			ChildrenAndParents = 6
		}

		public enum ReferenceType
		{
			GameObjectReference = 0,
			GameObjectName = 1,
			CollisionGameObject = 2
		}

		public GameObject targetGameObject;

		public string targetGameObjectName;

		public Material material;

		public AffectOthers affectOthers;

		public ReferenceType referenceType;

		public override bool requiresCollisionObjectData
		{
			get
			{
				return true;
			}
		}

		public override bool ExecuteAction(GameObject collisionGameObject)
		{
			switch (referenceType)
			{
			case ReferenceType.CollisionGameObject:
				targetGameObject = collisionGameObject;
				break;
			case ReferenceType.GameObjectName:
				targetGameObject = GameObject.Find(targetGameObjectName);
				break;
			}
			if (targetGameObject == null || material == null)
			{
				Debug.Log("Unable to execute Modify Material Response. Missing gameobject or material reference!");
				return true;
			}
			switch (affectOthers)
			{
			case AffectOthers.Self:
				AffectSelf();
				break;
			case AffectOthers.Children:
				AffectChildren();
				break;
			case AffectOthers.Parents:
				AffectParents();
				break;
			case AffectOthers.ChildrenAndParents:
				AffectChildren();
				AffectParents();
				break;
			case AffectOthers.SelfAndChildren:
				AffectSelf();
				AffectChildren();
				break;
			case AffectOthers.SelfAndParents:
				AffectSelf();
				AffectParents();
				break;
			case AffectOthers.SelfChildrenAndParents:
				AffectSelf();
				AffectChildren();
				AffectParents();
				break;
			}
			return true;
		}

		private void AffectSelf()
		{
			targetGameObject.GetComponent<MeshRenderer>().material = material;
		}

		private void AffectChildren()
		{
			MeshRenderer[] componentsInChildren = targetGameObject.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = material;
			}
		}

		private void AffectParents()
		{
			MeshRenderer[] componentsInParent = targetGameObject.GetComponentsInParent<MeshRenderer>();
			for (int i = 0; i < componentsInParent.Length; i++)
			{
				componentsInParent[i].material = material;
			}
		}

		public override void Validation()
		{
			if (!targetGameObject && (bool)material)
			{
				ShowWarningMessage("You need to add a reference to a target gameobject for the modify material response to work.");
			}
			if (!material && (bool)targetGameObject)
			{
				ShowWarningMessage("You need to add a reference to a material for the modify material response to work.");
			}
		}
	}
}
