using UnityEngine;

public class NoisePlayer : MonoBehaviour
{
	public Transform noisegenerator;

	public AudioClip[] sounds;

	public AudioSource audioDevice;

	private void Start()
	{
	}

	public void PlayAudio()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, 45f));
		if (!audioDevice.isPlaying && num == 0)
		{
			base.transform.position = noisegenerator.position;
			int num2 = Mathf.RoundToInt(Random.Range(0f, sounds.Length - 1));
			audioDevice.PlayOneShot(sounds[num2]);
		}
	}
}
