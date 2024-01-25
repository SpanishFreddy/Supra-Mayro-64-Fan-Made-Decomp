using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class TransformCondition : ConditionComponent
	{
		public enum TransformConditionType
		{
			SingleAxis = 0,
			DistanceToObject2D = 1,
			DistanceToObject3D = 2,
			LocalDistanceToObject2D = 3,
			LocalDistanceToObject3D = 4
		}

		public enum TransformComponent
		{
			Position = 0,
			Rotation = 1,
			LocalPosition = 2,
			LocalRotation = 3
		}

		public enum ConditionType
		{
			GreaterThan = 0,
			GreaterThanOrEqualTo = 1,
			EqualTo = 2,
			LessThanOrEqualTo = 3,
			LessThan = 4
		}

		public enum Axis
		{
			X = 0,
			Y = 1,
			Z = 2
		}

		public Transform targetTransform;

		public Transform destinationTransform;

		public TransformComponent transformComponent;

		public Axis axis;

		public ConditionType conditionType;

		public float value;

		public TransformConditionType transformConditionType;

		public override bool ExecuteAction()
		{
			if (!targetTransform)
			{
				Debug.Log("You haven't assigned a target transform in the transform condition.");
				return false;
			}
			switch (transformConditionType)
			{
			case TransformConditionType.SingleAxis:
				switch (transformComponent)
				{
				case TransformComponent.Position:
					switch (axis)
					{
					case Axis.X:
						return CompareValue(targetTransform.position.x);
					case Axis.Y:
						return CompareValue(targetTransform.position.y);
					case Axis.Z:
						return CompareValue(targetTransform.position.z);
					}
					break;
				case TransformComponent.Rotation:
					switch (axis)
					{
					case Axis.X:
						return CompareValue(targetTransform.rotation.x);
					case Axis.Y:
						return CompareValue(targetTransform.rotation.y);
					case Axis.Z:
						return CompareValue(targetTransform.rotation.z);
					}
					break;
				case TransformComponent.LocalPosition:
					switch (axis)
					{
					case Axis.X:
						return CompareValue(targetTransform.localPosition.x);
					case Axis.Y:
						return CompareValue(targetTransform.localPosition.y);
					case Axis.Z:
						return CompareValue(targetTransform.localPosition.z);
					}
					break;
				case TransformComponent.LocalRotation:
					switch (axis)
					{
					case Axis.X:
						return CompareValue(targetTransform.localRotation.x);
					case Axis.Y:
						return CompareValue(targetTransform.localRotation.y);
					case Axis.Z:
						return CompareValue(targetTransform.localRotation.z);
					}
					break;
				}
				break;
			case TransformConditionType.DistanceToObject2D:
				return CompareValue(Vector2.Distance(targetTransform.position, destinationTransform.position));
			case TransformConditionType.DistanceToObject3D:
				return CompareValue(Vector3.Distance(targetTransform.position, destinationTransform.position));
			case TransformConditionType.LocalDistanceToObject2D:
				return CompareValue(Vector2.Distance(targetTransform.localPosition, destinationTransform.localPosition));
			case TransformConditionType.LocalDistanceToObject3D:
				return CompareValue(Vector3.Distance(targetTransform.localPosition, destinationTransform.localPosition));
			}
			return false;
		}

		public override void Validation()
		{
			if (transformConditionType != 0 && !destinationTransform)
			{
				ShowWarningMessage("You need to assign a transform to the Destination Transform to calculate the distance between that and the Target Transform.");
			}
			base.Validation();
		}

		private bool CompareValue(float val)
		{
			switch (conditionType)
			{
			case ConditionType.EqualTo:
				if (val == value)
				{
					return true;
				}
				return false;
			case ConditionType.GreaterThan:
				if (val > value)
				{
					return true;
				}
				return false;
			case ConditionType.GreaterThanOrEqualTo:
				if (val >= value)
				{
					return true;
				}
				return false;
			case ConditionType.LessThan:
				if (val < value)
				{
					return true;
				}
				return false;
			case ConditionType.LessThanOrEqualTo:
				if (val <= value)
				{
					return true;
				}
				return false;
			default:
				return false;
			}
		}
	}
}
