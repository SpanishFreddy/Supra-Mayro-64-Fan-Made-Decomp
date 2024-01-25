using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	public class ModifyTransformResponse : ResponseComponent
	{
		public enum SelectAttribute
		{
			Position = 0,
			Rotation = 1,
			Scale = 2
		}

		public enum Axis
		{
			X = 0,
			Y = 1,
			Z = 2
		}

		public enum ReferenceType
		{
			TransformReference = 0,
			TransformName = 1,
			CollisionTransform = 2
		}

		public enum ValueType
		{
			Set = 0,
			Additive = 1
		}

		public Transform targetTransform;

		public string targetTransformName;

		public SelectAttribute targetAttribute;

		public Axis targetAxis;

		public bool localSpace;

		public float targetValue;

		public ReferenceType referenceType;

		public ValueType valueType;

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
			case ReferenceType.CollisionTransform:
				targetTransform = collisionGameObject.transform;
				break;
			case ReferenceType.TransformName:
				targetTransform = GameObject.Find(targetTransformName).transform;
				break;
			}
			if (!targetTransform)
			{
				Debug.LogError("Error in ModifyTransformResponse. Unable to retrieve the transform to modify.");
				return false;
			}
			if (duration != 0f)
			{
				activeCoroutines.Add(StartCoroutine(ChangeAttributeOverTime()));
			}
			else
			{
				float value = ((valueType != ValueType.Additive) ? targetValue : (GetStartValue() + targetValue));
				SetValue(value);
			}
			return false;
		}

		private IEnumerator ChangeAttributeOverTime()
		{
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / duration;
			float startValue = GetStartValue();
			float endValue = ((valueType != ValueType.Additive) ? targetValue : (GetStartValue() + targetValue));
			while (progress < 1f)
			{
				SetValue(Mathf.Lerp(startValue, endValue, progress));
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}

		private float GetStartValue()
		{
			switch (targetAttribute)
			{
			case SelectAttribute.Position:
				switch (targetAxis)
				{
				case Axis.X:
					if (localSpace)
					{
						return targetTransform.localPosition.x;
					}
					return targetTransform.position.x;
				case Axis.Y:
					if (localSpace)
					{
						return targetTransform.localPosition.y;
					}
					return targetTransform.position.y;
				case Axis.Z:
					if (localSpace)
					{
						return targetTransform.localPosition.z;
					}
					return targetTransform.position.z;
				}
				break;
			case SelectAttribute.Rotation:
				switch (targetAxis)
				{
				case Axis.X:
					if (localSpace)
					{
						return targetTransform.localEulerAngles.x;
					}
					return targetTransform.eulerAngles.x;
				case Axis.Y:
					if (localSpace)
					{
						return targetTransform.localEulerAngles.y;
					}
					return targetTransform.eulerAngles.y;
				case Axis.Z:
					if (localSpace)
					{
						return targetTransform.localEulerAngles.z;
					}
					return targetTransform.eulerAngles.z;
				}
				break;
			case SelectAttribute.Scale:
				switch (targetAxis)
				{
				case Axis.X:
					if (localSpace)
					{
						return targetTransform.localScale.x;
					}
					return targetTransform.lossyScale.x;
				case Axis.Y:
					if (localSpace)
					{
						return targetTransform.localScale.y;
					}
					return targetTransform.lossyScale.y;
				case Axis.Z:
					if (localSpace)
					{
						return targetTransform.localScale.z;
					}
					return targetTransform.lossyScale.z;
				}
				break;
			}
			return 0f;
		}

		private void SetValue(float value)
		{
			switch (targetAttribute)
			{
			case SelectAttribute.Position:
				switch (targetAxis)
				{
				case Axis.X:
					if (localSpace)
					{
						targetTransform.localPosition = new Vector3(value, targetTransform.localPosition.y, targetTransform.localPosition.z);
					}
					else
					{
						targetTransform.position = new Vector3(value, targetTransform.position.y, targetTransform.position.z);
					}
					break;
				case Axis.Y:
					if (localSpace)
					{
						targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, value, targetTransform.localPosition.z);
					}
					else
					{
						targetTransform.position = new Vector3(targetTransform.position.x, value, targetTransform.position.z);
					}
					break;
				case Axis.Z:
					if (localSpace)
					{
						targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, targetTransform.localPosition.y, value);
					}
					else
					{
						targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, value);
					}
					break;
				}
				break;
			case SelectAttribute.Rotation:
				switch (targetAxis)
				{
				case Axis.X:
					if (localSpace)
					{
						targetTransform.localEulerAngles = new Vector3(value, targetTransform.localEulerAngles.y, targetTransform.localEulerAngles.z);
					}
					else
					{
						targetTransform.eulerAngles = new Vector3(value, targetTransform.eulerAngles.y, targetTransform.eulerAngles.z);
					}
					break;
				case Axis.Y:
					if (localSpace)
					{
						targetTransform.localEulerAngles = new Vector3(targetTransform.localEulerAngles.x, value, targetTransform.localEulerAngles.z);
					}
					else
					{
						targetTransform.eulerAngles = new Vector3(targetTransform.eulerAngles.x, value, targetTransform.eulerAngles.z);
					}
					break;
				case Axis.Z:
					if (localSpace)
					{
						targetTransform.localEulerAngles = new Vector3(targetTransform.localEulerAngles.x, targetTransform.localEulerAngles.y, value);
					}
					else
					{
						targetTransform.eulerAngles = new Vector3(targetTransform.eulerAngles.x, targetTransform.eulerAngles.y, value);
					}
					break;
				}
				break;
			case SelectAttribute.Scale:
				switch (targetAxis)
				{
				case Axis.X:
					if (localSpace)
					{
						targetTransform.localScale = new Vector3(value, targetTransform.localScale.y, targetTransform.localScale.z);
					}
					else
					{
						targetTransform.lossyScale.Set(value, targetTransform.lossyScale.y, targetTransform.lossyScale.z);
					}
					break;
				case Axis.Y:
					if (localSpace)
					{
						targetTransform.localScale = new Vector3(targetTransform.localScale.x, value, targetTransform.localScale.z);
					}
					else
					{
						targetTransform.lossyScale.Set(targetTransform.lossyScale.x, value, targetTransform.lossyScale.z);
					}
					break;
				case Axis.Z:
					if (localSpace)
					{
						targetTransform.localScale = new Vector3(targetTransform.localScale.x, targetTransform.localScale.y, value);
					}
					else
					{
						targetTransform.lossyScale.Set(targetTransform.lossyScale.x, targetTransform.lossyScale.y, value);
					}
					break;
				}
				break;
			}
		}
	}
}
