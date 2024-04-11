public class PoisoningEffect : Effect
{
    protected override void InitializeExtended()
    {
        Damage();
    }

    private void Damage()
    {
        if (_isActive == true)
        {
            _character.Health.GetDamage((int)_value, DamageType.Poison, null);
            _timer.AddSignal(_period, Damage);
        }
    }
}

