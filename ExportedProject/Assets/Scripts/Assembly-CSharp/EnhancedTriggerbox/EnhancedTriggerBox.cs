using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnhancedTriggerbox.Component;
using UnityEngine;

namespace EnhancedTriggerbox
{
	[Serializable]
	[RequireComponent(typeof(BoxCollider))]
	[DisallowMultipleComponent]
	[HelpURL("https://alex-scott.co.uk/portfolio/enhanced-trigger-box.html")]
	public class EnhancedTriggerBox : MonoBehaviour
	{
		public enum AfterTriggerOptions
		{
			SetInactive = 0,
			DestroyTriggerBox = 1,
			DestroyParent = 2,
			DoNothing = 3,
			ExecuteExitResponses = 4
		}

		public enum TriggerFollow
		{
			Static = 0,
			FollowMainCamera = 1,
			FollowTransform = 2
		}

		[SerializeField]
		private List<EnhancedTriggerBoxComponent> conditions = new List<EnhancedTriggerBoxComponent>();

		[SerializeField]
		private List<EnhancedTriggerBoxComponent> responses = new List<EnhancedTriggerBoxComponent>();

		[SerializeField]
		private List<EnhancedTriggerBoxComponent> exitResponses = new List<EnhancedTriggerBoxComponent>();

		public bool showBaseOptions = true;

		[Tooltip("If true, the script will write to the console when certain events happen such as when the trigger box is triggered.")]
		public bool debugTriggerBox;

		[Tooltip("If this is true, you won't see any warnings in the editor when you're missing references or if there's something which could cause an error.")]
		public bool hideWarnings;

		[Tooltip("If true, the entry check on the trigger box will be disabled, meaning it will go straight to the condition checking instead of waiting for something to enter the trigger box.")]
		public bool disableEntryCheck;

		[Tooltip("Only gameobjects with tags listed here are able to trigger the trigger box. To have more than one tag, put a comma between them. If you leave this field blank any object will be able to trigger it.")]
		public string triggerTags;

		[Tooltip("If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.")]
		public bool canWander;

		[Tooltip("This is the colour the trigger box and it's edges will have in the editor.")]
		public Color triggerboxColour;

		[Tooltip("This allows you to choose what happens to this gameobject after the trigger box has been triggered. Set Inactive will set this gameobject as inactive. Destroy trigger box will destroy this gameobject. Destroy parent will destroy this gameobject's parent. Do Nothing will mean the trigger box will stay active and continue to operate. ExecuteExitResponses allows you to set up additional responses to be executed after the object that entered the trigger box leaves it.")]
		public AfterTriggerOptions afterTrigger;

		[Tooltip("This allows you to choose if you want your trigger box to stay positioned on a moving transform or the main camera. If you pick Follow Transform a field will appear to set which transform you want the trigger box to follow. Or if you pick Follow Main Camera the trigger box will stay positioned on wherever the main camera currently is.")]
		public TriggerFollow triggerFollow;

		[Tooltip("This is the total time that the conditions must be met for in seconds before the responses get executed.")]
		public float conditionTime;

		[Tooltip("This is used when Trigger Follow is set to Follow Transform. The trigger box will stay positioned on wherever this transform is currently positioned.")]
		public Transform followTransform;

		[Tooltip("This is can be used if you cannot get a reference for the above Follow Transform transform. GameObject.Find() will be used to find the gameobject and transform with the name you enter.")]
		public string followTransformName;

		private bool triggered;

		private bool responsesExecuting;

		private float conditionTimer;

		private GameObject collidingObject;

		private Collider etbCollider;

		public List<EnhancedTriggerBoxComponent> Conditions
		{
			get
			{
				return conditions;
			}
		}

		public List<EnhancedTriggerBoxComponent> Responses
		{
			get
			{
				return responses;
			}
		}

		public List<EnhancedTriggerBoxComponent> ExitResponses
		{
			get
			{
				return exitResponses;
			}
		}

		public void OnInspectorGUI()
		{
		}

		private void Start()
		{
			if (disableEntryCheck)
			{
				triggered = true;
			}
			else if (!string.IsNullOrEmpty(followTransformName))
			{
				try
				{
					followTransform = GameObject.Find(followTransformName).transform;
				}
				catch
				{
					Debug.Log("Unable to find game object" + followTransformName + " for Trigger Follow. Reverting to static.");
					triggerFollow = TriggerFollow.Static;
				}
			}
			for (int num = conditions.Count - 1; num >= 0; num--)
			{
				if ((bool)conditions[num])
				{
					conditions[num].OnAwake();
				}
				else
				{
					conditions.RemoveAt(num);
				}
			}
			for (int num2 = responses.Count - 1; num2 >= 0; num2--)
			{
				if ((bool)responses[num2])
				{
					responses[num2].OnAwake();
				}
				else
				{
					responses.RemoveAt(num2);
				}
			}
		}

