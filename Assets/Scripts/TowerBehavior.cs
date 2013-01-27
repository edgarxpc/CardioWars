using UnityEngine;
using System.Collections;

public class TowerBehavior : MonoBehaviour {
	
	// Target and bullets
	public GameObject bullet;
	public GameObject missile;
	
	// Damage properties
	public float attackRange;
	public int damage;
	
	// Fire rate vars
	public float fireRateOnSeconds = 0.5f;	
	private float timeBeforeNextFire = 0.0f;
	public OTAnimatingSprite Sprite;
	
	
	void Start()
	{
		//Sprite.Play("Walk");
	}
	// Update is called once per frame
	void Update () {
		// Reload
		if (this.missile == null)
		{
			// Cooldown
			if (this.timeBeforeNextFire <= 0.0f)
			{
				this.collider.enabled = true;
			}
			else
			{
				this.timeBeforeNextFire -= Time.deltaTime;
			}
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		EnemyBehavior enemyBehavior = collider.GetComponent<EnemyBehavior>();
		if (enemyBehavior != null)
		{
			// Enter on Cooldown
			this.timeBeforeNextFire = this.fireRateOnSeconds;
			
			// Disable collision. Enable when bullet dies.
			this.collider.enabled = false;
			
			// Create bullet
			GameObject firedBullet = (GameObject) Instantiate (this.bullet, this.transform.position, Quaternion.identity);
			firedBullet.GetComponent<BulletBehavior>().target = collider.gameObject;
			firedBullet.GetComponent<BulletBehavior>().parent = this.gameObject;
			firedBullet.GetComponent<BulletBehavior>().damage = this.damage;
			this.missile = firedBullet;
		}
	}
}

public enum TowerTypes
{
    None = 0,
    Cholesterol = 1,
    Tumor = 2,
    Worm = 3,
}
