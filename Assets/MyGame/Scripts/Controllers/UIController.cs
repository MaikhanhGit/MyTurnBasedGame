using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _AIThinkingTextUI = null;
    

    private void OnEnable()
    {
        CheckGameEndGameState.AITurnBegan += OnAITurnBegan;
        CheckGameEndGameState.AITurnEnded += OnAITurnEnded;
    }

    private void OnDisable()
    {
        CheckGameEndGameState.AITurnBegan -= OnAITurnBegan;
        CheckGameEndGameState.AITurnEnded -= OnAITurnEnded;
    }

    private void Start()
    {
        // make sure text is disabled on start
        _AIThinkingTextUI.gameObject.SetActive(false);
    }

    void OnAITurnBegan()
    {
        _AIThinkingTextUI.gameObject.SetActive(true);
    }

    void OnAITurnEnded()
    {
        _AIThinkingTextUI.gameObject.SetActive(false);
    }
}
