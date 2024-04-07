using UnityEngine;

public class AIDashToTargetTask : AITask
{
    private AIRotationHelper _helper;
    private DashState _currentState;
    private float _counter;

    public AIDashToTargetTask(AI ai) : base(ai)
    {
        _helper = new AIRotationHelper(_ai);
        _currentState = DashState.Dashing;
        _counter = 0;
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        DecreaseDashStateCounter();

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

    private void DecreaseDashStateCounter()
    {
        _counter -= Time.fixedDeltaTime;

        if (_counter <= 0)
        {
            switch (_currentState)
            {
                case DashState.Dashing:
                    _counter = _ai.Info.AI.DelayBetweenDashes;
                    _currentState = DashState.Waiting;
                    break;
                case DashState.Waiting:
                    _counter = _ai.Info.AI.DashDuration;
                    _currentState = DashState.Dashing;
                    break;
            }
        }
    }

    private enum DashState
    {
        Waiting,
        Dashing,
    }
}
