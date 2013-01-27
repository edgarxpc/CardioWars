using UnityEngine;
using System.Collections;

public class GameStateEndGood : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static AudioManager Audio { get; set; }

	private void Awake()
	{
		Audio = GetComponent<AudioManager>();
		GameStateIntro.Audio.playSceneWin();

		DontDestroyOnLoad(this);
	}
}
