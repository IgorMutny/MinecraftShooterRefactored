using System;
using UnityEngine;

public class AIAttackTargetWithThrowingWeaponTask : AITask
{
    public AIAttackTargetWithThrowingWeaponTask(AI ai) : base(ai)
    {
    }

    public override void OnTick()
    {
        if (_ai.Target == null)
        {
            return;
        }

        if (_ai.CanAttack == true)
        {
            if (IsTargetAtPoint() == true)
            {
                _ai.Character.Inventory.SetInput(true, false, -1, 0);
                _ai.OnAttacking();
            }
        }
    }

    private bool IsTargetAtPoint()
    {
        RaycastHit[] hits = Physics.RaycastAll(_ai.Character.AttackPoint.position,
            _ai.Character.AttackPoint.forward,
            _ai.Info.AI.DistanceToAttack);

        if (hits.Length == 0)
        {
            return false;
        }

        float[] distances = new float[hits.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            distances[i] = Vector3.Distance(_ai.Character.transform.position, hits[i].point);
        }

        Array.Sort(distances, hits);

        if (hits[0].collider.gameObject.TryGetComponent(out Character character) == true)
        { 
            if (character == _ai.Target)
            {
                return true;
            }
        }

        return false;
    }
}
