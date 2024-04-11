public class AIFleeState : AIState
{
    protected override void AddTasks()
    {
        if (_ai.Info.Weapon is ThrowingWeaponInfo)
        {
            _tasks = new AITask[]
            {
                new AIFleeAndRotateToTargetTask(_ai),
                new AIAttackTargetWithThrowingWeaponTask(_ai),
                new AIRotateThrowingWeaponToTargetHeightTask(_ai),
            };
        }
    }

    protected override void TryChangeState()
    {
        if (_ai.GetDistanceToTarget() > _ai.Info.AI.DistanceToFlee)
        {
            _ai.SetState<AIAttackTargetState>();
        }
    }
}
