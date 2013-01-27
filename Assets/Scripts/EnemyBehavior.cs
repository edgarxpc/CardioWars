using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public int Money;
    public int InitialHP;
    public int RecoveryAmmount;

    public UntrackEnemy Untrack;
    
    private int currentHP;

    void Start()
    {
        currentHP = InitialHP;
    }

    void Update()
    {
        if (currentHP <= 0)
        {
			GameState.AvailableMoney += Money;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        BulletBehavior bulletBehavior = collider.GetComponent<BulletBehavior>();
        if (bulletBehavior != null)
        {
            currentHP -= bulletBehavior.damage;
        }
    }

    void OnDestroy()
    {
        if (Untrack != null)
        {
            Untrack(transform);
        }
    }
}

public delegate void UntrackEnemy(Transform enemy);