using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurnBaseGameSM))]
public class TurnBaseGameState : State
{
    protected TurnBaseGameSM StateMachine { get; private set; }

    private void Awake()
    {
        StateMachine = GetComponent<TurnBaseGameSM>();
    }
}
