using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UTJ.FrameCapturer
{
	public static class fcAPI
	{
		public enum fcPixelFormat
		{
			Unknown = 0,
			ChannelMask = 15,
			TypeMask = 240,
			Type_f16 = 16,
			Type_f32 = 32,
			Type_u8 = 48,
			Type_i16 = 64,
			Type_i32 = 80,
			Rf16 = 17,
			RGf16 = 18,
			RGBf16 = 19,
			RGBAf16 = 20,
			Rf32 = 33,
			RGf32 = 34,
			RGBf32 = 35,
			RGBAf32 = 36,
			Ru8 = 49,
			RGu8 = 50,
			RGBu8 = 51,
			RGBAu8 = 52,
			Ri16 = 65,
			RGi16 = 66,
			RGBi16 = 67,
			RGBAi16 = 68,
			Ri32 = 81,
			RGi32 = 82,
			RGBi32 = 83,
			RGBAi32 = 84
		}

		public enum fcBitrateMode
		{
			CBR = 0,
			VBR = 1
		}

		public enum fcAudioBitsPerSample
		{
			_8Bits = 8,
			_16Bits = 16,
			_24Bits = 24
		}

		public struct fcDeferredCall
		{
			public int handle;

			public void Release()
			{
				fcReleaseDeferredCall(this);
				handle = 0;
			}

			public static implicit operator int(fcDeferredCall v)
			{
				return v.handle;
			}
		}

		public struct fcStream
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseStream(this);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcStream v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		public enum fcPngPixelFormat
		{
			Auto = 0,
			UInt8 = 1,
			UInt16 = 2
		}

		[Serializable]
		public struct fcPngConfig
		{
			public fcPngPixelFormat pixelFormat;

			[Range(1f, 32f)]
			public int maxTasks;

			[HideInInspector]
			public int width;

			[HideInInspector]
			public int height;

			[HideInInspector]
			public int channels;

			public static fcPngConfig default_value
			{
				get
				{
					fcPngConfig result = default(fcPngConfig);
					result.pixelFormat = fcPngPixelFormat.Auto;
					result.maxTasks = 2;
					return result;
				}
			}
		}

		public struct fcPngContext
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcPngContext v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		public enum fcExrPixelFormat
		{
			Auto = 0,
			Half = 1,
			Float = 2,
			Int = 3
		}

		public enum fcExrCompression
		{
			None = 0,
			RLE = 1,
			ZipS = 2,
			Zip = 3,
			PIZ = 4
		}

		[Serializable]
		public struct fcExrConfig
		{
			public fcExrPixelFormat pixelFormat;

			public fcExrCompression compression;

			[Range(1f, 32f)]
			public int maxTasks;

			[HideInInspector]
			public int width;

			[HideInInspector]
			public int height;

			[HideInInspector]
			public int channels;

			public static fcExrConfig default_value
			{
				get
				{
					fcExrConfig result = default(fcExrConfig);
					result.pixelFormat = fcExrPixelFormat.Auto;
					result.compression = fcExrCompression.Zip;
					result.maxTasks = 2;
					return result;
				}
			}
		}

		public struct fcExrContext
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcExrContext v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		[Serializable]
		public struct fcGifConfig
		{
			[HideInInspector]
			public int width;

			[HideInInspector]
			public int height;

			[Range(1f, 256f)]
			public int numColors;

			[Range(1f, 120f)]
			public int keyframeInterval;

			[Range(1f, 32f)]
			public int maxTasks;

			public static fcGifConfig default_value
			{
				get
				{
					fcGifConfig result = default(fcGifConfig);
					result.numColors = 256;
					result.maxTasks = 8;
					result.keyframeInterval = 30;
					return result;
				}
			}
		}

		public struct fcGifContext
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcGifContext v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		public enum fcMP4VideoFlags
		{
			H264NVIDIA = 2,
			H264AMD = 4,
			H264IntelHW = 8,
			H264IntelSW = 16,
			H264OpenH264 = 32,
			H264Mask = 62
		}

		public enum fcMP4AudioFlags
		{
			AACIntel = 2,
			AACFAAC = 4,
			AACMask = 6
		}

		[Serializable]
		public struct fcMP4Config
		{
			[HideInInspector]
			public Bool video;

			[HideInInspector]
			public int videoWidth;

			[HideInInspector]
			public int videoHeight;

			[HideInInspector]
			public int videoTargetFramerate;

			public fcBitrateMode videoBitrateMode;

			public int videoTargetBitrate;

			[HideInInspector]
			public int videoFlags;

			[Range(1f, 32f)]
			public int videoMaxTasks;

			[HideInInspector]
			public Bool audio;

			[HideInInspector]
			public int audioSampleRate;

			[HideInInspector]
			public int audioNumChannels;

			public fcBitrateMode audioBitrateMode;

			public int audioTargetBitrate;

			[HideInInspector]
			public int audioFlags;

			[Range(1f, 32f)]
			public int audioMaxTasks;

			public static fcMP4Config default_value
			{
				get
				{
					fcMP4Config result = default(fcMP4Config);
					result.video = true;
					result.videoBitrateMode = fcBitrateMode.VBR;
					result.videoTargetBitrate = 1024000;
					result.videoTargetFramerate = 30;
					result.videoFlags = 62;
					result.videoMaxTasks = 4;
					result.audio = true;
					result.audioSampleRate = 48000;
					result.audioNumChannels = 2;
					result.audioBitrateMode = fcBitrateMode.VBR;
					result.audioTargetBitrate = 128000;
					result.audioFlags = 6;
					result.audioMaxTasks = 4;
					return result;
				}
			}
		}

		public struct fcMP4Context
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcMP4Context v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		public struct fcWebMContext
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcWebMContext v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		public enum fcWebMVideoEncoder
		{
			VP8 = 0,
			VP9 = 1,
			VP9LossLess = 2
		}

		public enum fcWebMAudioEncoder
		{
			Vorbis = 0,
			Opus = 1
		}

		[Serializable]
		public struct fcWebMConfig
		{
			[HideInInspector]
			public Bool video;

			public fcWebMVideoEncoder videoEncoder;

			[HideInInspector]
			public int videoWidth;

			[HideInInspector]
			public int videoHeight;

			[HideInInspector]
			public int videoTargetFramerate;

			public fcBitrateMode videoBitrateMode;

			public int videoTargetBitrate;

			[Range(1f, 32f)]
			public int videoMaxTasks;

			[HideInInspector]
			public Bool audio;

			public fcWebMAudioEncoder audioEncoder;

			[HideInInspector]
			public int audioSampleRate;

			[HideInInspector]
			public int audioNumChannels;

			public fcBitrateMode audioBitrateMode;

			public int audioTargetBitrate;

			[Range(1f, 32f)]
			public int audioMaxTasks;

			public static fcWebMConfig default_value
			{
				get
				{
					fcWebMConfig result = default(fcWebMConfig);
					result.video = true;
					result.videoEncoder = fcWebMVideoEncoder.VP8;
					result.videoTargetFramerate = 60;
					result.videoBitrateMode = fcBitrateMode.VBR;
					result.videoTargetBitrate = 1024000;
					result.videoMaxTasks = 4;
					result.audio = true;
					result.audioEncoder = fcWebMAudioEncoder.Vorbis;
					result.audioSampleRate = 48000;
					result.audioNumChannels = 2;
					result.audioBitrateMode = fcBitrateMode.VBR;
					result.audioTargetBitrate = 128000;
					result.audioMaxTasks = 4;
					return result;
				}
			}
		}

		public struct fcWaveContext
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcWaveContext v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		[Serializable]
		public struct fcWaveConfig
		{
			[HideInInspector]
			public int sampleRate;

			[HideInInspector]
			public int numChannels;

			public fcAudioBitsPerSample bitsPerSample;

			[Range(1f, 32f)]
			public int maxTasks;

			public static fcWaveConfig default_value
			{
				get
				{
					fcWaveConfig result = default(fcWaveConfig);
					result.sampleRate = 48000;
					result.numChannels = 2;
					result.bitsPerSample = fcAudioBitsPerSample._16Bits;
					result.maxTasks = 2;
					return result;
				}
			}
		}

		public struct fcOggContext
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcOggContext v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		[Serializable]
		public struct fcOggConfig
		{
			[HideInInspector]
			public int sampleRate;

			[HideInInspector]
			public int numChannels;

			public fcBitrateMode bitrateMode;

			public int targetBitrate;

			[Range(1f, 32f)]
			public int maxTasks;

			public static fcOggConfig default_value
			{
				get
				{
					fcOggConfig result = default(fcOggConfig);
					result.sampleRate = 48000;
					result.numChannels = 2;
					result.bitrateMode = fcBitrateMode.VBR;
					result.targetBitrate = 128000;
					result.maxTasks = 2;
					return result;
				}
			}
		}

		public struct fcFlacContext
		{
			public IntPtr ptr;

			public void Release()
			{
				fcReleaseContext(ptr);
				ptr = IntPtr.Zero;
			}

			public static implicit operator bool(fcFlacContext v)
			{
				return v.ptr != IntPtr.Zero;
			}
		}

		[Serializable]
		public struct fcFlacConfig
		{
			[HideInInspector]
			public int sampleRate;

			[HideInInspector]
			public int numChannels;

			public fcAudioBitsPerSample bitsPerSample;

			[Range(0f, 9f)]
			public int compressionLevel;

			public int blockSize;

			[HideInInspector]
			public Bool verify;

			[Range(1f, 32f)]
			public int maxTasks;

			public static fcFlacConfig default_value
			{
				get
				{
					fcFlacConfig result = default(fcFlacConfig);
					result.sampleRate = 48000;
					result.numChannels = 2;
					result.bitsPerSample = fcAudioBitsPerSample._16Bits;
					result.compressionLevel = 5;
					result.blockSize = 0;
					result.verify = false;
					result.maxTasks = 2;
					return result;
				}
			}
		}

		[DllImport("fccore")]
		public static extern void fcSetModulePath(string path);

		[DllImport("fccore")]
		public static extern double fcGetTime();

		[DllImport("fccore")]
		public static extern fcStream fcCreateFileStream(string path);

		[DllImport("fccore")]
		public static extern fcStream fcCreateMemoryStream();

		[DllImport("fccore")]
		private static extern void fcReleaseStream(fcStream s);

		[DllImport("fccore")]
		public static extern ulong fcStreamGetWrittenSize(fcStream s);

		[DllImport("fccore")]
		public static extern void fcGuardBegin();

		[DllImport("fccore")]
		public static extern void fcGuardEnd();

		[DllImport("fccore")]
		public static extern fcDeferredCall fcAllocateDeferredCall();

		[DllImport("fccore")]
		private static extern void fcReleaseDeferredCall(fcDeferredCall dc);

		[DllImport("fccore")]
		public static extern IntPtr fcGetRenderEventFunc();

		public static void fcGuard(Action body)
		{
			fcGuardBegin();
			body();
			fcGuardEnd();
		}

		public static fcPixelFormat fcGetPixelFormat(RenderTextureFormat v)
		{
			switch (v)
			{
			case RenderTextureFormat.ARGB32:
				return fcPixelFormat.RGBAu8;
			case RenderTextureFormat.ARGBHalf:
				return fcPixelFormat.RGBAf16;
			case RenderTextureFormat.RGHalf:
				return fcPixelFormat.RGf16;
			case RenderTextureFormat.RHalf:
				return fcPixelFormat.Rf16;
			case RenderTextureFormat.ARGBFloat:
				return fcPixelFormat.RGBAf32;
			case RenderTextureFormat.RGFloat:
				return fcPixelFormat.RGf32;
			case RenderTextureFormat.RFloat:
				return fcPixelFormat.Rf32;
			case RenderTextureFormat.ARGBInt:
				return fcPixelFormat.RGBAi32;
			case RenderTextureFormat.RGInt:
				return fcPixelFormat.RGi32;
			case RenderTextureFormat.RInt:
				return fcPixelFormat.Ri32;
			default:
				return fcPixelFormat.Unknown;
			}
		}

		public static fcPixelFormat fcGetPixelFormat(TextureFormat v)
		{
			switch (v)
			{
			case TextureFormat.Alpha8:
				return fcPixelFormat.Ru8;
			case TextureFormat.RGB24:
				return fcPixelFormat.RGBu8;
			case TextureFormat.RGBA32:
				return fcPixelFormat.RGBAu8;
			case TextureFormat.ARGB32:
				return fcPixelFormat.RGBAu8;
			case TextureFormat.RGBAHalf:
				return fcPixelFormat.RGBAf16;
			case TextureFormat.RGHalf:
				return fcPixelFormat.RGf16;
			case TextureFormat.RHalf:
				return fcPixelFormat.Rf16;
			case TextureFormat.RGBAFloat:
				return fcPixelFormat.RGBAf32;
			case TextureFormat.RGFloat:
				return fcPixelFormat.RGf32;
			case TextureFormat.RFloat:
				return fcPixelFormat.Rf32;
			default:
				return fcPixelFormat.Unknown;
			}
		}

		public static int fcGetNumAudioChannels()
		{
			switch (AudioSettings.speakerMode)
			{
			case AudioSpeakerMode.Mono:
				return 1;
			case AudioSpeakerMode.Stereo:
				return 2;
			case AudioSpeakerMode.Quad:
				return 4;
			case AudioSpeakerMode.Surround:
				return 5;
			case AudioSpeakerMode.Mode5point1:
				return 6;
			case AudioSpeakerMode.Mode7point1:
				return 8;
			case AudioSpeakerMode.Prologic:
				return 6;
			default:
				return 0;
			}
		}

		[DllImport("fccore")]
		public static extern void fcEnableAsyncReleaseContext(Bool v);

		[DllImport("fccore")]
		public static extern void fcWaitAsyncDelete();

		[DllImport("fccore")]
		public static extern void fcReleaseContext(IntPtr ctx);

		[DllImport("fccore")]
		public static extern Bool fcPngIsSupported();

		[DllImport("fccore")]
		public static extern fcPngContext fcPngCreateContext(ref fcPngConfig conf);

		[DllImport("fccore")]
		public static extern Bool fcPngExportPixels(fcPngContext ctx, string path, byte[] pixels, int width, int height, fcPixelFormat fmt, int num_channels);

		[DllImport("fccore")]
		public static extern Bool fcExrIsSupported();

		[DllImport("fccore")]
		public static extern fcExrContext fcExrCreateContext(ref fcExrConfig conf);

		[DllImport("fccore")]
		public static extern Bool fcExrBeginImage(fcExrContext ctx, string path, int width, int height);

		[DllImport("fccore")]
		public static extern Bool fcExrAddLayerPixels(fcExrContext ctx, byte[] pixels, fcPixelFormat fmt, int ch, string name);

		[DllImport("fccore")]
		public static extern Bool fcExrEndImage(fcExrContext ctx);

		[DllImport("fccore")]
		public static extern Bool fcGifIsSupported();

		[DllImport("fccore")]
		public static extern fcGifContext fcGifCreateContext(ref fcGifConfig conf);

		[DllImport("fccore")]
		public static extern void fcGifAddOutputStream(fcGifContext ctx, fcStream stream);

		[DllImport("fccore")]
		public static extern Bool fcGifAddFramePixels(fcGifContext ctx, byte[] pixels, fcPixelFormat fmt, double timestamp = -1.0);

		[DllImport("fccore")]
		public static extern Bool fcMP4IsSupported();

		[DllImport("fccore")]
		public static extern Bool fcMP4OSIsSupported();

		[DllImport("fccore")]
		public static extern fcMP4Context fcMP4CreateContext(ref fcMP4Config conf);

		[DllImport("fccore")]
		public static extern fcMP4Context fcMP4OSCreateContext(ref fcMP4Config conf, string path);

		[DllImport("fccore")]
		public static extern void fcMP4AddOutputStream(fcMP4Context ctx, fcStream s);

		[DllImport("fccore")]
		private static extern IntPtr fcMP4GetAudioEncoderInfo(fcMP4Context ctx);

		[DllImport("fccore")]
		private static extern IntPtr fcMP4GetVideoEncoderInfo(fcMP4Context ctx);

		[DllImport("fccore")]
		public static extern Bool fcMP4AddVideoFramePixels(fcMP4Context ctx, byte[] pixels, fcPixelFormat fmt, double timestamp = -1.0);

		[DllImport("fccore")]
		public static extern Bool fcMP4AddAudioSamples(fcMP4Context ctx, float[] samples, int num_samples);

		public static string fcMP4GetAudioEncoderInfoS(fcMP4Context ctx)
		{
			return Marshal.PtrToStringAnsi(fcMP4GetAudioEncoderInfo(ctx));
		}

		public static string fcMP4GetVideoEncoderInfoS(fcMP4Context ctx)
		{
			return Marshal.PtrToStringAnsi(fcMP4GetVideoEncoderInfo(ctx));
		}

		[DllImport("fccore")]
		public static extern Bool fcWebMIsSupported();

		[DllImport("fccore")]
		public static extern fcWebMContext fcWebMCreateContext(ref fcWebMConfig conf);

		[DllImport("fccore")]
		public static extern void fcWebMAddOutputStream(fcWebMContext ctx, fcStream stream);

		[DllImport("fccore")]
		public static extern Bool fcWebMAddVideoFramePixels(fcWebMContext ctx, byte[] pixels, fcPixelFormat fmt, double timestamp = -1.0);

		[DllImport("fccore")]
		public static extern Bool fcWebMAddAudioSamples(fcWebMContext ctx, float[] samples, int num_samples);

		[DllImport("fccore")]
		public static extern Bool fcWaveIsSupported();

		[DllImport("fccore")]
		public static extern fcWaveContext fcWaveCreateContext(ref fcWaveConfig conf);

		[DllImport("fccore")]
		public static extern void fcWaveAddOutputStream(fcWaveContext ctx, fcStream stream);

		[DllImport("fccore")]
		public static extern Bool fcWaveAddAudioSamples(fcWaveContext ctx, float[] samples, int num_samples);

		[DllImport("fccore")]
		public static extern Bool fcOggIsSupported();

		[DllImport("fccore")]
		public static extern fcOggContext fcOggCreateContext(ref fcOggConfig conf);

		[DllImport("fccore")]
		public static extern void fcOggAddOutputStream(fcOggContext ctx, fcStream stream);

		[DllImport("fccore")]
		public static extern Bool fcOggAddAudioSamples(fcOggContext ctx, float[] samples, int num_samples);

		[DllImport("fccore")]
		public static extern Bool fcFlacIsSupported();

		[DllImport("fccore")]
		public static extern fcFlacContext fcFlacCreateContext(ref fcFlacConfig conf);

		[DllImport("fccore")]
		public static extern void fcFlacAddOutputStream(fcFlacContext ctx, fcStream stream);

		[DllImport("fccore")]
		public static extern Bool fcFlacAddAudioSamples(fcFlacContext ctx, float[] samples, int num_samples);

		public static void fcLock(RenderTexture src, TextureFormat dstfmt, Action<byte[], fcPixelFormat> body)
		{
			Texture2D texture2D = new Texture2D(src.width, src.height, dstfmt, false);
			RenderTexture.active = src;
			texture2D.ReadPixels(new Rect(0f, 0f, texture2D.width, texture2D.height), 0, 0, false);
			texture2D.Apply();
			body(texture2D.GetRawTextureData(), fcGetPixelFormat(texture2D.format));
			UnityEngine.Object.Destroy(texture2D);
		}

		public static void fcLock(RenderTexture src, Action<byte[], fcPixelFormat> body)
		{
			TextureFormat dstfmt = TextureFormat.RGBA32;
			switch (src.format)
			{
			case RenderTextureFormat.Depth:
			case RenderTextureFormat.ARGBHalf:
			case RenderTextureFormat.Shadowmap:
			case RenderTextureFormat.ARGB2101010:
			case RenderTextureFormat.DefaultHDR:
			case RenderTextureFormat.RGHalf:
			case RenderTextureFormat.RHalf:
			case RenderTextureFormat.RGB111110Float:
				dstfmt = TextureFormat.RGBAHalf;
				break;
			case RenderTextureFormat.ARGBFloat:
			case RenderTextureFormat.RGFloat:
			case RenderTextureFormat.RFloat:
				dstfmt = TextureFormat.RGBAFloat;
				break;
			}
			fcLock(src, dstfmt, body);
		}

		public static Mesh CreateFullscreenQuad()
		{
			Mesh mesh = new Mesh();
			mesh.vertices = new Vector3[4]
			{
				new Vector3(1f, 1f, 0f),
				new Vector3(-1f, 1f, 0f),
				new Vector3(-1f, -1f, 0f),
				new Vector3(1f, -1f, 0f)
			};
			mesh.triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
			mesh.UploadMeshData(true);
			return mesh;
		}
	}
}
