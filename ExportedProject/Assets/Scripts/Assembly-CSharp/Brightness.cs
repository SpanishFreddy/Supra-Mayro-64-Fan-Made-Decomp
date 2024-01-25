using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Brightness")]
public class Brightness : MonoBehaviour
{
	public Shader shaderDerp;

	private Material m_Material;

	[Range(0.5f, 2f)]
	public float brightness = 1f;

	private Material material
	{
		get
		{
			if (m_Material == null)
			{
				m_Material = new Material(shaderDerp);
				m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_Material;
		}
	}

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
		else if (!shaderDerp || !shaderDerp.isSupported)
		{
			base.enabled = false;
		}
	}

	private void OnDisable()
	{
		if ((bool)m_Material)
		{
			Object.DestroyImmediate(m_Material);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Brightness", brightness);
		Graphics.Blit(source, destination, material);
	}
}
