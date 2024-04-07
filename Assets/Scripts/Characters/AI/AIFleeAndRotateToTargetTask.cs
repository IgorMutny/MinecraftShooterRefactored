using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFleeAndRotateToTargetTask : AITask
{
    private AIRotationHelper _helper;
    private float _strafeCounter;
    private int _strafeDirection;

    public AIFleeAndRotateToTargetTask(AI ai) : base(ai)
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

        Vector3 direction = (Vector3.right * _strafeDirection + Vector3.down * 2).normalized;

        Vector3 rotation = _helper.GetRotation();
        _ai.Character.Movement.SetInput(direction, rotation);
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
