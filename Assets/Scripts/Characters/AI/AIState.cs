public abstract class AIState
{
    protected AI _ai;

    protected AITask[] _tasks;

    public void SetAI(AI ai)
    {
        _ai = ai;

        AddTasks();
    }

    public void OnTick()
    { 
        foreach (AITask task in _tasks)
        {
            task.OnTick();
        }

        TryChangeState();
    }

    protected abstract void AddTasks();
    protected abstract void TryChangeState();
}
