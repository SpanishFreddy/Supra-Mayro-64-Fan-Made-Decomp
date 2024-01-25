using System;

namespace UTJ.FrameCapturer
{
	[Serializable]
	public class MovieEncoderConfigs
	{
		public MovieEncoder.Type format = MovieEncoder.Type.WebM;

		public fcAPI.fcPngConfig pngEncoderSettings = fcAPI.fcPngConfig.default_value;

		public fcAPI.fcExrConfig exrEncoderSettings = fcAPI.fcExrConfig.default_value;

		public fcAPI.fcGifConfig gifEncoderSettings = fcAPI.fcGifConfig.default_value;

		public fcAPI.fcWebMConfig webmEncoderSettings = fcAPI.fcWebMConfig.default_value;

		public fcAPI.fcMP4Config mp4EncoderSettings = fcAPI.fcMP4Config.default_value;

		public bool supportVideo
		{
			get
			{
				return format == MovieEncoder.Type.Png || format == MovieEncoder.Type.Exr || format == MovieEncoder.Type.Gif || format == MovieEncoder.Type.WebM || format == MovieEncoder.Type.MP4;
			}
		}

		public bool supportAudio
		{
			get
			{
				return format == MovieEncoder.Type.WebM || format == MovieEncoder.Type.MP4;
			}
		}

		public bool captureVideo
		{
			get
			{
				switch (format)
				{
				case MovieEncoder.Type.Png:
					return true;
				case MovieEncoder.Type.Exr:
					return true;
				case MovieEncoder.Type.Gif:
					return true;
				case MovieEncoder.Type.WebM:
					return webmEncoderSettings.video;
				case MovieEncoder.Type.MP4:
					return webmEncoderSettings.video;
				default:
					return false;
				}
			}
			set
			{
				webmEncoderSettings.video = (mp4EncoderSettings.video = value);
			}
		}

		public bool captureAudio
		{
			get
			{
				switch (format)
				{
				case MovieEncoder.Type.Png:
					return false;
				case MovieEncoder.Type.Exr:
					return false;
				case MovieEncoder.Type.Gif:
					return false;
				case MovieEncoder.Type.WebM:
					return webmEncoderSettings.audio;
				case MovieEncoder.Type.MP4:
					return webmEncoderSettings.audio;
				default:
					return false;
				}
			}
			set
			{
				webmEncoderSettings.audio = (mp4EncoderSettings.audio = value);
			}
		}

		public MovieEncoderConfigs(MovieEncoder.Type t)
		{
			format = t;
		}

		public void Setup(int w, int h, int ch = 4, int targetFrameRate = 60)
		{
			pngEncoderSettings.width = (exrEncoderSettings.width = (gifEncoderSettings.width = (webmEncoderSettings.videoWidth = (mp4EncoderSettings.videoWidth = w))));
			pngEncoderSettings.height = (exrEncoderSettings.height = (gifEncoderSettings.height = (webmEncoderSettings.videoHeight = (mp4EncoderSettings.videoHeight = h))));
			pngEncoderSettings.channels = (exrEncoderSettings.channels = ch);
			webmEncoderSettings.videoTargetFramerate = (mp4EncoderSettings.videoTargetFramerate = targetFrameRate);
		}
	}
}
