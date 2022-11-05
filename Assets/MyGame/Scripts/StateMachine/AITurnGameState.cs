using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AITurnGameState : TurnBaseGameState
{
    public static event Action AITurnBegan;
    public static event Action AITurnEnded;

    [SerializeField] float _pauseDuration = 2f;

    public override void Enter()
    {        
        AITurnBegan?.Invoke();
        StateMachine.GameBoardInput?.DeactivatePlayersTurn();

        
    }

    public override void Exit()
    {
        StartCoroutine(AIThinkingRoutine(_pauseDuration));
        // StateMachine.ChangeState<PlayerTurnGameState>();
    }

    public override void Tick()
    {
        
    }

    IEnumerator AIThinkingRoutine(float pauseDuration)
    {
        yield return new WaitForSeconds(pauseDuration);

        AITurnEnded?.Invoke();

        StateMachine.ChangeState<PlayerTurnGameState>();
    }
}
