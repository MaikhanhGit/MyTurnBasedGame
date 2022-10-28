using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AITurnGameState : TurnBaseGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 1.5f;

    public override void Enter()
    {
        EnemyTurnBegan?.Invoke();

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    public override void Exit()
    {
        
    }

    public override void Tick()
    {
        
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        yield return new WaitForSeconds(pauseDuration);

        EnemyTurnEnded?.Invoke();

        StateMachine.ChangeState<PlayerTurnGameState>();
    }
}
