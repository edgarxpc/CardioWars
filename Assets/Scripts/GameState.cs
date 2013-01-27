using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public enum Tower
	{
		None = 0,
		Cholesterol,
		Tumor,
		Worm
	}

    public int InitialWave = 1;
    public int InitialMoney = 100;

    public static int WaveNumber { 
        get; 
        set; 
    }

    public static int AvailableMoney { 
        get; 
        set; 
    }

	public static Tower TowerSelected { get; set; }

    void Awake()
    {
        WaveNumber = InitialWave;
        AvailableMoney = InitialMoney;

        DontDestroyOnLoad(this);
    }
}
