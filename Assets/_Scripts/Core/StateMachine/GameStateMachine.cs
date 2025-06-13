public class GameStateMachine
{
    private IGameState _currentState;
    
    public void ChangeState(IGameState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        
        _currentState = newState;
        
        if (_currentState != null)
        {
            _currentState.Enter();
        }
    }
    
    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }
}