using System;
using UnityEngine;

namespace UTJ.FrameCapturer
{
	public class ExrEncoder : MovieEncoder
	{
		private static readonly string[] s_channelNames = new string[4] { "R", "G", "B", "A" };

		private fcAPI.fcExrContext m_ctx;

		private fcAPI.fcExrConfig m_config;

		private string m_outPath;

		private int m_frame;

		public override Type type
		{
			get
			{
				return Type.Exr;
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
			if (!fcAPI.fcExrIsSupported())
			{
				Debug.LogError("Exr encoder is not available on this platform.");
				return;
			}
			m_config = (fcAPI.fcExrConfig)config;
			m_ctx = fcAPI.fcExrCreateContext(ref m_config);
			m_outPath = outPath;
			m_frame = 0;
		}

		public override void AddVideoFrame(byte[] frame, fcAPI.fcPixelFormat format, double timestamp = -1.0)
		{
			if ((bool)m_ctx)
			{
				string path = m_outPath + "_" + m_frame.ToString("0000") + ".exr";
				int num = Math.Min(m_config.channels, (int)(format & (fcAPI.fcPixelFormat)7));
				fcAPI.fcExrBeginImage(m_ctx, path, m_config.width, m_config.height);
				for (int i = 0; i < num; i++)
				{
					fcAPI.fcExrAddLayerPixels(m_ctx, frame, format, i, s_channelNames[i]);
				}
				fcAPI.fcExrEndImage(m_ctx);
			}
			m_frame++;
		}

		public override void AddAudioSamples(float[] samples)
		{
		}
	}
}
