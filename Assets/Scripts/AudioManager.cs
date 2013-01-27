using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioClip HearthBeat1;
	public AudioClip HearthBeat2;
	public AudioClip ThemeIntro;
	public AudioClip ThemeGame;
	public AudioClip ShootCholesterol;
	public AudioClip ShootWorm;
	public AudioClip ShootTumor;
	public AudioSource Source;
	
	public AudioClip SceneWin;

	public void playHearthBeat1()
	{
		Source.PlayOneShot(HearthBeat1, 1);
	}

	public void playHearthBeat2()
	{
		Source.PlayOneShot(HearthBeat2, 1);
	}

	public void playThemeIntro()
	{
		Source.clip = ThemeIntro;
		Source.loop = true;
		Source.Play();
	}
	
	public void playSceneWin()
	{
		Source.clip = SceneWin;
		Source.loop = true;
		Source.Play();
	}

	public void playThemeGame()
	{
		Source.clip = ThemeGame;
		Source.loop = true;
		Source.Play();
	}

	public void playShootCholesterol()
	{
		Source.PlayOneShot(ShootCholesterol, 1);
	}

	public void playShootWorm()
	{
		Source.PlayOneShot(ShootWorm, 1);
	}

	public void playShootTumor()
	{
		Source.PlayOneShot(ShootTumor, 1);
	}
}
