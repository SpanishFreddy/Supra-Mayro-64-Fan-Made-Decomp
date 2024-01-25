using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
	[AddComponentMenu("")]
	public class AudioResponse : ResponseComponent
	{
		public enum ResponseType
		{
			AudioSource = 0,
			SoundEffect = 1
		}

		public enum AudioSourceAction
		{
			Play = 0,
			Stop = 1,
			Restart = 2,
			ChangeVolume = 3
		}

		public AudioSource audioSource;

		public AudioClip playMusic;

		public bool loopMusic;

		public float musicVolume = 1f;

		public AudioClip playSoundEffect;

		public Transform soundEffectPosition;

		public ResponseType responseType;

		public AudioSourceAction audioSourceAction;

		public override void Validation()
		{
			if (responseType == ResponseType.AudioSource)
			{
				switch (audioSourceAction)
				{
				case AudioSourceAction.Stop:
					if (!audioSource)
					{
						ShowWarningMessage("You have chosen to stop an audio source but haven't specified an Audio Source.");
					}
					break;
				case AudioSourceAction.ChangeVolume:
					if (!audioSource)
					{
						ShowWarningMessage("You have chosen to change the volume on an audio source but haven't specified an Audio Source.");
					}
					break;
				case AudioSourceAction.Play:
					if (!playMusic)
					{
						ShowWarningMessage("You have chosen to play an audio source but haven't specified an Audio Clip.");
					}
					if (!audioSource)
					{
						ShowWarningMessage("You have chosen to play an audio source but haven't specified an Audio Source.");
					}
					break;
				case AudioSourceAction.Restart:
					if (!audioSource)
					{
						ShowWarningMessage("You have chosen to restart an audio source but haven't specified an Audio Source.");
					}
					break;
				}
			}
			else if (!soundEffectPosition)
			{
				ShowWarningMessage("You have chosen to play a sound effect but haven't set a position for it to play at!");
			}
		}

		public override bool ExecuteAction()
		{
			if (responseType == ResponseType.AudioSource)
			{
				switch (audioSourceAction)
				{
				case AudioSourceAction.Stop:
					audioSource.Stop();
					break;
				case AudioSourceAction.ChangeVolume:
					if (duration != 0f)
					{
						activeCoroutines.Add(StartCoroutine(ChangeVolumeOverTime()));
					}
					else
					{
						audioSource.volume = musicVolume;
					}
					break;
				case AudioSourceAction.Play:
					audioSource.loop = loopMusic;
					audioSource.volume = musicVolume;
					if ((bool)playMusic)
					{
						audioSource.clip = playMusic;
					}
					audioSource.Play();
					break;
				case AudioSourceAction.Restart:
					audioSource.time = 0f;
					break;
				}
			}
			else
			{
				AudioSource.PlayClipAtPoint(playSoundEffect, soundEffectPosition.position, musicVolume);
			}
			return false;
		}

		private IEnumerator ChangeVolumeOverTime()
		{
			float smoothness = 0.02f;
			float progress = 0f;
			float increment = smoothness / duration;
			float startVolume = audioSource.volume;
			while (progress < 1f)
			{
				audioSource.volume = Mathf.Lerp(startVolume, musicVolume, progress);
				progress += increment;
				yield return new WaitForSeconds(smoothness);
			}
		}
	}
}
