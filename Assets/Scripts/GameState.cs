using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
    public int InitialWave = 1;
    public int InitialMoney = 100;

    public static int WaveNumber { get; set; }
    public static int AvailableMoney { get; set; }
    public static LevelState CurrentState { get; set; }
	public static TowerTypes TowerSelected { get; set; }
	public static AudioManager Audio { get; set; }

    private void Awake()
    {
        WaveNumber = InitialWave;
        AvailableMoney = InitialMoney;
        TowerSelected = TowerTypes.None;
		Audio = GetComponent<AudioManager>();
        CurrentState = LevelState.WaveStarting;
		
        GameState.Audio.playThemeGame();

        DontDestroyOnLoad(this);
    }
}
