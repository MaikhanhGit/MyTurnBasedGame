using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseGameSM : StateMachine
{
    [SerializeField] InputController _input;    
    public InputController PlayerInput => _input;

    private void Start()
    {
        ChangeState<SetupGameState>();
    }
}
