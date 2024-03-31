public class GameStateMachine : IService
{
    private IGameState _currentState;

    public GameStateMachine()
    {
        _currentState = new MetaGameState();
        _currentState.Enter();
    }

    public void SetState(IGameState state)
    {
        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }
}
