public abstract class AITask
{
    protected AI _ai;

    public AITask(AI ai)
    {
        _ai = ai;
    }

    public abstract void OnTick();
}
