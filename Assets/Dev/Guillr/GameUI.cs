using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{

	public GUISkin skin;

	public Texture btnTower1;
	public Texture btnTower2;
	public Texture btnTower3;

	public Texture scoreMoney;

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
		if (GUI.Button(new Rect(5, 5, 74, 74), btnTower1))
		{
			Debug.Log("Hit tower  1");
			//something
		}
		else if (GUI.Button(new Rect(84, 5, 74, 74), btnTower2))
		{
			Debug.Log("Hit tower  2");
			//something
		}
		else if (GUI.Button(new Rect(163, 5, 74, 74), btnTower3))
		{
			Debug.Log("Hit tower  3");
			//something
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;
		loadGUIPlay();

	}
}
