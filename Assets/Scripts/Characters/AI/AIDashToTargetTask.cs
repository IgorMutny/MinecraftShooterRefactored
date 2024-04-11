using UnityEngine;

public class AIDashToTargetTask : AITask
{
    private AIRotationHelper _helper;
    private DashState _currentState;
    private TimerWrapper _timer;

    public AIDashToTargetTask(AI ai) : base(ai)
    {
        _helper = new AIRotationHelper(_ai);
        _currentState = DashState.Dashing;

        _timer = ServiceLocator.Get<TimerWrapper>();
        SwitchDashState();
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        Vector3 rotation = _helper.GetRotation();

        if (_currentState == DashState.Dashing)
        {
            _ai.Character.Movement.SetInput(Vector2.up, rotation);
        }

        if (_currentState == DashState.Waiting)
        {
            _ai.Character.Movement.SetInput(Vector2.zero, rotation);
        }
    }

    private void SwitchDashState()
    {
        switch (_currentState)
        {
            case DashState.Dashing:
                _timer.AddSignal(_ai.Info.AI.DelayBetweenDashes, SwitchDashState);
                _currentState = DashState.Waiting;
                break;
            case DashState.Waiting:
                _timer.AddSignal(_ai.Info.AI.DashDuration, SwitchDashState);
                _currentState = DashState.Dashing;
                break;
        }
    }

    private enum DashState
    {
        Waiting,
        Dashing,
    }
}
