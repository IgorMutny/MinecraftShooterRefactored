public class AIPermanentTasks : AIState
{
    protected override void AddTasks()
    {
        _tasks = new AITask[] 
        { 
            new AILookToTargetTask(_ai),
            new AIRemoveDeadTargetTask(_ai),
            new AISetAttackerAsTargetTask(_ai),
            new AIFindTargetIfNullTask(_ai)
        };
    }

    protected override void TryChangeState() { }
}
