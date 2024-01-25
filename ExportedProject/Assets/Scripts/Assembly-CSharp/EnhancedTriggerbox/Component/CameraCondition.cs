using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class CameraCondition : ConditionComponent
	{
		public enum LookType
		{
			LookingAt = 0,
			LookingAway = 1
		}

		public enum CameraConditionComponentParameters
		{
			Transform = 0,
			FullBoxCollider = 1,
			MinimumBoxCollider = 2,
			MeshRenderer = 3
		}

		public enum RaycastIntensity
		{
			IgnoreObstacles = 0,
			VeryLow = 1,
			Low = 2,
			Med = 3,
			High = 4
		}

		public Camera cam;

		public LookType cameraConditionType;

		public GameObject conditionObject;

		public CameraConditionComponentParameters componentParameter;

		public RaycastIntensity raycastIntensity = RaycastIntensity.Med;

		public float minDistance;

		private Vector3 viewConditionScreenPoint = default(Vector3);

		private RaycastHit viewConditionRaycastHit = default(RaycastHit);

		private BoxCollider viewConditionObjectCollider;

		private MeshRenderer viewConditionObjectMeshRenderer;

		private Plane[] viewConditionCameraPlane;

		private bool canRaycast = true;

		public override void Validation()
		{
			if (!cam)
			{
				cam = Camera.main;
			}
			else if (componentParameter == CameraConditionComponentParameters.MeshRenderer && Camera.allCamerasCount > 1)
			{
				ShowWarningMessage("You have selected the Component Parameter for the camera condition to be Mesh Renderer however you have more than 1 camera in the scene. The condition will pass if the mesh can be seen by ANY camera, not just the one you selected above. This is a limitation caused by the MeshRenderer().isVisible bool.");
			}
			if (!conditionObject)
			{
				ShowWarningMessage("You have selected the " + ((cameraConditionType != 0) ? "Looking Away" : "Looking At") + " camera condition but have not specified the gameobject for the condition!");
			}
			else if (componentParameter == CameraConditionComponentParameters.FullBoxCollider || componentParameter == CameraConditionComponentParameters.MinimumBoxCollider)
			{
				if (conditionObject.GetComponent<BoxCollider>() == null)
				{
					ShowWarningMessage("You have selected the Component Parameter for the camera condition to be " + ((componentParameter != CameraConditionComponentParameters.FullBoxCollider) ? "Minimum Box Collider" : "Full Box Collider") + " but the object doesn't have a Box Collider component!");
				}
			}
			else if (componentParameter == CameraConditionComponentParameters.MeshRenderer && conditionObject.GetComponent<MeshRenderer>() == null)
			{
				ShowWarningMessage("You have selected the Component Parameter for the camera condition to be Mesh Renderer but the object doesn't have a Mesh Renderer component!");
			}
			if (componentParameter == CameraConditionComponentParameters.MeshRenderer && raycastIntensity == RaycastIntensity.High)
			{
				ShowWarningMessage("High raycast intensity will have no extra effect than med when using mesh renderer. This is because high intensity uses all the points from the a box colliders bounds but mesh renderers do not have bounds so only the position is checked.");
			}
		}

		public override void OnAwake()
		{
			if ((bool)conditionObject)
			{
				if (componentParameter == CameraConditionComponentParameters.FullBoxCollider || componentParameter == CameraConditionComponentParameters.MinimumBoxCollider)
				{
					viewConditionObjectCollider = conditionObject.GetComponent<BoxCollider>();
				}
				else if (componentParameter == CameraConditionComponentParameters.MeshRenderer)
				{
					viewConditionObjectMeshRenderer = conditionObject.GetComponent<MeshRenderer>();
				}
			}
			if (!cam)
			{
				cam = Camera.main;
			}
		}

		public override bool ExecuteAction()
		{
			if (Vector3.Distance(cam.transform.position, conditionObject.transform.position) < minDistance)
			{
				return false;
			}
			switch (cameraConditionType)
			{
			case LookType.LookingAt:
				switch (componentParameter)
				{
				case CameraConditionComponentParameters.Transform:
					if (IsInCameraFrustum(conditionObject.transform.position))
					{
						if (raycastIntensity == RaycastIntensity.IgnoreObstacles)
						{
							return true;
						}
						if (CheckRaycastTransform(conditionObject.transform.position - cam.transform.position))
						{
							return true;
						}
					}
					break;
				case CameraConditionComponentParameters.MinimumBoxCollider:
					if (!CheckMinimumBoxCollider(viewConditionObjectCollider.bounds))
					{
						break;
					}
					if (raycastIntensity == RaycastIntensity.IgnoreObstacles)
					{
						return true;
					}
					if (raycastIntensity == RaycastIntensity.High)
					{
						if (CheckRaycastMinimumCollider(viewConditionObjectCollider.bounds))
						{
							return true;
						}
					}
					else if (CheckRaycastTransform(conditionObject.transform.position - cam.transform.position))
					{
						return true;
					}
					break;
				case CameraConditionComponentParameters.FullBoxCollider:
					if (!CheckFullBoxCollider(viewConditionObjectCollider.bounds))
					{
						break;
					}
					if (raycastIntensity == RaycastIntensity.IgnoreObstacles)
					{
						return true;
					}
					if (raycastIntensity == RaycastIntensity.High)
					{
						if (CheckRaycastFullCollider(viewConditionObjectCollider.bounds))
						{
							return true;
						}
					}
					else if (CheckRaycastTransform(conditionObject.transform.position - cam.transform.position))
					{
						return true;
					}
					break;
				case CameraConditionComponentParameters.MeshRenderer:
					if (viewConditionObjectMeshRenderer.isVisible)
					{
						if (raycastIntensity != 0)
						{
							return true;
						}
						if (CheckRaycastTransform(conditionObject.transform.position - cam.transform.position))
						{
							return true;
						}
					}
					break;
				}
				break;
			case LookType.LookingAway:
				switch (componentParameter)
				{
				case CameraConditionComponentParameters.Transform:
					if (!IsInCameraFrustum(conditionObject.transform.position))
					{
						return true;
					}
					break;
				case CameraConditionComponentParameters.MinimumBoxCollider:
					if (CheckMinimumBoxCollider(viewConditionObjectCollider.bounds))
					{
						return true;
					}
					break;
				case CameraConditionComponentParameters.FullBoxCollider:
					if (CheckFullBoxCollider(viewConditionObjectCollider.bounds))
					{
						return true;
					}
					break;
				case CameraConditionComponentParameters.MeshRenderer:
					if (!conditionObject.GetComponent<MeshRenderer>().isVisible)
					{
						return true;
					}
					break;
				}
				break;
			}
			return false;
		}

		private bool CheckMinimumBoxCollider(Bounds bounds)
		{
			if (cameraConditionType == LookType.LookingAt)
			{
				if (IsInCameraFrustum(bounds.min))
				{
					return true;
				}
				if (IsInCameraFrustum(bounds.max))
				{
					return true;
				}
				if (IsInCameraFrustum(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z)))
				{
					return true;
				}
				if (IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z)))
				{
					return true;
				}
				if (IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z)))
				{
					return true;
				}
				if (IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)))
				{
					return true;
				}
				if (IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)))
				{
					return true;
				}
				if (IsInCameraFrustum(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)))
				{
					return true;
				}
				return false;
			}
			if (!IsInCameraFrustum(bounds.min))
			{
				return true;
			}
			if (!IsInCameraFrustum(bounds.max))
			{
				return true;
			}
			if (!IsInCameraFrustum(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z)))
			{
				return true;
			}
			if (!IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z)))
			{
				return true;
			}
			if (!IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z)))
			{
				return true;
			}
			if (!IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)))
			{
				return true;
			}
			if (!IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)))
			{
				return true;
			}
			if (!IsInCameraFrustum(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)))
			{
				return true;
			}
			return false;
		}

		private bool CheckFullBoxCollider(Bounds bounds)
		{
			if (cameraConditionType == LookType.LookingAt)
			{
				if (IsInCameraFrustum(bounds.center) && IsInCameraFrustum(bounds.min) && IsInCameraFrustum(bounds.max) && IsInCameraFrustum(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z)) && IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z)) && IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z)) && IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)) && IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)) && IsInCameraFrustum(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)))
				{
					return true;
				}
				return false;
			}
			if (!IsInCameraFrustum(bounds.center) && !IsInCameraFrustum(bounds.min) && !IsInCameraFrustum(bounds.max) && !IsInCameraFrustum(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z)) && !IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z)) && !IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z)) && !IsInCameraFrustum(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)) && !IsInCameraFrustum(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)) && !IsInCameraFrustum(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)))
			{
				return true;
			}
			return false;
		}

		private bool IsInCameraFrustum(Vector3 position)
		{
			viewConditionScreenPoint = cam.WorldToViewportPoint(position);
			if (viewConditionScreenPoint.z > 0f && viewConditionScreenPoint.x > 0f && viewConditionScreenPoint.x < 1f && viewConditionScreenPoint.y > 0f && viewConditionScreenPoint.y < 1f)
			{
				return true;
			}
			return false;
		}

		private bool CheckRaycastTransform(Vector3 viewConditionDirection)
		{
			if (!canRaycast)
			{
				return false;
			}
			if (Physics.Raycast(cam.transform.position, viewConditionDirection.normalized, out viewConditionRaycastHit, viewConditionDirection.magnitude))
			{
				if (viewConditionRaycastHit.transform == conditionObject.transform)
				{
					return true;
				}
				if (raycastIntensity == RaycastIntensity.Low || raycastIntensity == RaycastIntensity.VeryLow)
				{
					StartCoroutine("WaitForSecs");
				}
				return false;
			}
			return true;
		}

		private bool CheckRaycastMinimumCollider(Bounds bounds)
		{
			if (CheckRaycastTransform(bounds.center - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(bounds.min - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(bounds.max - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z) - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z) - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z) - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z) - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z) - cam.transform.position))
			{
				return true;
			}
			if (CheckRaycastTransform(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z) - cam.transform.position))
			{
				return true;
			}
			return false;
		}

		private bool CheckRaycastFullCollider(Bounds bounds)
		{
			if (CheckRaycastTransform(bounds.center - cam.transform.position) && CheckRaycastTransform(bounds.min - cam.transform.position) && CheckRaycastTransform(bounds.max - cam.transform.position) && CheckRaycastTransform(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z) - cam.transform.position) && CheckRaycastTransform(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z) - cam.transform.position) && CheckRaycastTransform(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z) - cam.transform.position) && CheckRaycastTransform(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z) - cam.transform.position) && CheckRaycastTransform(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z) - cam.transform.position) && CheckRaycastTransform(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z) - cam.transform.position))
			{
				return true;
			}
			return false;
		}

		private IEnumerator WaitForSecs()
		{
			canRaycast = false;
			switch (raycastIntensity)
			{
			case RaycastIntensity.VeryLow:
				yield return new WaitForSeconds(1f);
				break;
			case RaycastIntensity.Low:
				yield return new WaitForSeconds(0.1f);
				break;
			}
			canRaycast = true;
		}
	}
}
