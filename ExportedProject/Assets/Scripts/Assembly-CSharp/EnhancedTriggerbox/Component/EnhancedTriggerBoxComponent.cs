using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[Serializable]
	[HideInInspector]
	[AddComponentMenu("")]
	public abstract class EnhancedTriggerBoxComponent : MonoBehaviour
	{
		protected bool hideShowSection = true;

		public bool deleted;

		public bool showWarnings = true;

		public float duration;

		protected List<Coroutine> activeCoroutines = new List<Coroutine>();

		public virtual bool requiresCollisionObjectData { get; protected set; }

		public void OnInspectorGUI()
		{
		}

		public virtual void OnAwake()
		{
		}

		public virtual void DrawInspectorGUI()
		{
			List<FieldInfo> list = (from field in GetType().GetFields()
				select (field)).ToList();
			foreach (FieldInfo item in list)
			{
				if (item.Name != "showWarnings" && item.Name != "deleted" && item.Name != "hideShowSection")
				{
					RenderGeneric(item);
				}
			}
		}

		public virtual void ResetComponent()
		{
		}

		public void EndComponentCoroutine()
		{
			for (int num = activeCoroutines.Count - 1; num >= 0; num--)
			{
				if (activeCoroutines[num] != null)
				{
					StopCoroutine(activeCoroutines[num]);
				}
				activeCoroutines.RemoveAt(num);
			}
		}

		public virtual bool ExecuteAction()
		{
			return false;
		}

		public virtual bool ExecuteAction(GameObject collidingObject)
		{
			return false;
		}

		public virtual void Validation()
		{
		}

		protected void RenderGeneric(FieldInfo o)
		{
		}

		protected bool RenderHeader(string s, bool optionRef, bool bold = true, bool topspace = false)
		{
			return true;
		}

		protected void RenderDivider()
		{
		}

		protected void ShowWarningMessage(string message)
		{
		}
	}
}
