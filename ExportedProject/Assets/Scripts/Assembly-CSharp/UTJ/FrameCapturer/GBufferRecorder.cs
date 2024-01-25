using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UTJ.FrameCapturer
{
	[AddComponentMenu("UTJ/FrameCapturer/GBuffer Recorder")]
	[RequireComponent(typeof(Camera))]
	[ExecuteInEditMode]
	public class GBufferRecorder : RecorderBase
	{
		[Serializable]
		public struct FrameBufferConponents
		{
			public bool frameBuffer;

			public bool fbColor;

			public bool fbAlpha;

			public bool GBuffer;

			public bool gbAlbedo;

			public bool gbOcclusion;

			public bool gbSpecular;

			public bool gbSmoothness;

			public bool gbNormal;

			public bool gbEmission;

			public bool gbDepth;

			public bool gbVelocity;

			public static FrameBufferConponents defaultValue
			{
				get
				{
					FrameBufferConponents result = default(FrameBufferConponents);
					result.frameBuffer = true;
					result.fbColor = true;
					result.fbAlpha = true;
					result.GBuffer = true;
					result.gbAlbedo = true;
					result.gbOcclusion = true;
					result.gbSpecular = true;
					result.gbSmoothness = true;
					result.gbNormal = true;
					result.gbEmission = true;
					result.gbDepth = true;
					result.gbVelocity = true;
					return result;
				}
			}
		}

		private class BufferRecorder
		{
			private RenderTexture m_rt;

			private int m_channels;

			private int m_targetFramerate = 30;

			private string m_name;

			private MovieEncoder m_encoder;

			public BufferRecorder(RenderTexture rt, int ch, string name, int tf)
			{
				m_rt = rt;
				m_channels = ch;
				m_name = name;
			}

			public bool Initialize(MovieEncoderConfigs c, DataPath p)
			{
				string path = p.GetFullPath() + "/" + m_name;
				c.Setup(m_rt.width, m_rt.height, m_channels, m_targetFramerate);
				m_encoder = MovieEncoder.Create(c, path);
				return m_encoder != null && m_encoder.IsValid();
			}

			public void Release()
			{
				if (m_encoder != null)
				{
					m_encoder.Release();
					m_encoder = null;
				}
			}

			public void Update(double time)
			{
				if (m_encoder != null)
				{
					fcAPI.fcLock(m_rt, delegate(byte[] data, fcAPI.fcPixelFormat fmt)
					{
						m_encoder.AddVideoFrame(data, fmt, time);
					});
				}
			}
		}

		[SerializeField]
		private MovieEncoderConfigs m_encoderConfigs = new MovieEncoderConfigs(MovieEncoder.Type.Exr);

		[SerializeField]
		private FrameBufferConponents m_fbComponents = FrameBufferConponents.defaultValue;

		[SerializeField]
		private Shader m_shCopy;

		private Material m_matCopy;

		private Mesh m_quad;

		private CommandBuffer m_cbCopyFB;

		private CommandBuffer m_cbCopyGB;

		private CommandBuffer m_cbClearGB;

		private CommandBuffer m_cbCopyVelocity;

		private RenderTexture[] m_rtFB;

		private RenderTexture[] m_rtGB;

		private List<BufferRecorder> m_recorders = new List<BufferRecorder>();

		public FrameBufferConponents fbComponents
		{
			get
			{
				return m_fbComponents;
			}
			set
			{
				m_fbComponents = value;
			}
		}

		public MovieEncoderConfigs encoderConfigs
		{
			get
			{
				return m_encoderConfigs;
			}
		}

		public override bool BeginRecording()
		{
			if (m_recording)
			{
				return false;
			}
			if (m_shCopy == null)
			{
				Debug.LogError("GBufferRecorder: copy shader is missing!");
				return false;
			}
			m_outputDir.CreateDirectory();
			if (m_quad == null)
			{
				m_quad = fcAPI.CreateFullscreenQuad();
			}
			if (m_matCopy == null)
			{
				m_matCopy = new Material(m_shCopy);
			}
			Camera component = GetComponent<Camera>();
			if (component.targetTexture != null)
			{
				m_matCopy.EnableKeyword("OFFSCREEN");
			}
			else
			{
				m_matCopy.DisableKeyword("OFFSCREEN");
			}
			int w = component.pixelWidth;
			int h = component.pixelHeight;
			GetCaptureResolution(ref w, ref h);
			if (m_encoderConfigs.format == MovieEncoder.Type.MP4 || m_encoderConfigs.format == MovieEncoder.Type.WebM)
			{
				w = (w + 1) & -2;
				h = (h + 1) & -2;
			}
			if (m_fbComponents.frameBuffer)
			{
				m_rtFB = new RenderTexture[2];
				for (int i = 0; i < m_rtFB.Length; i++)
				{
					m_rtFB[i] = new RenderTexture(w, h, 0, RenderTextureFormat.ARGBHalf);
					m_rtFB[i].filterMode = FilterMode.Point;
					m_rtFB[i].Create();
				}
				int num = Shader.PropertyToID("_TmpFrameBuffer");
				m_cbCopyFB = new CommandBuffer();
				m_cbCopyFB.name = "GBufferRecorder: Copy FrameBuffer";
				m_cbCopyFB.GetTemporaryRT(num, -1, -1, 0, FilterMode.Point);
				m_cbCopyFB.Blit(BuiltinRenderTextureType.CurrentActive, num);
				m_cbCopyFB.SetRenderTarget(new RenderTargetIdentifier[2]
				{
					m_rtFB[0],
					m_rtFB[1]
				}, m_rtFB[0]);
				m_cbCopyFB.DrawMesh(m_quad, Matrix4x4.identity, m_matCopy, 0, 0);
				m_cbCopyFB.ReleaseTemporaryRT(num);
				component.AddCommandBuffer(CameraEvent.AfterEverything, m_cbCopyFB);
			}
			if (m_fbComponents.GBuffer)
			{
				m_rtGB = new RenderTexture[8];
				for (int j = 0; j < m_rtGB.Length; j++)
				{
					m_rtGB[j] = new RenderTexture(w, h, 0, RenderTextureFormat.ARGBHalf);
					m_rtGB[j].filterMode = FilterMode.Point;
					m_rtGB[j].Create();
				}
				m_cbClearGB = new CommandBuffer();
				m_cbClearGB.name = "GBufferRecorder: Cleanup GBuffer";
				if (component.allowHDR)
				{
					m_cbClearGB.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
				}
				else
				{
					m_cbClearGB.SetRenderTarget(BuiltinRenderTextureType.GBuffer3);
				}
				m_cbClearGB.DrawMesh(m_quad, Matrix4x4.identity, m_matCopy, 0, 3);
				m_matCopy.SetColor("_ClearColor", component.backgroundColor);
				m_cbCopyGB = new CommandBuffer();
				m_cbCopyGB.name = "GBufferRecorder: Copy GBuffer";
				m_cbCopyGB.SetRenderTarget(new RenderTargetIdentifier[7]
				{
					m_rtGB[0],
					m_rtGB[1],
					m_rtGB[2],
					m_rtGB[3],
					m_rtGB[4],
					m_rtGB[5],
					m_rtGB[6]
				}, m_rtGB[0]);
				m_cbCopyGB.DrawMesh(m_quad, Matrix4x4.identity, m_matCopy, 0, 2);
				component.AddCommandBuffer(CameraEvent.BeforeGBuffer, m_cbClearGB);
				component.AddCommandBuffer(CameraEvent.BeforeLighting, m_cbCopyGB);
				if (m_fbComponents.gbVelocity)
				{
					m_cbCopyVelocity = new CommandBuffer();
					m_cbCopyVelocity.name = "GBufferRecorder: Copy Velocity";
					m_cbCopyVelocity.SetRenderTarget(m_rtGB[7]);
					m_cbCopyVelocity.DrawMesh(m_quad, Matrix4x4.identity, m_matCopy, 0, 4);
					component.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, m_cbCopyVelocity);
					component.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
				}
			}
			int tf = m_targetFramerate;
			if (m_fbComponents.frameBuffer)
			{
				if (m_fbComponents.fbColor)
				{
					m_recorders.Add(new BufferRecorder(m_rtFB[0], 4, "FrameBuffer", tf));
				}
				if (m_fbComponents.fbAlpha)
				{
					m_recorders.Add(new BufferRecorder(m_rtFB[1], 1, "Alpha", tf));
				}
			}
			if (m_fbComponents.GBuffer)
			{
				if (m_fbComponents.gbAlbedo)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[0], 3, "Albedo", tf));
				}
				if (m_fbComponents.gbOcclusion)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[1], 1, "Occlusion", tf));
				}
				if (m_fbComponents.gbSpecular)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[2], 3, "Specular", tf));
				}
				if (m_fbComponents.gbSmoothness)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[3], 1, "Smoothness", tf));
				}
				if (m_fbComponents.gbNormal)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[4], 3, "Normal", tf));
				}
				if (m_fbComponents.gbEmission)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[5], 3, "Emission", tf));
				}
				if (m_fbComponents.gbDepth)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[6], 1, "Depth", tf));
				}
				if (m_fbComponents.gbVelocity)
				{
					m_recorders.Add(new BufferRecorder(m_rtGB[7], 2, "Velocity", tf));
				}
			}
			foreach (BufferRecorder recorder in m_recorders)
			{
				if (!recorder.Initialize(m_encoderConfigs, m_outputDir))
				{
					EndRecording();
					return false;
				}
			}
			base.BeginRecording();
			Debug.Log("GBufferRecorder: BeginRecording()");
			return true;
		}

		public override void EndRecording()
		{
			foreach (BufferRecorder recorder in m_recorders)
			{
				recorder.Release();
			}
			m_recorders.Clear();
			Camera component = GetComponent<Camera>();
			if (m_cbCopyFB != null)
			{
				component.RemoveCommandBuffer(CameraEvent.AfterEverything, m_cbCopyFB);
				m_cbCopyFB.Release();
				m_cbCopyFB = null;
			}
			if (m_cbClearGB != null)
			{
				component.RemoveCommandBuffer(CameraEvent.BeforeGBuffer, m_cbClearGB);
				m_cbClearGB.Release();
				m_cbClearGB = null;
			}
			if (m_cbCopyGB != null)
			{
				component.RemoveCommandBuffer(CameraEvent.BeforeLighting, m_cbCopyGB);
				m_cbCopyGB.Release();
				m_cbCopyGB = null;
			}
			if (m_cbCopyVelocity != null)
			{
				component.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, m_cbCopyVelocity);
				m_cbCopyVelocity.Release();
				m_cbCopyVelocity = null;
			}
			if (m_rtFB != null)
			{
				RenderTexture[] rtFB = m_rtFB;
				foreach (RenderTexture renderTexture in rtFB)
				{
					renderTexture.Release();
				}
				m_rtFB = null;
			}
			if (m_rtGB != null)
			{
				RenderTexture[] rtGB = m_rtGB;
				foreach (RenderTexture renderTexture2 in rtGB)
				{
					renderTexture2.Release();
				}
				m_rtGB = null;
			}
			if (m_recording)
			{
				Debug.Log("GBufferRecorder: EndRecording()");
			}
			base.EndRecording();
		}

		private IEnumerator OnPostRender()
		{
			if (m_recording)
			{
				yield return new WaitForEndOfFrame();
				double timestamp = 1.0 / (double)m_targetFramerate * (double)m_recordedFrames;
				foreach (BufferRecorder recorder in m_recorders)
				{
					recorder.Update(timestamp);
				}
				m_recordedFrames++;
			}
			m_frame++;
		}
	}
}
