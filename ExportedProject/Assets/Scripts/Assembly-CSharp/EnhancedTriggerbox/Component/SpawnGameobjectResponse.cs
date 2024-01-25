using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class SpawnGameobjectResponse : ResponseComponent
	{
		public GameObject prefabToSpawn;

		public string newInstanceName;

		public Transform customPositionRotation;

		public override bool ExecuteAction()
		{
			if ((bool)prefabToSpawn)
			{
				if (!string.IsNullOrEmpty(newInstanceName))
				{
					if ((bool)customPositionRotation)
					{
						GameObject gameObject = Object.Instantiate(prefabToSpawn, customPositionRotation.position, customPositionRotation.rotation);
						gameObject.name = newInstanceName;
					}
					else
					{
						GameObject gameObject2 = Object.Instantiate(prefabToSpawn);
						gameObject2.name = newInstanceName;
					}
				}
				else if ((bool)customPositionRotation)
				{
					Object.Instantiate(prefabToSpawn, customPositionRotation.position, customPositionRotation.rotation);
				}
				else
				{
					Object.Instantiate(prefabToSpawn);
				}
			}
			return true;
		}
	}
}
