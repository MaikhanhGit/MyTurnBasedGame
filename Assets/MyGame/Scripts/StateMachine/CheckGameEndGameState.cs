using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckGameEndGameState : TurnBaseGameState
{
    private int _playerTotalCount = 7;
    private int _AITotalCount = 7;
    private bool _playersTurn = false;
    private bool _won = false;
    private int _wonScene = 2;
    private int _lostScene = 3;
    private int _tieScene = 4;

        
    public override void Enter()
    {
        _AITotalCount = StateMachine.Board.AIPieceCount;
        _playerTotalCount = StateMachine.Board.PlayerPieceCount;
        _won = StateMachine.Board.Won;        
    }

    public override void Tick()
    {
        if(_playerTotalCount == 1 && _AITotalCount == 1)
        {            
            // open scene
            LoadTie();
        }

        if(_playerTotalCount <= 0)
        {
            LoadLose();
        }

        if(_AITotalCount <= 0 || _won == true)
        {
            LoadWon();
            
        }
        
        Exit();
        
    }

    public override void Exit()
    {        
        _playersTurn = StateMachine.Board.CurrentPlayersTurn;

        if (_playersTurn == true)
        {
            StateMachine.ChangeState<AITurnGameState>();
        }
        else if (_playersTurn == false)
        {
            StateMachine.ChangeState<PlayerTurnGameState>();
        }        
    }

    private void LoadWon()
    {
        SceneManager.LoadScene(_wonScene);
    }

    private void LoadLose()
    {
        SceneManager.LoadScene(_lostScene);
    }

    private void LoadTie()
    {
        SceneManager.LoadScene(_tieScene);
    }
}
