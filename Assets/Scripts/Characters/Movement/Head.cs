using UnityEngine;

public class Head
{
    private Transform _head;
    private Vector2 _maxAngles;

    private Vector3 HeadAngles => _head.localEulerAngles;

    public Head(Transform head, Vector2 maxAngles)
    {
        _head = head;
        _maxAngles = maxAngles;
    }

    public void RotateToTarget(Transform target)
    {
        _head.LookAt(target.position);
        ClampHeadAngles();
    }

    public void RotateX(float deltaX)
    {
        _head.localEulerAngles = HeadAngles - new Vector3(deltaX * Time.fixedDeltaTime, 0, 0);
        ClampHeadAngles();
    }

    private void ClampHeadAngles()
    {
        Vector3 headAngles = HeadAngles;
        headAngles.x = 
            Mathf.Clamp(NormalizeAngle(headAngles.x), -_maxAngles.x, _maxAngles.x);
        headAngles.y = 
            Mathf.Clamp(NormalizeAngle(headAngles.y), -_maxAngles.y, _maxAngles.y);
        headAngles.z = 0;

        _head.localEulerAngles = headAngles;
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
