using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeartBehavior : MonoBehaviour
{
    public OTAnimatingSprite Animation;
        
    private int health;
    private float heartBeatInterval;
    private float elapsedTimeForBeat;
    private float elapsedTimeForAttack;
    private HealthStates state;

    const int MaxHealth = 2500;
    const int InitialHealth = 2000;
    const int HealthDecrement = 85;
    const int HealthBar = 1400;
    const int SickBar = 800;
    const int MoreSickBar = 250;

    const float AttackInterval = 5f;
    const float HealthBeatInterval = 2f;
    const float SickBeatInterval = 1f;
    const float MoreSickBeatInterval = 0.65f;    
    const float CriticBeatInterval = 0.5f;
    const float CriticRate = 1.25f;
    const float DyingRate = 0.9f;
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
        health = InitialHealth;
        elapsedTimeForBeat = HealthBeatInterval;
        elapsedTimeForAttack = 0;
        heartBeatInterval = HealthBeatInterval;
        state = HealthStates.Health;
    }

    private void Update()
    {
        if (state == HealthStates.Dead)
        {
            GameState.LevelCompleted();
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
                Animation.Play(state.ToString());
				GameState.Audio.playHearthBeat1();
            }

            elapsedTimeForAttack += Time.deltaTime;
            if (elapsedTimeForAttack > AttackInterval)
            {
                elapsedTimeForAttack = 0;
                health -= HealthDecrement;
            }

            if (health > HealthBar)
            {
                state = HealthStates.Health;
                heartBeatInterval = HealthBeatInterval;
            }
            else if (health > SickBar)
            {
                state = HealthStates.Sick;
                heartBeatInterval = SickBeatInterval;
            }
            else if (health > MoreSickBar)
            {
                state = HealthStates.MoreSick;
                heartBeatInterval = MoreSickBeatInterval;
            }
            else if (health > 0)
            {
                heartBeatInterval = CriticBeatInterval;
                Animation.speed = CriticRate;
            }
            else
            {
                Animation.speed = DyingRate;
                state = HealthStates.Dying;
                Animation.Play(state.ToString());
                heartBeatInterval = DyingTime;
                elapsedTimeForBeat = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
        //health += enemy.RecoveryAmmount;
        Destroy(other.gameObject);
    }
}
