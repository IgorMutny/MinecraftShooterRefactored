public interface ILevelState
{
    public void Enter(LevelStateMachine levelStateMachine);

    public void OnTick();
}
