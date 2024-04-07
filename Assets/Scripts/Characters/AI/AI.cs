using UnityEngine;

public class AI
{
    private AIState _currentState;
    private AIState _permanentTasks;
    private float _delayBetweenAttacksCounter;

    public Character Character { get; private set; }
    public CharacterInfo Info { get; private set; }
    public Character Target { get; private set; }

    public bool CanAttack => _delayBetweenAttacksCounter == 0;

    public AI(Character character, CharacterInfo info)
    {
        Character = character;
        Info = info;

        _delayBetweenAttacksCounter = info.AI.DelayBetweenAttacks;

        _permanentTasks = new AIPermanentTasks();
        _permanentTasks.SetAI(this);

        SetState<AIMoveToTargetState>();
    }

    public void OnTick()
    {
        _currentState.OnTick();
        _permanentTasks.OnTick();

        DecreaseDelayBetweenAttacks();
    }

    private void DecreaseDelayBetweenAttacks()
    {
        if (_delayBetweenAttacksCounter > 0)
        {
            _delayBetweenAttacksCounter -= Time.fixedDeltaTime;
            {
                if (_delayBetweenAttacksCounter < 0)
                {
                    _delayBetweenAttacksCounter = 0;
                }
            }
        }
    }

    public void OnAttacking()
    {
        _delayBetweenAttacksCounter = Info.AI.DelayBetweenAttacks;
    }

    public void SetTarget(Character target)
    {
        if (target != null && target.IsAlive == true)
        {
            Target = target;
        }
        else
        {
            Target = null;
        }
    }

    public void SetState<T>() where T : AIState, new()
    {
        if (_currentState is not T)
        {
            _currentState = new T();
            _currentState.SetAI(this);
        }
    }

    public float GetDistanceToTarget()
    {
        if (Target == null)
        {
            return int.MaxValue;
        }

        Vector3 myPosition = Character.transform.position;
        Vector3 targetPosition = Target.transform.position;
        return Vector3.Distance(myPosition, targetPosition);
    }
}
