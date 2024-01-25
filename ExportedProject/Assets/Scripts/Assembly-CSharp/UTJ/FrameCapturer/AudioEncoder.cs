using System.Collections.Generic;

namespace UTJ.FrameCapturer
{
	public abstract class AudioEncoder : EncoderBase
	{
		public enum Type
		{
			Wave = 0,
			Ogg = 1,
			Flac = 2
		}

		public abstract Type type { get; }

		public static Type[] GetAvailableEncoderTypes()
		{
			List<Type> list = new List<Type>();
			if ((bool)fcAPI.fcWaveIsSupported())
			{
				list.Add(Type.Wave);
			}
			if ((bool)fcAPI.fcOggIsSupported())
			{
				list.Add(Type.Ogg);
			}
			if ((bool)fcAPI.fcFlacIsSupported())
			{
				list.Add(Type.Flac);
			}
			return list.ToArray();
		}

		public abstract void Initialize(object config, string outPath);

		public abstract void AddAudioSamples(float[] samples);

		public static AudioEncoder Create(Type t)
		{
			switch (t)
			{
			case Type.Wave:
				return new WaveEncoder();
			case Type.Ogg:
				return new OggEncoder();
			case Type.Flac:
				return new FlacEncoder();
			default:
				return null;
			}
		}

		public static AudioEncoder Create(AudioEncoderConfigs c, string path)
		{
			AudioEncoder audioEncoder = Create(c.format);
			switch (c.format)
			{
			case Type.Wave:
				audioEncoder.Initialize(c.waveEncoderSettings, path);
				break;
			case Type.Ogg:
				audioEncoder.Initialize(c.oggEncoderSettings, path);
				break;
			case Type.Flac:
				audioEncoder.Initialize(c.flacEncoderSettings, path);
				break;
			}
			return audioEncoder;
		}
	}
}