		private void Update()
		{
			if (!disableEntryCheck)
			{
				switch (triggerFollow)
				{
				case TriggerFollow.FollowTransform:
					base.transform.position = followTransform.position;
					break;
				case TriggerFollow.FollowMainCamera:
					base.transform.position = Camera.main.transform.position;
					break;
				}
			}
			if (!triggered || responsesExecuting)
			{
				return;
			}
			bool flag = true;
			for (int num = conditions.Count - 1; num >= 0; num--)
			{
				if ((bool)conditions[num])
				{
					flag = ((!conditions[num].requiresCollisionObjectData) ? conditions[num].ExecuteAction() : conditions[num].ExecuteAction(collidingObject));
					if (!flag)
					{
						break;
					}
				}
				else
				{
					conditions.RemoveAt(num);
				}
			}
			if (flag && CheckConditionTimer())
			{
				StartCoroutine(ConditionsMet());
			}
		}

		private IEnumerator ConditionsMet()
		{
			responsesExecuting = true;
			if (debugTriggerBox)
			{
				Debug.Log(base.gameObject.name + " has been triggered!");
			}
			if (afterTrigger == AfterTriggerOptions.ExecuteExitResponses)
			{
				for (int num = exitResponses.Count - 1; num >= 0; num--)
				{
					exitResponses[num].EndComponentCoroutine();
				}
			}
			float waitTime = 0f;
			for (int num2 = responses.Count - 1; num2 >= 0; num2--)
			{
				if ((bool)responses[num2])
				{
					if (responses[num2].requiresCollisionObjectData)
					{
						responses[num2].ExecuteAction(collidingObject);
					}
					else
					{
						responses[num2].ExecuteAction();
					}
					if (responses[num2].duration > waitTime)
					{
						waitTime = responses[num2].duration;
					}
				}
				else
				{
					responses.RemoveAt(num2);
				}
			}
			if (afterTrigger == AfterTriggerOptions.DestroyParent || afterTrigger == AfterTriggerOptions.DestroyTriggerBox || afterTrigger == AfterTriggerOptions.SetInactive)
			{
				yield return new WaitForSeconds(waitTime);
			}
			switch (afterTrigger)
			{
			case AfterTriggerOptions.SetInactive:
				base.gameObject.SetActive(false);
				break;
			case AfterTriggerOptions.DestroyTriggerBox:
				UnityEngine.Object.Destroy(base.gameObject);
				break;
			case AfterTriggerOptions.DestroyParent:
				UnityEngine.Object.Destroy(base.transform.parent.gameObject);
				break;
			case AfterTriggerOptions.DoNothing:
			{
				conditionTimer = 0f;
				responsesExecuting = false;
				if (!disableEntryCheck)
				{
					triggered = false;
				}
				for (int i = 0; i < conditions.Count; i++)
				{
					conditions[i].ResetComponent();
				}
				for (int j = 0; j < responses.Count; j++)
				{
					responses[j].ResetComponent();
				}
				break;
			}
			}
		}

		private void OnExitResponses()
		{
			for (int num = responses.Count - 1; num >= 0; num--)
			{
				responses[num].EndComponentCoroutine();
			}
			for (int num2 = responses.Count - 1; num2 >= 0; num2--)
			{
				responses[num2].ResetComponent();
			}
			for (int num3 = conditions.Count - 1; num3 >= 0; num3--)
			{
				conditions[num3].ResetComponent();
			}
			for (int num4 = exitResponses.Count - 1; num4 >= 0; num4--)
			{
				if (exitResponses[num4].requiresCollisionObjectData)
				{
					exitResponses[num4].ExecuteAction(collidingObject);
				}
				else
				{
					exitResponses[num4].ExecuteAction();
				}
			}
			responsesExecuting = false;
			triggered = false;
			conditionTimer = 0f;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!disableEntryCheck && (string.IsNullOrEmpty(triggerTags) || triggerTags.Split(',').Contains(other.gameObject.tag)))
			{
				triggered = true;
				collidingObject = other.gameObject;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!(collidingObject == other.gameObject))
			{
				return;
			}
			if (afterTrigger == AfterTriggerOptions.ExecuteExitResponses)
			{
				if (responsesExecuting)
				{
					OnExitResponses();
				}
			}
			else if (!canWander)
			{
				triggered = false;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = triggerboxColour;
			if (!disableEntryCheck)
			{
				if (etbCollider == null)
				{
					etbCollider = GetComponent<Collider>();
				}
				Gizmos.DrawCube(new Vector3(etbCollider.bounds.center.x, etbCollider.bounds.center.y, etbCollider.bounds.center.z), new Vector3(etbCollider.bounds.size.x, etbCollider.bounds.size.y, etbCollider.bounds.size.z));
			}
		}

		private bool CheckConditionTimer()
		{
			if (conditionTimer >= conditionTime)
			{
				return true;
			}
			conditionTimer += Time.deltaTime;
			return false;
		}

		public static string AddSpacesToSentence(string text, bool preserveAcronyms)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length * 2);
			stringBuilder.Append(text[0]);
			for (int i = 1; i < text.Length; i++)
			{
				if (char.IsUpper(text[i]) && ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) || (preserveAcronyms && char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1]))))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(text[i]);
			}
			return stringBuilder.ToString();
		}
	}
}
