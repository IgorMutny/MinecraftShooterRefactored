using UnityEngine;

public class AIRotateThrowingWeaponToTargetHeightTask : AITask
{
    public AIRotateThrowingWeaponToTargetHeightTask(AI ai) : base(ai) { }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        Transform attackPoint = _ai.Character.AttackPoint;
        Transform target = _ai.Target.transform;

        Vector3 attackPointPosition = attackPoint.position;
        attackPointPosition.y = 0;

        Vector3 targetPosition = target.position;
        targetPosition.y = 0;

        float distance = Vector3.Distance(attackPointPosition, targetPosition);
        float deltaY = attackPoint.position.y - target.position.y;
        float angle = NormalizeAngle(Mathf.Atan2(deltaY, distance) * Mathf.Rad2Deg);

        attackPoint.localEulerAngles = new Vector3(angle, 0, 0);
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
