using UnityEngine;

namespace EnhancedTriggerbox.Demo
{
	public class CubeShootController : MonoBehaviour
	{
		public void ShootCubes(float velocity)
		{
			CubeShoot[] componentsInChildren = GetComponentsInChildren<CubeShoot>();
			foreach (CubeShoot cubeShoot in componentsInChildren)
			{
				cubeShoot.ShootCube(velocity);
			}
		}
	}
}
