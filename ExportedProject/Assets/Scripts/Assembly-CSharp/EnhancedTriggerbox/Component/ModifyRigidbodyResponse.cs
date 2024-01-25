using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class ModifyRigidbodyResponse : ResponseComponent
	{
		public enum ChangeBool
		{
			RemainTheSame = 0,
			SetTrue = 1,
			SetFalse = 2,
			Toggle = 3
		}

		public enum ChangeInterpolate
		{
			RemainTheSame = 0,
			SetInterpolate = 1,
			SetExtrapolate = 2
		}

		public enum ChangeCollisionDetection
		{
			RemainTheSame = 0,
			SetContinuous = 1,
			SetContinuousDynamic = 2,
			SetDiscrete = 3
		}

		public Rigidbody rigbody;

		public string setMass;

		public string setDrag;

		public string setAngularDrag;

		public ChangeBool changeGravity;

		public ChangeBool changeKinematic;

		public ChangeInterpolate changeInterpolate;

		public ChangeCollisionDetection changeCollisionDetection;

		public bool editConstraints;

		public bool xPos;

		public bool yPos;

		public bool zPos;

		public bool xRot;

		public bool yRot;

		public bool zRot;

		public override bool ExecuteAction()
		{
			if (!string.IsNullOrEmpty(setMass))
			{
				int result;
				if (int.TryParse(setMass, out result))
				{
					rigbody.mass = result;
				}
				else
				{
					Debug.Log("Unable to parse setMass, " + setMass + ", to an integer.");
				}
			}
			if (!string.IsNullOrEmpty(setDrag))
			{
				int result2;
				if (int.TryParse(setDrag, out result2))
				{
					rigbody.drag = result2;
				}
				else
				{
					Debug.Log("Unable to parse setDrag, " + setMass + ", to an integer.");
				}
			}
			if (!string.IsNullOrEmpty(setAngularDrag))
			{
				float result3;
				if (float.TryParse(setAngularDrag, out result3))
				{
					rigbody.angularDrag = result3;
				}
				else
				{
					Debug.Log("Unable to parse setAngularDrag, " + setAngularDrag + ", to a float.");
				}
			}
			switch (changeGravity)
			{
			case ChangeBool.SetFalse:
				rigbody.useGravity = false;
				break;
			case ChangeBool.SetTrue:
				rigbody.useGravity = true;
				break;
			case ChangeBool.Toggle:
				rigbody.useGravity = !rigbody.useGravity;
				break;
			}
			switch (changeKinematic)
			{
			case ChangeBool.SetFalse:
				rigbody.isKinematic = false;
				break;
			case ChangeBool.SetTrue:
				rigbody.isKinematic = true;
				break;
			case ChangeBool.Toggle:
				rigbody.isKinematic = !rigbody.isKinematic;
				break;
			}
			switch (changeInterpolate)
			{
			case ChangeInterpolate.SetExtrapolate:
				rigbody.interpolation = RigidbodyInterpolation.Extrapolate;
				break;
			case ChangeInterpolate.SetInterpolate:
				rigbody.interpolation = RigidbodyInterpolation.Interpolate;
				break;
			}
			switch (changeCollisionDetection)
			{
			case ChangeCollisionDetection.SetContinuous:
				rigbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
				break;
			case ChangeCollisionDetection.SetDiscrete:
				rigbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
				break;
			case ChangeCollisionDetection.SetContinuousDynamic:
				rigbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				break;
			}
			if (editConstraints)
			{
				rigbody.constraints = RigidbodyConstraints.FreezeAll;
				if (!xPos)
				{
					rigbody.constraints &= (RigidbodyConstraints)(-3);
				}
				if (!yPos)
				{
					rigbody.constraints &= (RigidbodyConstraints)(-5);
				}
				if (!zPos)
				{
					rigbody.constraints &= (RigidbodyConstraints)(-9);
				}
				if (!xRot)
				{
					rigbody.constraints &= (RigidbodyConstraints)(-17);
				}
				if (!yRot)
				{
					rigbody.constraints &= (RigidbodyConstraints)(-33);
				}
				if (!zRot)
				{
					rigbody.constraints &= (RigidbodyConstraints)(-65);
				}
			}
			return true;
		}

		public override void Validation()
		{
			if (!rigbody)
			{
				ShowWarningMessage("For the rigidbody response you need to add a rigidbody reference!");
			}
			int result;
			if (!string.IsNullOrEmpty(setMass) && !int.TryParse(setMass, out result))
			{
				ShowWarningMessage("You have entered a mass but it couldn't be parsed. Please make sure it is a valid integer.");
			}
			int result2;
			if (!string.IsNullOrEmpty(setDrag) && !int.TryParse(setDrag, out result2))
			{
				ShowWarningMessage("You have entered a drag but it couldn't be parsed. Please make sure it is a valid integer.");
			}
			float result3;
			if (!string.IsNullOrEmpty(setAngularDrag) && !float.TryParse(setAngularDrag, out result3))
			{
				ShowWarningMessage("You have entered a angular drag but it couldn't be parsed. Please make sure it is a valid float.");
			}
		}
	}
}
