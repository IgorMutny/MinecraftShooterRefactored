using UnityEngine;

public abstract class Movement
{
    protected Character Character { get; private set; }
    protected CharacterInfo Info { get; private set; }
    protected Head Head { get; private set; }
    protected CharacterController Controller { get; private set; }
    protected Collider[] Colliders { get; private set; }
    protected Transform Transform { get; private set; }
    protected float MovementSpeed { get; private set; }
    protected float RotationSpeed { get; private set; }
    protected Vector3 MovementInput { get; private set; }
    protected Vector3 RotationInput { get; private set; }
    protected Vector3 MovementVector { get; set; }

    protected bool IsAlive { get; private set; }
    protected bool IsWalking { get; private set; }

    public Movement(Character character, CharacterInfo info)
    {
        Character = character;
        Info = info;
        Head = new Head(character.Head, info.HeadMaxAngles);
        Controller = character.GetComponent<CharacterController>();
        Colliders = character.GetComponents<Collider>();
        Transform = character.transform;
        MovementSpeed = info.MovementSpeed;
        RotationSpeed = info.RotationSpeed;
        MovementVector = Vector3.zero;
        IsAlive = true;
        IsWalking = info.MovementType == MovementType.Walking;
    }

    public virtual void OnTick()
    {
        if (IsAlive == false)
        {
            return;
        }

        if (IsWalking == true)
        {
            if (IsGrounded() == false)
            {
                MovementSpeed = Info.MovementSpeed * 0.75f;
            }
            else
            {
                MovementSpeed = Info.MovementSpeed;
            }
        }

        PreventFallingOutOfBounds();
    }

    private void PreventFallingOutOfBounds()
    {
        if (Transform.position.y < -10)
        {
            Transform.position = new Vector3(Transform.position.x, 20, Transform.position.z);
        }
    }

    public void SetInput(Vector3 movement, Vector3 rotation)
    {
        if (IsAlive == false)
        {
            return;
        }

        MovementInput = movement;
        RotationInput = rotation;
    }

    public void RotateHeadToTarget(Transform target)
    {
        if (IsAlive == false)
        {
            return;
        }

        Head.RotateToTarget(target);
    }

    public void OnDied()
    {
        MovementInput = Vector3.zero;
        RotationInput = Vector3.zero;

        foreach (Collider collider in Colliders)
        {
            GameObject.Destroy(collider);
        }

        IsAlive = false;
    }

    protected bool IsGrounded()
    {
        RaycastHit[] hits = Physics.RaycastAll(Character.GroundChecker.position, Vector3.down, 0.1f);
        foreach (RaycastHit hit in hits)
        { 
            if (hit.collider.gameObject.GetComponent<SolidBlock>() != null)
            {
                return true;
            }
        }

        return false;
    }
}
