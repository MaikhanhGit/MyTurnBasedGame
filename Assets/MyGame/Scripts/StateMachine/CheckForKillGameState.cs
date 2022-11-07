using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForKillGameState : TurnBaseGameState
{
    [SerializeField] GameBoard _gameBoard;

    GameBoard _board;

    private void Awake()
    {
        _board = _gameBoard.GetComponent<GameBoard>();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();
    }
}
