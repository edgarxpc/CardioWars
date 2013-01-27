using UnityEngine;
using System.Collections;

public class TowerBehavior : MonoBehaviour {
	
	private GameObject target;
	
	public GameObject bullet;
	
	public GameObject missile;
	
	public float attackRange;
	
	public int damage;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (this.missile == null)
		{
			this.collider.enabled = true;
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		// Disable collision. Enable when bullet dies.
		this.collider.enabled = false;
		
		// Create bullet
		GameObject firedBullet = (GameObject) Instantiate (this.bullet, this.transform.position, Quaternion.identity);
		firedBullet.GetComponent<BulletBehavior>().target = collider.gameObject;
		firedBullet.GetComponent<BulletBehavior>().parent = this.gameObject;
		firedBullet.GetComponent<BulletBehavior>().damage = this.damage;
		this.missile = firedBullet;
		
		// Store enemy
		this.target = collider.gameObject;
	}
}
