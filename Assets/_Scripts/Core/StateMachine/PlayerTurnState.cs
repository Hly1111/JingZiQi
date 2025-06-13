using UnityEngine;

public class PlayerTurnState : IGameState
{
    private readonly BoardManager _boardManager;
    
    public PlayerTurnState(BoardManager boardManager)
    {
        _boardManager = boardManager;
    }
    
    public void Enter()
    {
        Debug.Log("Player Turn");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        _boardManager.HandlePlayerInput();
    }
}