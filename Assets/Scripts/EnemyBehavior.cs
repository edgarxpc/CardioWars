using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public int Money;
    public int InitialHP;
    public int RecoveryAmmount;
    public OTAnimatingSprite Sprite;

    public UntrackEnemy Untrack;
    
    private int currentHP;
    private bool dying;

    void Start()
    {
        Sprite.Play("Walk");
        currentHP = InitialHP;
        dying = false;
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            if (!dying)
            {
                GameState.AvailableMoney += Money;
                dying = true;
                collider.enabled = false;
                Sprite.PlayOnce("Dying");
                Sprite.onAnimationFinish = delegate {
                    Destroy(gameObject);
                };
            }
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