public class AIAttackTargetWithMeleeTask : AITask
{
    public AIAttackTargetWithMeleeTask(AI ai) : base(ai) { }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        if (_ai.CanAttack == true)
        {
            _ai.Character.Inventory.SetInput(true, false, -1, 0);
            _ai.OnAttacking();
        }
    }
}
