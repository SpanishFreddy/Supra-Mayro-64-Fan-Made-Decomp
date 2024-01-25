using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace UTJ.FrameCapturer
{
	[AddComponentMenu("UTJ/FrameCapturer/Movie Recorder")]
	[RequireComponent(typeof(Camera))]
	[ExecuteInEditMode]
	public class MovieRecorder : RecorderBase
	{
		public enum CaptureTarget
		{
			FrameBuffer = 0,
			RenderTexture = 1
		}

		[SerializeField]
		private MovieEncoderConfigs m_encoderConfigs = new MovieEncoderConfigs(MovieEncoder.Type.WebM);

		[SerializeField]
		private CaptureTarget m_captureTarget;

		[SerializeField]
		private RenderTexture m_targetRT;

		[SerializeField]
		private bool m_captureVideo = true;

		[SerializeField]
		private bool m_captureAudio = true;

		[SerializeField]
		private Shader m_shCopy;

		private Material m_matCopy;

		private Mesh m_quad;

		private CommandBuffer m_cb;

		private RenderTexture m_scratchBuffer;

		private MovieEncoder m_encoder;

		public CaptureTarget captureTarget
		{
			get
			{
				return m_captureTarget;
			}
			set
			{
				m_captureTarget = value;
			}
		}

		public RenderTexture targetRT
		{
			get
			{
				return m_targetRT;
			}
			set
			{
				m_targetRT = value;
			}
		}

		public bool captureAudio
		{
			get
			{
				return m_captureAudio;
			}
			set
			{
				m_captureAudio = value;
			}
		}

		public bool captureVideo
		{
			get
			{
				return m_captureVideo;
			}
			set
			{
				m_captureVideo = value;
			}
		}

		public bool supportVideo
		{
			get
			{
				return m_encoderConfigs.supportVideo;
			}
		}

		public bool supportAudio
		{
			get
			{
				return m_encoderConfigs.supportAudio;
			}
		}

		public RenderTexture scratchBuffer
		{
			get
			{
				return m_scratchBuffer;
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
				Debug.LogError("MovieRecorder: copy shader is missing!");
				return false;
			}
			if (m_captureTarget == CaptureTarget.RenderTexture && m_targetRT == null)
			{
				Debug.LogError("MovieRecorder: target RenderTexture is null!");
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
			m_scratchBuffer = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32);
			m_scratchBuffer.wrapMode = TextureWrapMode.Repeat;
			m_scratchBuffer.Create();
			int targetFrameRate = 60;
			if (m_framerateMode == FrameRateMode.Constant)
			{
				targetFrameRate = m_targetFramerate;
			}
			string path = m_outputDir.GetFullPath() + "/" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
			m_encoderConfigs.captureVideo = m_captureVideo;
			m_encoderConfigs.captureAudio = m_captureAudio;
			m_encoderConfigs.Setup(m_scratchBuffer.width, m_scratchBuffer.height, 3, targetFrameRate);
			m_encoder = MovieEncoder.Create(m_encoderConfigs, path);
			if (m_encoder == null || !m_encoder.IsValid())
			{
				EndRecording();
				return false;
			}
			int num = Shader.PropertyToID("_TmpFrameBuffer");
			m_cb = new CommandBuffer();
			m_cb.name = "MovieRecorder: copy frame buffer";
			if (m_captureTarget == CaptureTarget.FrameBuffer)
			{
				m_cb.GetTemporaryRT(num, -1, -1, 0, FilterMode.Bilinear);
				m_cb.Blit(BuiltinRenderTextureType.CurrentActive, num);
				m_cb.SetRenderTarget(m_scratchBuffer);
				m_cb.DrawMesh(m_quad, Matrix4x4.identity, m_matCopy, 0, 0);
				m_cb.ReleaseTemporaryRT(num);
			}
			else if (m_captureTarget == CaptureTarget.RenderTexture)
			{
				m_cb.SetRenderTarget(m_scratchBuffer);
				m_cb.SetGlobalTexture("_TmpRenderTarget", m_targetRT);
				m_cb.DrawMesh(m_quad, Matrix4x4.identity, m_matCopy, 0, 1);
			}
			component.AddCommandBuffer(CameraEvent.AfterEverything, m_cb);
			base.BeginRecording();
			Debug.Log("MovieRecorder: BeginRecording()");
			return true;
		}

		public override void EndRecording()
		{
			if (m_encoder != null)
			{
				m_encoder.Release();
				m_encoder = null;
			}
			if (m_cb != null)
			{
				GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.AfterEverything, m_cb);
				m_cb.Release();
				m_cb = null;
			}
			if (m_scratchBuffer != null)
			{
				m_scratchBuffer.Release();
				m_scratchBuffer = null;
			}
			if (m_recording)
			{
				Debug.Log("MovieRecorder: EndRecording()");
			}
			base.EndRecording();
		}

		private IEnumerator OnPostRender()
		{
			if (m_recording && m_encoder != null && Time.frameCount % m_captureEveryNthFrame == 0)
			{
				yield return new WaitForEndOfFrame();
				double timestamp = Time.unscaledTime - m_initialTime;
				if (m_framerateMode == FrameRateMode.Constant)
				{
					timestamp = 1.0 / (double)m_targetFramerate * (double)m_recordedFrames;
				}
				fcAPI.fcLock(m_scratchBuffer, TextureFormat.RGB24, delegate(byte[] data, fcAPI.fcPixelFormat fmt)
				{
					m_encoder.AddVideoFrame(data, fmt, timestamp);
				});
				m_recordedFrames++;
			}
			m_frame++;
		}

		private void OnAudioFilterRead(float[] samples, int channels)
		{
			if (m_recording && m_encoder != null)
			{
				m_encoder.AddAudioSamples(samples);
				m_recordedSamples += samples.Length;
			}
		}
	}
}
