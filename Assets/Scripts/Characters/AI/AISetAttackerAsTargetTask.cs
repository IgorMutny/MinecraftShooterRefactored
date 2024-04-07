public class AISetAttackerAsTargetTask : AITask
{
    public AISetAttackerAsTargetTask(AI ai) : base(ai)
    {
        _ai.Character.Health.Damaged += OnDamaged;
        _ai.Character.Health.Died += OnDied;
    }

    public override void OnTick() { }

    private void OnDamaged(int value, Character attacker)
    {
        _ai.SetTarget(attacker);
    }

    private void OnDied(Character attacker)
    {
        _ai.Character.Health.Damaged -= OnDamaged;
        _ai.Character.Health.Died -= OnDied;
    }
}
