using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AITurnGameState : TurnBaseGameState
{
    //public static event Action AITurnBegan;
    //public static event Action AITurnEnded;

    [SerializeField] float _pauseDuration = 2f;

    public override void Enter()
    {        
        // AITurnBegan?.Invoke();
        StartCoroutine(AIThinkingRoutine(_pauseDuration));
    }

    public override void Exit()
    {
        // AITurnEnded?.Invoke();

        StateMachine.ChangeState<CheckGameEndGameState>();
    }
    

    IEnumerator AIThinkingRoutine(float pauseDuration)
    {
        yield return new WaitForSeconds(pauseDuration);
        StateMachine.Board?.DeactivatePlayersTurn();        
        
    }
}
