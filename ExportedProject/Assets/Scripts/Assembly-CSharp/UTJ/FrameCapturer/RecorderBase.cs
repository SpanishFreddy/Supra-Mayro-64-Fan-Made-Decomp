using System.Collections;
using System.Threading;
using UnityEngine;

namespace UTJ.FrameCapturer
{
	[ExecuteInEditMode]
	public abstract class RecorderBase : MonoBehaviour
	{
		public enum ResolutionUnit
		{
			Percent = 0,
			Pixels = 1
		}

		public enum FrameRateMode
		{
			Variable = 0,
			Constant = 1
		}

		public enum CaptureControl
		{
			Manual = 0,
			FrameRange = 1,
			TimeRange = 2
		}

		[SerializeField]
		protected DataPath m_outputDir = new DataPath(DataPath.Root.Current, "Capture");

		[SerializeField]
		protected ResolutionUnit m_resolution;

		[SerializeField]
		[Range(1f, 100f)]
		protected int m_resolutionPercent = 100;

		[SerializeField]
		protected int m_resolutionWidth = 1920;

		[SerializeField]
		protected FrameRateMode m_framerateMode = FrameRateMode.Constant;

		[SerializeField]
		protected int m_targetFramerate = 30;

		[SerializeField]
		protected bool m_fixDeltaTime = true;

		[SerializeField]
		protected bool m_waitDeltaTime = true;

		[SerializeField]
		[Range(1f, 10f)]
		protected int m_captureEveryNthFrame = 1;

		[SerializeField]
		protected CaptureControl m_captureControl = CaptureControl.FrameRange;

		[SerializeField]
		protected int m_startFrame;

		[SerializeField]
		protected int m_endFrame = 100;

		[SerializeField]
		protected float m_startTime;

		[SerializeField]
		protected float m_endTime = 10f;

		[SerializeField]
		private bool m_recordOnStart;

		protected bool m_recording;

		protected bool m_aborted;

		protected int m_initialFrame;

		protected float m_initialTime;

		protected float m_initialRealTime;

		protected int m_frame;

		protected int m_recordedFrames;

		protected int m_recordedSamples;

		public DataPath outputDir
		{
			get
			{
				return m_outputDir;
			}
			set
			{
				m_outputDir = value;
			}
		}

		public ResolutionUnit resolutionUnit
		{
			get
			{
				return m_resolution;
			}
			set
			{
				m_resolution = value;
			}
		}

		public int resolutionPercent
		{
			get
			{
				return m_resolutionPercent;
			}
			set
			{
				m_resolutionPercent = value;
			}
		}

		public int resolutionWidth
		{
			get
			{
				return m_resolutionWidth;
			}
			set
			{
				m_resolutionWidth = value;
			}
		}

		public FrameRateMode framerateMode
		{
			get
			{
				return m_framerateMode;
			}
			set
			{
				m_framerateMode = value;
			}
		}

		public int targetFramerate
		{
			get
			{
				return m_targetFramerate;
			}
			set
			{
				m_targetFramerate = value;
			}
		}

		public bool fixDeltaTime
		{
			get
			{
				return m_fixDeltaTime;
			}
			set
			{
				m_fixDeltaTime = value;
			}
		}

		public bool waitDeltaTime
		{
			get
			{
				return m_waitDeltaTime;
			}
			set
			{
				m_waitDeltaTime = value;
			}
		}

		public int captureEveryNthFrame
		{
			get
			{
				return m_captureEveryNthFrame;
			}
			set
			{
				m_captureEveryNthFrame = value;
			}
		}

		public CaptureControl captureControl
		{
			get
			{
				return m_captureControl;
			}
			set
			{
				m_captureControl = value;
			}
		}

		public int startFrame
		{
			get
			{
				return m_startFrame;
			}
			set
			{
				m_startFrame = value;
			}
		}

		public int endFrame
		{
			get
			{
				return m_endFrame;
			}
			set
			{
				m_endFrame = value;
			}
		}

		public float startTime
		{
			get
			{
				return m_startTime;
			}
			set
			{
				m_startTime = value;
			}
		}

		public float endTime
		{
			get
			{
				return m_endTime;
			}
			set
			{
				m_endTime = value;
			}
		}

		public bool isRecording
		{
			get
			{
				return m_recording;
			}
			set
			{
				if (value)
				{
					BeginRecording();
				}
				else
				{
					EndRecording();
				}
			}
		}

		public bool recordOnStart
		{
			set
			{
				m_recordOnStart = value;
			}
		}

		public virtual bool BeginRecording()
		{
			if (m_recording)
			{
				return false;
			}
			if (m_framerateMode == FrameRateMode.Constant && m_fixDeltaTime)
			{
				Time.maximumDeltaTime = 1f / (float)m_targetFramerate;
				if (!m_waitDeltaTime)
				{
					Time.captureFramerate = m_targetFramerate;
				}
			}
			m_initialFrame = Time.renderedFrameCount;
			m_initialTime = Time.unscaledTime;
			m_initialRealTime = Time.realtimeSinceStartup;
			m_recordedFrames = 0;
			m_recordedSamples = 0;
			m_recording = true;
			return true;
		}

		public virtual void EndRecording()
		{
			if (m_recording)
			{
				if (m_framerateMode == FrameRateMode.Constant && m_fixDeltaTime && !m_waitDeltaTime)
				{
					Time.captureFramerate = 0;
				}
				m_recording = false;
				m_aborted = true;
			}
		}

		protected void GetCaptureResolution(ref int w, ref int h)
		{
			if (m_resolution == ResolutionUnit.Percent)
			{
				float num = (float)m_resolutionPercent * 0.01f;
				w = (int)((float)w * num);
				h = (int)((float)h * num);
			}
			else
			{
				float num2 = (float)h / (float)w;
				w = m_resolutionWidth;
				h = (int)((float)m_resolutionWidth * num2);
			}
		}

		protected IEnumerator Wait()
		{
			yield return new WaitForEndOfFrame();
			float wt = 1f / (float)m_targetFramerate * (float)(Time.renderedFrameCount - m_initialFrame);
			while (Time.realtimeSinceStartup - m_initialRealTime < wt)
			{
				Thread.Sleep(1);
			}
		}

		protected virtual void Start()
		{
			m_initialFrame = Time.renderedFrameCount;
			m_initialTime = Time.unscaledTime;
			m_initialRealTime = Time.realtimeSinceStartup;
			if (m_recordOnStart)
			{
				BeginRecording();
			}
			m_recordOnStart = false;
		}

		protected virtual void OnDisable()
		{
			EndRecording();
		}

		protected virtual void Update()
		{
			if (m_captureControl == CaptureControl.FrameRange)
			{
				if (!m_aborted && m_frame >= m_startFrame && m_frame <= m_endFrame)
				{
					if (!m_recording)
					{
						BeginRecording();
					}
				}
				else if (m_recording)
				{
					EndRecording();
				}
			}
			else if (m_captureControl == CaptureControl.TimeRange)
			{
				float num = Time.unscaledTime - m_initialTime;
				if (!m_aborted && num >= m_startTime && num <= m_endTime)
				{
					if (!m_recording)
					{
						BeginRecording();
					}
				}
				else if (m_recording)
				{
					EndRecording();
				}
			}
			else if (m_captureControl != 0)
			{
			}
			if (m_framerateMode == FrameRateMode.Constant && m_fixDeltaTime && m_waitDeltaTime)
			{
				StartCoroutine(Wait());
			}
		}
	}
}
