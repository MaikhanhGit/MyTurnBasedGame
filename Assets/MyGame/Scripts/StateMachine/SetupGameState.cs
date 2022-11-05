using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGameState : TurnBaseGameState
{
    [SerializeField] private int _totalPlayerGamePieces = 7;
    [SerializeField] private int _totalAIGamePieces = 7;
    [SerializeField] GameBoard _gameBoard;

    GameBoard _board;
    bool _activated = false;

    public override void Enter()
    {
        // build gameboard
        _board = _gameBoard.GetComponent<GameBoard>();
        _board.BuildGameBoard();
        _activated = false;
    }

    public override void Tick()
    {
        // admittedly hacky for demo. you would usually have delays or Input
        // TODO: main menu
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
