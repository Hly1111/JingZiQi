using System.Collections;
using UnityEngine;

public class AITurnState : IGameState
{
    private readonly BoardManager _boardManager;
    
    public AITurnState(BoardManager boardManager)
    {
        _boardManager = boardManager;
    }
    
    public void Enter()
    {
        Debug.Log("AI Turn");
        _boardManager.StartCoroutine(ThinkAndPlace());
    }

    private IEnumerator ThinkAndPlace()
    {
        yield return new WaitForSeconds(0.5f);
        _boardManager.HandleAIInput();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}