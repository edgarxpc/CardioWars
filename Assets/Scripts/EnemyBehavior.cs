using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public int Money;

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
			GameState.AvailableMoney += Money;
			this.hp -= bulletBehavior.damage;
		}
	}
	
	public int hp;
}
