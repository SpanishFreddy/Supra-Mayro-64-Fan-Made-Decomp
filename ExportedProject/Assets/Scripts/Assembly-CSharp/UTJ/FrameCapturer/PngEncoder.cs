using System;
using UnityEngine;

namespace UTJ.FrameCapturer
{
	public class PngEncoder : MovieEncoder
	{
		private fcAPI.fcPngContext m_ctx;

		private fcAPI.fcPngConfig m_config;

		private string m_outPath;

		private int m_frame;

		public override Type type
		{
			get
			{
				return Type.Png;
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
			if (!fcAPI.fcPngIsSupported())
			{
				Debug.LogError("Png encoder is not available on this platform.");
				return;
			}
			m_config = (fcAPI.fcPngConfig)config;
			m_ctx = fcAPI.fcPngCreateContext(ref m_config);
			m_outPath = outPath;
			m_frame = 0;
		}

		public override void AddVideoFrame(byte[] frame, fcAPI.fcPixelFormat format, double timestamp = -1.0)
		{
			if ((bool)m_ctx)
			{
				string path = m_outPath + "_" + m_frame.ToString("0000") + ".png";
				int num_channels = Math.Min(m_config.channels, (int)(format & (fcAPI.fcPixelFormat)7));
				fcAPI.fcPngExportPixels(m_ctx, path, frame, m_config.width, m_config.height, format, num_channels);
			}
			m_frame++;
		}

		public override void AddAudioSamples(float[] samples)
		{
		}
	}
}
