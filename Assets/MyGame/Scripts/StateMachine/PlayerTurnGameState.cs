using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTurnGameState : TurnBaseGameState
{
    [SerializeField] TextMeshProUGUI _playerTurnTextUI = null;

    int _playerTurnCount = 0;
        

    public override void Enter()
    {        
        _playerTurnTextUI.gameObject.SetActive(true);
        // activate player's input
        StateMachine.Board?.ActivatePlayersTurn();

        _playerTurnCount++;
        _playerTurnTextUI.text = "Player Turn: " + _playerTurnCount.ToString();

        // hook into events
        //StateMachine.PlayerInput.PressedConfirm += OnPressedConfirm;
        // TODO: add player's pieces Count, AI pieces Count
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        _playerTurnTextUI.gameObject.SetActive(false);
        //StateMachine.PlayerInput.PressedConfirm -= OnPressedConfirm;
        // change to the AI turn state
        StateMachine.ChangeState<CheckGameEndGameState>();        
    }    

    void OnPressedConfirm()
    {
        // Cancel player's input
        
        
        
    }
}
