using System;
using TMPro;
using UnityEngine;

public class OutcomePanel : MonoBehaviour
{
    [SerializeField] BoardManager boardManager;
    [SerializeField] TextMeshProUGUI outcomeText;

    private void Start()
    {
        boardManager.OutcomeEvent += ShowOutcome;
        boardManager.RestartEvent += Hide;
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowOutcome(int winner)
    {
        if (winner == 0)
        {
            outcomeText.text = "It's a draw!";
        }
        else if (winner == 1)
        {
            outcomeText.text = "Player wins!";
        }
        else if (winner == 2)
        {
            outcomeText.text = "AI wins!";
        }
        
        gameObject.SetActive(true);
    }
}
