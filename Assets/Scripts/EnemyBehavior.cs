using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (this.hp <= 0)
		{
			Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		BulletBehavior bulletBehavior = collider.GetComponent<BulletBehavior>();
		if (bulletBehavior != null)
		{
			this.hp -= bulletBehavior.damage;
		}
	}
	
	public int hp;
}
