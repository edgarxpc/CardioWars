using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{

	public GUISkin skin;

	public Texture btnTower1 = null;
	public Texture btnTower2 = null;
	public Texture btnTower3 = null;

	public GUIText scoreMoney;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void loadGUIPlay()
	{
		string strMoney = GameState.AvailableMoney.ToString("$0000");
		
		if(!scoreMoney.text.Equals(strMoney))
		{
			scoreMoney.text = strMoney;
		}
		

		if (GUI.Button(new Rect(5, 5, 74, 74), btnTower1))
		{
			GameState.TowerSelected = GameState.Tower.Worm;
//			Debug.Log(string.Format("Hit tower {0}", GameState.Tower.Cholesterol.ToString()));
		}
		else if (GUI.Button(new Rect(84, 5, 74, 74), btnTower2))
		{
			GameState.TowerSelected = GameState.Tower.Cholesterol;
//			Debug.Log(string.Format("Hit tower {0}", GameState.Tower.Tumor.ToString()));
		}
		else if (GUI.Button(new Rect(163, 5, 74, 74), btnTower3))
		{
			GameState.TowerSelected = GameState.Tower.Tumor;
//			Debug.Log(string.Format("Hit tower {0}", GameState.Tower.Worm.ToString()));
		}
		else
		{
			//GameState.TowerSelected = GameState.Tower.None;
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;
		loadGUIPlay();
	}
}
