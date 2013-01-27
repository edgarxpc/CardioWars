using UnityEngine;
using System.Collections;

public class GameStateIntro : MonoBehaviour {

	public static AudioManager Audio { get; set; }

	private void Awake()
	{
		Audio = GetComponent<AudioManager>();
		GameStateIntro.Audio.playThemeIntro();

		DontDestroyOnLoad(this);
	}
}
