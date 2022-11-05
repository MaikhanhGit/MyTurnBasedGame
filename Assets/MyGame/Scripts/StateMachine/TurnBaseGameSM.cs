using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBaseGameSM : StateMachine
{
    [SerializeField] InputController _input;
    [SerializeField] GameBoard _gameBoardInput;
    public InputController PlayerInput => _input;
    public GameBoard GameBoardInput => _gameBoardInput;

    private void Start()
    {
        ChangeState<SetupGameState>();
    }
}
