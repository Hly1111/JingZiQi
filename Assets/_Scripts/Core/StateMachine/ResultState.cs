using UnityEngine;

public class ResultState : IGameState
{
    private readonly int _winner; // 0 = draw, 1 = disc, 2 = rectangle
    private readonly BoardManager _boardManager;
    
    public ResultState(int winner, BoardManager boardManager)
    {
        _winner = winner;
        _boardManager = boardManager;
    }
    
    public void Enter()
    {
        _boardManager.OutcomeEvent?.Invoke(_winner);
    }

    public void Exit()
    {
        _boardManager.RestartEvent?.Invoke();
    }

    public void Update()
    {
        _boardManager.HandleRestart();
    }
}