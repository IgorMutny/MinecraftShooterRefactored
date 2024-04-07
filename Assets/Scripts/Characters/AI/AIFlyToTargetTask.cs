using UnityEngine;

public class AIFlyToTargetTask : AITask
{
    private AIRotationHelper _helper;
    private Vector3 _sideMovement;

    public AIFlyToTargetTask(AI ai) : base(ai)
    {
        _helper = new AIRotationHelper(_ai);
        GetRandomSideMovement();
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        Vector3 rotation = _helper.GetRotation();
        Vector3 movement = Vector3.zero;

        int direction;

        if (_ai.CanAttack == true)
        {
            direction = GetDirection(
            _ai.Character.transform.position.y,
            _ai.Target.transform.position.y);
        }
        else
        {
            float preferredHeight =
                _ai.Target.transform.position.y + _ai.Info.AI.PreferredFlightHeight;
            direction = GetDirection(
            _ai.Character.transform.position.y,
            preferredHeight);

            if (direction != 0)
            {
                rotation = (rotation + _sideMovement).normalized;
            }
        }

        switch (direction)
        {
            case 0: movement = Vector3.up; break;
            case 1: movement = Vector3.up + Vector3.back; break;
            case -1: movement = Vector3.up + Vector3.forward; break;
        }

        _ai.Character.Movement.SetInput(movement, rotation);
    }

    private int GetDirection(float myY, float targetY)
    {
        int result = 0;
        float deltaY = myY - targetY;

        if (Mathf.Abs(deltaY) <= 0.5f)
        {
            result = 0;
            return result;
        }

        if (myY > targetY)
        {
            result = 1;
        }

        if (myY < targetY)
        {
            result = -1;
        }

        return result;
    }

    private void GetRandomSideMovement()
    {
        float rnd = Random.Range(-1f, 1f);
        int side = (int)Mathf.Sign(rnd);
        _sideMovement = new Vector3(0, side * 90, 0);
    }
}
