using UnityEngine;

public class SoundPlayOnButtonPress : MonoBehaviour
{
	public string ButtonDown = string.Empty;
	private AudioSource ThisAudio = null;

	void Awake()
	{
		ThisAudio = GetComponent<AudioSource>();
	}

	void Update () 
	{
		if (Input.GetButtonDown(ButtonDown))
		{
			ThisAudio.PlayOneShot(ThisAudio.clip);
		}
	}
}