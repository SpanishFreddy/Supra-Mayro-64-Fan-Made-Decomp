using System;
using UnityEngine;

namespace UTJ.FrameCapturer
{
	[AddComponentMenu("UTJ/FrameCapturer/Audio Recorder")]
	[RequireComponent(typeof(AudioListener))]
	[ExecuteInEditMode]
	public class AudioRecorder : RecorderBase
	{
		[SerializeField]
		private AudioEncoderConfigs m_encoderConfigs = new AudioEncoderConfigs();

		private AudioEncoder m_encoder;

		public override bool BeginRecording()
		{
			if (m_recording)
			{
				return false;
			}
			m_outputDir.CreateDirectory();
			string path = m_outputDir.GetFullPath() + "/" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
			m_encoderConfigs.Setup();
			m_encoder = AudioEncoder.Create(m_encoderConfigs, path);
			if (m_encoder == null || !m_encoder.IsValid())
			{
				EndRecording();
				return false;
			}
			base.BeginRecording();
			Debug.Log("AudioMRecorder: BeginRecording()");
			return true;
		}

		public override void EndRecording()
		{
			if (m_encoder != null)
			{
				m_encoder.Release();
				m_encoder = null;
			}
			if (m_recording)
			{
				Debug.Log("AudioMRecorder: EndRecording()");
			}
			base.EndRecording();
		}

		private void LateUpdate()
		{
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
