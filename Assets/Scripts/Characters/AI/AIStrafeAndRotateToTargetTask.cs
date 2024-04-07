using UnityEngine;

public class AIStrafeAndRotateToTargetTask : AITask
{
    private AIRotationHelper _helper;
    private float _strafeCounter;
    private int _strafeDirection;

    public AIStrafeAndRotateToTargetTask(AI ai) : base(ai)
    {
        _helper = new AIRotationHelper(_ai);
        _strafeCounter = _ai.Info.AI.StrafeDuration;
        SetRandomDirectionOfStrafe();
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        DecreaseStrafeCounter();

        Vector3 rotation = _helper.GetRotation();
        _ai.Character.Movement.SetInput(Vector3.right * _strafeDirection, rotation);
    }

    private void DecreaseStrafeCounter()
    {
        _strafeCounter -= Time.fixedDeltaTime;

        if (_strafeCounter <= 0)
        {
            _strafeCounter = _ai.Info.AI.StrafeDuration;
            SetRandomDirectionOfStrafe();
        }
    }

    private void SetRandomDirectionOfStrafe()
    {
        float rnd = Random.Range(-1f, 1f);
        _strafeDirection = (int)Mathf.Sign(rnd);
    }
}
