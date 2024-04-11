using UnityEngine;

public class AI
{
    private AIState _currentState;
    private AIState _permanentTasks;
    private TimerWrapper _timer;

    public Character Character { get; private set; }
    public CharacterInfo Info { get; private set; }
    public Character Target { get; private set; }

    public bool CanAttack { get; private set; }

    public AI(Character character, CharacterInfo info)
    {
        Character = character;
        Info = info;

        _timer = ServiceLocator.Get<TimerWrapper>();

        _permanentTasks = new AIPermanentTasks();
        _permanentTasks.SetAI(this);

        CanAttack = true;

        SetState<AIMoveToTargetState>();
    }

    public void OnTick()
    {
        _currentState.OnTick();
        _permanentTasks.OnTick();
    }


    public void OnAttacking()
    {
        CanAttack = false;
        _timer.AddSignal(Info.AI.DelayBetweenAttacks, AllowAttack);
    }

    private void AllowAttack()
    {
        CanAttack = true;
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
