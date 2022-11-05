using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTurnGameState : TurnBaseGameState
{
    [SerializeField] TextMeshProUGUI _playTurnTextUI = null;

    int _playerTurnCount = 0;
        

    public override void Enter()
    {        
        _playTurnTextUI.gameObject.SetActive(true);
        // activate player's input
        StateMachine.GameBoardInput?.ActivatePlayersTurn();

        _playerTurnCount++;
        _playTurnTextUI.text = "Player Turn: " + _playerTurnCount.ToString();

        // hook into events
        //StateMachine.PlayerInput.PressedConfirm += OnPressedConfirm;
        // TODO: add player's pieces Count, AI pieces Count
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        _playTurnTextUI.gameObject.SetActive(false);
        //StateMachine.PlayerInput.PressedConfirm -= OnPressedConfirm;
        // change to the AI turn state        
        StateMachine.ChangeState<AITurnGameState>();
    }    

    void OnPressedConfirm()
    {
        // Cancel player's input
        
        
        
    }
}
