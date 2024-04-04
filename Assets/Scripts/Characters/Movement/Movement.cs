using UnityEngine;

public abstract class Movement
{
    protected Character Character { get; private set; }
    protected CharacterInfo CharacterInfo { get; private set; }
    protected Head Head { get; private set; }
    protected Rigidbody Rigidbody { get; private set; }
    protected Collider Collider { get; private set; }
    protected Transform Transform { get; private set; }
    protected float MovementSpeed { get; private set; }
    protected float RotationSpeed { get; private set; }
    protected Vector3 MovementInput { get; private set; }
    protected Vector3 RotationInput { get; private set; }
    protected Vector3 MovementVector { get; set; }

    protected bool IsAlive { get; private set; }

    public Movement(Character character, CharacterInfo characterInfo)
    {
        Character = character;
        CharacterInfo = characterInfo;
        Head = new Head(character.Head, characterInfo.HeadMaxAngles);
        Rigidbody = character.GetComponent<Rigidbody>();
        Collider = character.GetComponent<Collider>();
        Transform = character.transform;
        MovementSpeed = characterInfo.MovementSpeed;
        RotationSpeed = characterInfo.RotationSpeed;
        MovementVector = Vector3.zero;
        IsAlive = true;
    }

    public virtual void OnTick()
    {
        if (IsAlive == false)
        {
            return;
        }

        if (IsGrounded() == false)
        {
            MovementSpeed = CharacterInfo.MovementSpeed * 0.75f;
        }
        else
        {
            MovementSpeed = CharacterInfo.MovementSpeed;
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
        Rigidbody.velocity = Vector3.zero;

        GameObject.Destroy(Collider);
        GameObject.Destroy(Rigidbody);

        IsAlive = false;
    }

    protected bool IsGrounded()
    {
        RaycastHit[] hits = Physics.RaycastAll(Character.GroundChecker.position, Vector3.down, 0.01f);
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
