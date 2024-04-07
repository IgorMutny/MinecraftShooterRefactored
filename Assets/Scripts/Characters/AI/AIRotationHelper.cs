using UnityEngine;

public class AIRotationHelper
{
    private AI _ai;

    public AIRotationHelper(AI ai)
    {
        _ai = ai;
    }

    public Vector3 GetRotation()
    {
        float angle = DefineRotation();
        Vector3 rotation = Vector2.zero;

        if (Mathf.Abs(angle) >= _ai.Info.RotationSpeed * Time.fixedDeltaTime)
        {
            rotation = new Vector3(angle, 0, 0);
        }

        return rotation;
    }

    private float DefineRotation()
    {
        Vector3 myPosition = _ai.Character.transform.position;
        Vector3 targetPosition = _ai.Target.transform.position;
        Vector3 direction = targetPosition - myPosition;
        direction.y = 0;

        float delta = GetVectorAngleY(direction) - GetVectorAngleY(_ai.Character.transform.forward);
        delta = NormalizeAngle(delta);

        return delta;
    }

    private float GetVectorAngleY(Vector3 v)
    {
        float angle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        return angle;
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180)
        {
            return angle - 360;
        }
        else if (angle < -180)
        {
            return angle + 360;
        }
        else
        {
            return angle;
        }
    }
}
