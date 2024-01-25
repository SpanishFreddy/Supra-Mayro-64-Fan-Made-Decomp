using UnityEngine;

namespace UTJ.FrameCapturer
{
	public class FlacEncoder : AudioEncoder
	{
		private fcAPI.fcFlacContext m_ctx;

		private fcAPI.fcFlacConfig m_config;

		public override Type type
		{
			get
			{
				return Type.Flac;
			}
		}

		public override void Release()
		{
			m_ctx.Release();
		}

		public override bool IsValid()
		{
			return m_ctx;
		}

		public override void Initialize(object config, string outPath)
		{
			if (!fcAPI.fcFlacIsSupported())
			{
				Debug.LogError("Flac encoder is not available on this platform.");
				return;
			}
			m_config = (fcAPI.fcFlacConfig)config;
			m_config.sampleRate = AudioSettings.outputSampleRate;
			m_config.numChannels = fcAPI.fcGetNumAudioChannels();
			m_ctx = fcAPI.fcFlacCreateContext(ref m_config);
			string path = outPath + ".flac";
			fcAPI.fcStream stream = fcAPI.fcCreateFileStream(path);
			fcAPI.fcFlacAddOutputStream(m_ctx, stream);
			stream.Release();
		}

		public override void AddAudioSamples(float[] samples)
		{
			if ((bool)m_ctx)
			{
				fcAPI.fcFlacAddAudioSamples(m_ctx, samples, samples.Length);
			}
		}
	}
}
