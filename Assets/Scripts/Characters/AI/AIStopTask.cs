using UnityEngine;

public class AIStopTask : AITask
{
    public AIStopTask(AI ai) : base(ai) { }

    public override void OnTick()
    {
        _ai.Character.Movement.SetInput(Vector3.zero, Vector3.zero);
    }
}
