using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartBehavior : MonoBehaviour
{
    public OTAnimatingSprite Sprite;
    public int InitialHP = 2000;
    public int HealthDecrement = 85;
    public float AttackInterval = 5f;
    
    private int currentHP;
    private float heartBeatInterval;
    private float elapsedTimeForBeat;
    private float elapsedTimeForAttack;
    private HealthStates state;

    const int MaxHealth = 2500;   
    const int HealthBar = 1400;
    const int SickBar = 800;
    const int MoreSickBar = 250;

    const float HealthBeatInterval = 2f;    
    const float SickBeatInterval = 1f;
    const float MoreSickBeatInterval = 0.65f;    
    const float CriticBeatInterval = 0.5f;
    const float CriticRate = 1.25f;
    const float DyingRate = 1f;
    const float DyingTime = 3f;

    private enum HealthStates
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
        state = HealthStates.Health;
    }

    private void Update()
    {
        if (state == HealthStates.Dead)
        {
            GameState.CurrentState = LevelState.PlayerWon;
        }
        else if (state == HealthStates.Dying)
        {
            elapsedTimeForBeat += Time.deltaTime;
            if (elapsedTimeForBeat > heartBeatInterval)
            {
                state = HealthStates.Dead;
            }            
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
                }
            }

            if (currentHP > MaxHealth)
            {
                GameState.CurrentState = LevelState.HeartWon;
            }
            else if (currentHP > HealthBar)
            {
                state = HealthStates.Health;
                heartBeatInterval = HealthBeatInterval;
            }
            else if (currentHP > SickBar)
            {
                state = HealthStates.Sick;
                heartBeatInterval = SickBeatInterval;
            }
            else if (currentHP > MoreSickBar)
            {
                state = HealthStates.MoreSick;
                heartBeatInterval = MoreSickBeatInterval;
            }
            else if (currentHP > 0)
            {
                heartBeatInterval = CriticBeatInterval;
                Sprite.speed = CriticRate;
            }
            else
            {
                Sprite.speed = DyingRate;
                state = HealthStates.Dying;
                Sprite.Play(state.ToString());
                heartBeatInterval = DyingTime;
                elapsedTimeForBeat = 0;
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
