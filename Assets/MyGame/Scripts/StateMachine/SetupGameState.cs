using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameState : TurnBaseGameState
{
    [SerializeField] private int _totalPlayerGamePieces = 7;
    [SerializeField] private int _totalAIGamePieces = 7;

    bool _activated = false;

    public override void Enter()
    {

        _activated = false;
    }

    public override void Tick()
    {
        // admittedly hacky for demo. you would usually have delays or Input
        if(_activated == false)
        {
            _activated = true;
            StateMachine.ChangeState<PlayerTurnGameState>();
        }
    }

    public override void Exit()
    {
        _activated = false;
    }
    
}
