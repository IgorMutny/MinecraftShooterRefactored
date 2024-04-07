public class AIRemoveDeadTargetTask : AITask
{
    public AIRemoveDeadTargetTask(AI ai) : base(ai)
    {
    }

    public override void OnTick()
    {
        if (_ai.Target != null)
        {
            if (_ai.Target.IsAlive == false)
            {
                _ai.SetTarget(null);
            }
        }
    }
}
