public class AIMoveToTargetState : AIState
{
    protected override void AddTasks()
    {
        if (_ai.Info.MovementType == MovementType.Walking && _ai.Info.AI.DashingMovement == false)
        {
            _tasks = new AITask[]
            {
                new AIWalkToTargetTask(_ai)
            };
        }

        if (_ai.Info.MovementType == MovementType.Walking && _ai.Info.AI.DashingMovement == true)
        {
            _tasks = new AITask[]
            {
                new AIDashToTargetTask(_ai)
            };
        }

        if (_ai.Info.MovementType == MovementType.Flying)
        {
            _tasks = new AITask[]
            {
                new AIFlyToTargetTask(_ai)
            };
        }
    }

    protected override void TryChangeState()
    {
        if (_ai.GetDistanceToTarget() < _ai.Info.AI.DistanceToAttack)
        {
            _ai.SetState<AIAttackTargetState>();
        }
    }
}
