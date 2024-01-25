using System.Collections.Generic;

namespace UTJ.FrameCapturer
{
	public abstract class MovieEncoder : EncoderBase
	{
		public enum Type
		{
			Png = 0,
			Exr = 1,
			Gif = 2,
			WebM = 3,
			MP4 = 4
		}

		public abstract Type type { get; }

		public static Type[] GetAvailableEncoderTypes()
		{
			List<Type> list = new List<Type>();
			if ((bool)fcAPI.fcPngIsSupported())
			{
				list.Add(Type.Png);
			}
			if ((bool)fcAPI.fcExrIsSupported())
			{
				list.Add(Type.Exr);
			}
			if ((bool)fcAPI.fcGifIsSupported())
			{
				list.Add(Type.Gif);
			}
			if ((bool)fcAPI.fcWebMIsSupported())
			{
				list.Add(Type.WebM);
			}
			if ((bool)fcAPI.fcMP4OSIsSupported())
			{
				list.Add(Type.MP4);
			}
			return list.ToArray();
		}

		public abstract void Initialize(object config, string outPath);

		public abstract void AddVideoFrame(byte[] frame, fcAPI.fcPixelFormat format, double timestamp = -1.0);

		public abstract void AddAudioSamples(float[] samples);

		public static MovieEncoder Create(Type t)
		{
			switch (t)
			{
			case Type.Png:
				return new PngEncoder();
			case Type.Exr:
				return new ExrEncoder();
			case Type.Gif:
				return new GifEncoder();
			case Type.WebM:
				return new WebMEncoder();
			case Type.MP4:
				return new MP4Encoder();
			default:
				return null;
			}
		}

		public static MovieEncoder Create(MovieEncoderConfigs c, string path)
		{
			MovieEncoder movieEncoder = Create(c.format);
			switch (c.format)
			{
			case Type.Png:
				movieEncoder.Initialize(c.pngEncoderSettings, path);
				break;
			case Type.Exr:
				movieEncoder.Initialize(c.exrEncoderSettings, path);
				break;
			case Type.Gif:
				movieEncoder.Initialize(c.gifEncoderSettings, path);
				break;
			case Type.WebM:
				movieEncoder.Initialize(c.webmEncoderSettings, path);
				break;
			case Type.MP4:
				movieEncoder.Initialize(c.mp4EncoderSettings, path);
				break;
			}
			return movieEncoder;
		}
	}
}
