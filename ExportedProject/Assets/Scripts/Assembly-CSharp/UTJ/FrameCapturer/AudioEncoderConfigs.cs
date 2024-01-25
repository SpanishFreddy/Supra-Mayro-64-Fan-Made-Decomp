using System;

namespace UTJ.FrameCapturer
{
	[Serializable]
	public class AudioEncoderConfigs
	{
		public AudioEncoder.Type format = AudioEncoder.Type.Flac;

		public fcAPI.fcWaveConfig waveEncoderSettings = fcAPI.fcWaveConfig.default_value;

		public fcAPI.fcOggConfig oggEncoderSettings = fcAPI.fcOggConfig.default_value;

		public fcAPI.fcFlacConfig flacEncoderSettings = fcAPI.fcFlacConfig.default_value;

		public void Setup()
		{
		}
	}
}
