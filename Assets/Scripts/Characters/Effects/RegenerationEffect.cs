public class RegenerationEffect : Effect
{
    protected override void InitializeExtended()
    {
        Cure();
    }

    private void Cure()
    {
        if (_isActive == true)
        {
            _character.Health.GetCure((int)_value);
            _timer.AddSignal(_period, Cure);
        }
    }
}
