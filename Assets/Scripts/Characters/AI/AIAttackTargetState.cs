public class AIAttackTargetState : AIState
{
    private bool _canChangeState;

    protected override void AddTasks()
    {
        if (_ai.Info.Weapon is MeleeInfo)
        {
            _tasks = new AITask[]
            {
                new AIStopTask(_ai),
                new AIAttackTargetWithMeleeTask(_ai),
            };

            _canChangeState = true;
        }

        if (_ai.Info.Weapon is ThrowingWeaponInfo)
        {
            _tasks = new AITask[]
            {
                new AIStrafeAndRotateToTargetTask(_ai),
                new AIAttackTargetWithThrowingWeaponTask(_ai),
                new AIRotateThrowingWeaponToTargetHeightTask(_ai),
            };

            _canChangeState = true;
        }

        if (_ai.Info.Weapon is SuicideBombingInfo)
        {
            _tasks = new AITask[]
            {
                new AIStopTask(_ai),
                new AIExplodeTargetTask(_ai),
            };

            _canChangeState = false;
        }
    }

    protected override void TryChangeState()
    {
        if (_canChangeState == false)
        {
            return;
        }

        if (_ai.GetDistanceToTarget() > _ai.Info.AI.DistanceToAttack)
        {
            _ai.SetState<AIMoveToTargetState>();
        }

        if (_ai.Info.AI.CanFlee == true
            && _ai.GetDistanceToTarget() < _ai.Info.AI.DistanceToFlee)
        {
            _ai.SetState<AIFleeState>();
        }
    }
}
