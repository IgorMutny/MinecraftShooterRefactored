using UnityEngine;

public class AIWalkToTargetTask : AITask
{
    private AIRotationHelper _helper;

    public AIWalkToTargetTask(AI ai) : base(ai)
    {
        _helper = new AIRotationHelper(_ai);
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        Vector3 rotation = _helper.GetRotation();
        _ai.Character.Movement.SetInput(Vector2.up, rotation);
    }
}
