using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class SetMaterialProperty : ResponseComponent
	{
		public enum PropertyType
		{
			Float = 0,
			Int = 1,
			Colour = 2,
			Vector4 = 3,
			Texture = 4
		}

		public enum ReferenceType
		{
			GameObjectReference = 0,
			GameObjectName = 1,
			CollisionGameObject = 2,
			Material = 3
		}

		public GameObject targetGameobject;

		public string targetGameobjectName;

		public Material targetMaterial;

		public bool cloneMaterial;

		public string propertyName;

		public PropertyType propertyType;

		public ReferenceType referenceType;

		public float propertyFloat;

		public int propertyInt;

		public Color propertyColour;

		public Vector4 propertyVector4;

		public Texture propertyTexture;

		private MeshRenderer meshRenderer;

		public override bool requiresCollisionObjectData
		{
			get
			{
				return true;
			}
		}

		public override bool ExecuteAction(GameObject collisionGameObject)
		{
			if (referenceType != ReferenceType.Material)
			{
				switch (referenceType)
				{
				case ReferenceType.CollisionGameObject:
					targetGameobject = collisionGameObject;
					break;
				case ReferenceType.GameObjectName:
					targetGameobject = GameObject.Find(targetGameobjectName);
					break;
				}
				if (!targetGameobject)
				{
					Debug.Log("Unable to execute Set Material Property Response. Missing gameobject reference!");
					return true;
				}
				if (cloneMaterial)
				{
					meshRenderer = targetGameobject.GetComponent<MeshRenderer>();
					if (!meshRenderer)
					{
						Debug.Log("Unable to execute Set Material Property Response. Missing mesh renderer component!");
						return true;
					}
					targetMaterial = new Material(meshRenderer.material);
					meshRenderer.material = targetMaterial;
				}
				else
				{
					targetMaterial = targetGameobject.GetComponent<MeshRenderer>().material;
				}
			}
			if (targetMaterial == null || string.IsNullOrEmpty(propertyName))
			{
				Debug.Log("Unable to execute Set Material Property Response. Missing material or property name!");
				return true;
			}
			if (duration > 0f)
			{
				ApplyOverTime();
			}
			else
			{
				ApplyInstantly();
			}
			return true;
		}

		private void ApplyInstantly()
		{
			switch (propertyType)
			{
			case PropertyType.Float:
				targetMaterial.SetFloat(propertyName, propertyFloat);
				break;
			case PropertyType.Int:
				targetMaterial.SetInt(propertyName, propertyInt);
				break;
			case PropertyType.Colour:
				targetMaterial.SetColor(propertyName, propertyColour);
				break;
			case PropertyType.Vector4:
				targetMaterial.SetVector(propertyName, propertyVector4);
				break;
			case PropertyType.Texture:
				targetMaterial.SetTexture(propertyName, propertyTexture);
				break;
			}
		}

		private void ApplyOverTime()
		{
			switch (propertyType)
			{
			case PropertyType.Float:
				activeCoroutines.Add(StartCoroutine(ModifyMaterialFloatOverTime()));
				break;
			case PropertyType.Int:
				activeCoroutines.Add(StartCoroutine(ModifyMaterialIntOverTime()));
				break;
			case PropertyType.Colour:
				activeCoroutines.Add(StartCoroutine(ModifyMaterialColourOverTime()));
				break;
			case PropertyType.Vector4:
				activeCoroutines.Add(StartCoroutine(ModifyMaterialVector4OverTime()));
				break;
			case PropertyType.Texture:
				targetMaterial.SetTexture(propertyName, propertyTexture);
				break;
			}
		}

		private IEnumerator ModifyMaterialFloatOverTime()
		{
			Material mat = targetMaterial;
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / duration;
			float originalValue = mat.GetFloat(propertyName);
			while (progress < 1f)
			{
				mat.SetFloat(propertyName, Mathf.Lerp(originalValue, propertyFloat, progress));
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}

		private IEnumerator ModifyMaterialIntOverTime()
		{
			Material mat = targetMaterial;
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / duration;
			int originalValue = mat.GetInt(propertyName);
			while (progress < 1f)
			{
				mat.SetInt(propertyName, Mathf.RoundToInt(Mathf.Lerp(originalValue, propertyFloat, progress)));
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}

		private IEnumerator ModifyMaterialColourOverTime()
		{
			Material mat = targetMaterial;
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / duration;
			Color originalValue = mat.GetColor(propertyName);
			while (progress < 1f)
			{
				mat.SetColor(propertyName, Color.Lerp(originalValue, propertyColour, progress));
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}

		private IEnumerator ModifyMaterialVector4OverTime()
		{
			Material mat = targetMaterial;
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / duration;
			Vector4 originalValue = mat.GetVector(propertyName);
			while (progress < 1f)
			{
				mat.SetVector(propertyName, Vector4.Lerp(originalValue, propertyVector4, progress));
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}

		public override void Validation()
		{
			if (!targetMaterial)
			{
				return;
			}
			if (!string.IsNullOrEmpty(propertyName))
			{
				PropertyType propertyType = this.propertyType;
				if (propertyType == PropertyType.Texture && propertyTexture == null)
				{
					ShowWarningMessage("You need to enter a texture to set the property with!");
				}
			}
			else
			{
				ShowWarningMessage("You need to enter the name of the property to want to set!");
			}
		}
	}
}
