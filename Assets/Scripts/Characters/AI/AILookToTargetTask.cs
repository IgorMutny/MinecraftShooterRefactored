public class AILookToTargetTask : AITask
{
    public AILookToTargetTask(AI ai) : base(ai)
    {
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        if (_ai.Target != null)
        {
            _ai.Character.Movement.RotateHeadToTarget(_ai.Target.transform);
        }
    }
}
