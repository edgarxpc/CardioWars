using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{

	public enum Tower
	{
		None = 0,
		Cholesterol,
		Tumor,
		Worm
	}

    public int InitialWave = 1;
    public int InitialMoney = 100;

    public static int WaveNumber
    {
        get;
        set;
    }

    public static int AvailableMoney
    {
        get;
        set;
    }

	public static Tower TowerSelected { get; set; }

	public static AudioManager Audio { get; set; }


    private void Awake()
    {
        WaveNumber = InitialWave;
        AvailableMoney = InitialMoney;
		Audio = GetComponent<AudioManager>();
		GameState.Audio.playThemeGame();

        DontDestroyOnLoad(this);
    }

    public static void LevelCompleted()
    { 
    
    }

    public static void GameOver()
    { 
    
    }
}
