using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseGameSM : StateMachine
{
    [SerializeField] InputController _input;
    [SerializeField] GameBoard _gameBoard;
    public InputController PlayerInput => _input;
    public GameBoard Board => _gameBoard;

    private void Start()
    {
        ChangeState<SetupGameState>();
    }
}
