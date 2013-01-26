using UnityEngine;
using System.Collections;

public class LevelObject : MonoBehaviour {
	
	Vector2[] wayPoints = { new Vector2(25.00f,11.00f), new Vector2(25.00f,18.00f),new Vector2(5.00f, 18.00f), new Vector2(5.00f,11.00f)};
	public GameObject gobjGlobuloRojo;

	// Use this for initialization
	void Start () {
		//GameObject obj = (GameObject)Instantiate(gobjGlobuloRojo, new Vector3(12, 0, 0), Quaternion.identity);
		GameObject obj = (GameObject)Instantiate(gobjGlobuloRojo, new Vector3(12, 0, 0), Quaternion.identity);
		

		Debug.Log("Success");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
