using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour {
	public GameObject parent;
	
	public GameObject target;
	
	public Vector3 movementDirection;
	
	public float movementDelta;
	
	public int damage;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.movementDirection = this.target.transform.position - this.transform.position;
		this.transform.position += this.movementDelta * this.movementDirection;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.GetComponent<Rigidbody>() == target.GetComponent<Rigidbody>())
		{
			Destroy(this.gameObject);
		}
	}
}
