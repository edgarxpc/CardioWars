using UnityEngine;
using System.Collections;

public class EmptyTowerScript : MonoBehaviour
{
	public GameObject objCholesterolTower;
	public GameObject objWormTower;
	public GameObject objTumorTower;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnMouseDown()
	{
		switch (GameState.TowerSelected)
		{
			case GameState.Tower.Cholesterol:
				Instantiate(objCholesterolTower, this.transform.position, Quaternion.identity);
				GameState.TowerSelected = GameState.Tower.None;
				Debug.Log(string.Format("New {0} tower created", GameState.TowerSelected.ToString()));
				Destroy(gameObject);
				break;
			case GameState.Tower.Tumor:
				Instantiate(objTumorTower, this.transform.position, Quaternion.identity);
				GameState.TowerSelected = GameState.Tower.None;
				Debug.Log(string.Format("New {0} tower created", GameState.TowerSelected.ToString()));
				Destroy(gameObject);
				break;
			case GameState.Tower.Worm:
				Instantiate(objWormTower, this.transform.position, Quaternion.identity);
				GameState.TowerSelected = GameState.Tower.None;
				Debug.Log(string.Format("New {0} tower created", GameState.TowerSelected.ToString()));
				Destroy(gameObject);
				break;
		}

	}
}
