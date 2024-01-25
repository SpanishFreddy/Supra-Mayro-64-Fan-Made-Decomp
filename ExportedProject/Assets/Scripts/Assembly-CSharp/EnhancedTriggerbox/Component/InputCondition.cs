using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class InputCondition : ConditionComponent
	{
		public enum TriggerType
		{
			OnPressed = 0,
			OnReleased = 1
		}

		public KeyCode inputKey;

		public TriggerType triggerType;

		private bool triggered;

		[SerializeField]
		private EnhancedTriggerBox etb;

		public override void Validation()
		{
			if (triggerType == TriggerType.OnReleased)
			{
				if (etb == null)
				{
					etb = GetComponent<EnhancedTriggerBox>();
				}
				if (etb.conditionTime > 0f)
				{
					ShowWarningMessage("Using a Condition Timer with the OnReleased trigger type will have no effect! This is because it is impossible to release a key for a certain duration.");
				}
			}
		}

		public override bool ExecuteAction()
		{
			switch (triggerType)
			{
			case TriggerType.OnPressed:
				if (Input.GetKey(inputKey))
				{
					return true;
				}
				break;
			case TriggerType.OnReleased:
				if (triggered)
				{
					return true;
				}
				if (Input.GetKeyUp(inputKey))
				{
					triggered = true;
					return true;
				}
				break;
			}
			return false;
		}

		public override void ResetComponent()
		{
			triggered = false;
		}
	}
}
