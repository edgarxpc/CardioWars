using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartBehavior : MonoBehaviour
{
    public OTAnimatingSprite Sprite;
    public int InitialHP = 2000;
    public int HealthDecrement = 85;
    public float AttackInterval = 8f;
    
    private int currentHP;
    private float heartBeatInterval;
    private float elapsedTimeForBeat;
    private float elapsedTimeForAttack;
    private HeartStates state;

    const int MaxHealth = 2500;   
    const int HealthBar = 1400;
    const int SickBar = 800;
    const int MoreSickBar = 250;

    const float HealthBeatInterval = 2f;    
    const float SickBeatInterval = 1.3f;
    const float MoreSickBeatInterval = 0.8f;    
    const float CriticBeatInterval = 0.6f;

    private enum HeartStates
    {
        Health,
        Sick,
        MoreSick,
        Dying,
        Dead,
    }

    private void Start()
    {
        currentHP = InitialHP;
        elapsedTimeForBeat = HealthBeatInterval;
        elapsedTimeForAttack = 0;
        heartBeatInterval = HealthBeatInterval;
        state = HeartStates.Health;
    }

    private void Update()
    {
        if (state == HeartStates.Dead)
        {
            return;
        }

        if (state == HeartStates.Dying)
        {
            Sprite.looping = false;
            Sprite.Play(state.ToString());
            Sprite.onAnimationFinish = delegate
            {
                GameState.CurrentState = LevelState.PlayerWon;
            };
            state = HeartStates.Dead;
        }
        else
        {
            elapsedTimeForBeat += Time.deltaTime;
            if (elapsedTimeForBeat > heartBeatInterval)
            {
                elapsedTimeForBeat = 0;
                Sprite.Play(state.ToString());
				GameState.Audio.playHearthBeat1();
            }

            if (GameState.CurrentState == LevelState.WaveStarted)
            {
                elapsedTimeForAttack += Time.deltaTime;
                if (elapsedTimeForAttack > AttackInterval)
                {
                    Debug.Log(string.Format("HeartBehavior: Decrementando HP -{0} (AUTO)", HealthDecrement));
                    elapsedTimeForAttack = 0;
                    currentHP -= HealthDecrement;

                    if (currentHP > MaxHealth)
                    {
                        GameState.CurrentState = LevelState.HeartWon;
                    }
                    else if (currentHP > HealthBar)
                    {
                        state = HeartStates.Health;
                        heartBeatInterval = HealthBeatInterval;
                    }
                    else if (currentHP > SickBar)
                    {
                        state = HeartStates.Sick;
                        heartBeatInterval = SickBeatInterval;
                    }
                    else if (currentHP > MoreSickBar)
                    {
                        state = HeartStates.MoreSick;
                        heartBeatInterval = MoreSickBeatInterval;
                    }
                    else if (currentHP > 0)
                    {
                        heartBeatInterval = CriticBeatInterval;
                        Sprite.speed = 1.5f;
                    }
                    else
                    {
                        state = HeartStates.Dying;
                        heartBeatInterval = 0;
                        elapsedTimeForBeat = 0;
                        Sprite.speed = 1;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
        Debug.Log(string.Format("HeartBehavior: Incremantando HP +{0}", enemy.RecoveryAmmount));

        currentHP += enemy.RecoveryAmmount;
        Destroy(other.gameObject);
    }
}
