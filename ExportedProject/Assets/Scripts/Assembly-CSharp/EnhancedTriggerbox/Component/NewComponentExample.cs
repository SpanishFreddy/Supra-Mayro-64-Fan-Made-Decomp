using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class NewComponentExample : EnhancedTriggerBoxComponent
	{
		public GameObject exampleGameobject;

		public bool exampleBool;

		public int exampleInt;

		public override void DrawInspectorGUI()
		{
		}

		public override bool ExecuteAction()
		{
			return base.ExecuteAction();
		}

		public override void OnAwake()
		{
		}

		public override void Validation()
		{
			if (!exampleGameobject)
			{
				ShowWarningMessage("WARNING: You haven't assigned an object to exampleGameobject.");
			}
		}
	}
}
