using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CheckGameEndGameState : TurnBaseGameState
{
    private int _playerTotalCount = 7;
    private int _AITotalCount = 7;
    private bool _playersTurn = false;
    private bool _won = false;
    private int _wonScene = 2;
    private int _lostScene = 3;
    private int _tieScene = 4;    
    private float _LoadSceneDelayDuration = 2f;
    public float _exitDelay = 2f;

    public static event Action AITurnBegan;
    public static event Action AITurnEnded;

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
            StartCoroutine(LoadTie());
        }

        if(_playerTotalCount <= 0)
        {
            StartCoroutine(LoadLose());
        }

        if (_AITotalCount <= 0 || _won == true)
        {
            StartCoroutine(LoadWon());
        }

        else
        {
            StartCoroutine(StartExit());
        }
        
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
            AITurnEnded?.Invoke();
            StateMachine.ChangeState<PlayerTurnGameState>(); ;
        }        
    }

    private IEnumerator LoadWon()
    {
        yield return new WaitForSeconds(_LoadSceneDelayDuration);
        SceneManager.LoadScene(_wonScene);
    }

    private IEnumerator LoadLose()
    {
        yield return new WaitForSeconds(_LoadSceneDelayDuration);
        SceneManager.LoadScene(_lostScene);
    }

    private IEnumerator LoadTie()
    {
        yield return new WaitForSeconds(_LoadSceneDelayDuration);
        SceneManager.LoadScene(_tieScene);
    }

    private IEnumerator StartExit()
    {
        AITurnBegan?.Invoke();
        yield return new WaitForSeconds(_exitDelay);
        Exit();
    }
    
}
