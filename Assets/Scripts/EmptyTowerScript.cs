using UnityEngine;
using System.Collections;

public class EmptyTowerScript : MonoBehaviour
{
	public GameObject objCholesterolTower;
	public GameObject objWormTower;
	public GameObject objTumorTower;

	const int c_WormPrice = 10;
	const int c_CholesterolPrice = 50;
	const int c_TumorPrice = 30;

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
			case TowerTypes.Cholesterol:

				if (GameState.AvailableMoney >= c_CholesterolPrice)
				{
					Instantiate(objCholesterolTower, this.transform.position, Quaternion.identity);
                    GameState.TowerSelected = TowerTypes.None;
                    Debug.Log(string.Format("New {0} tower created", GameState.TowerSelected.ToString()));
					GameState.AvailableMoney -= c_CholesterolPrice;
					Destroy(gameObject);
				}
				break;
			case TowerTypes.Tumor:
				if (GameState.AvailableMoney >= c_TumorPrice)
				{
					Instantiate(objTumorTower, this.transform.position, Quaternion.identity);
                    GameState.TowerSelected = TowerTypes.None;
                    Debug.Log(string.Format("New {0} tower created", GameState.TowerSelected.ToString()));
					GameState.AvailableMoney -= c_TumorPrice;
					Destroy(gameObject);
				}
				break;
			case TowerTypes.Worm:
				if (GameState.AvailableMoney >= c_WormPrice)
				{
					Instantiate(objWormTower, this.transform.position, Quaternion.identity);
                    GameState.TowerSelected = TowerTypes.None;
                    Debug.Log(string.Format("New {0} tower created", GameState.TowerSelected.ToString()));
					GameState.AvailableMoney -= c_WormPrice;
					Destroy(gameObject);
				}
				break;
		}

	}
}
