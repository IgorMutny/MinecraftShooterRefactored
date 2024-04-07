public class AIExplodeTargetTask : AITask
{
    public AIExplodeTargetTask(AI ai) : base(ai)
    {
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        _ai.Character.Inventory.SetInput(true, false, -1, 0);
        _ai.OnAttacking();
    }
}
