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
		if ((this.gameObject != null) && (this.target != null))
		{
			this.movementDirection = this.target.transform.position - this.transform.position;
			this.transform.position += this.movementDelta * this.movementDirection;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider collider) {
		if ((this.gameObject != null) && (this.target != null))
		{
			if (collider.GetComponent<Rigidbody>() == target.GetComponent<Rigidbody>())
			{
				GameState.Audio.playShootTumor();
				Destroy(this.gameObject);
			}
		}
	}
}
