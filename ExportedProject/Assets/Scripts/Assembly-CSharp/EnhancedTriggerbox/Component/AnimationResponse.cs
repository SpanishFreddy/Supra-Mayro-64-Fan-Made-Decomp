using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class AnimationResponse : ResponseComponent
	{
		public enum ReferenceType
		{
			GameObjectReference = 0,
			GameObjectName = 1,
			CollisionGameObject = 2
		}

		public GameObject animationTarget;

		public string targetGameObjectName;

		public string setMecanimTrigger;

		public bool stopAnim;

		public AnimationClip animationClip;

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
			if (!string.IsNullOrEmpty(setMecanimTrigger) && animationTarget == null)
			{
				ShowWarningMessage("You have set a Mecanim Trigger as an Animation Response but haven't set an Animation Target to apply it to!");
			}
			if (stopAnim && animationTarget == null)
			{
				ShowWarningMessage("You have set Stop Animation as an Animation Response but haven't set an Animation Target to apply it to!");
			}
			if (animationClip != null && animationTarget == null)
			{
				ShowWarningMessage("You have chosen to play a legacy animation as an Animation Response but haven't set an Animation Target to apply it to!");
			}
		}

		public override bool ExecuteAction(GameObject collisionGameObject)
		{
			switch (referenceType)
			{
			case ReferenceType.CollisionGameObject:
				animationTarget = collisionGameObject;
				break;
			case ReferenceType.GameObjectName:
				animationTarget = GameObject.Find(targetGameObjectName);
				break;
			}
			if (stopAnim && (bool)animationTarget)
			{
				animationTarget.GetComponent<Animator>().StopPlayback();
			}
			if ((bool)animationClip && (bool)animationTarget)
			{
				animationTarget.GetComponent<Animation>().CrossFade(animationClip.name, 0.3f, PlayMode.StopAll);
			}
			if (!string.IsNullOrEmpty(setMecanimTrigger) && (bool)animationTarget)
			{
				animationTarget.GetComponent<Animator>().SetTrigger(setMecanimTrigger);
			}
			return true;
		}
	}
}
