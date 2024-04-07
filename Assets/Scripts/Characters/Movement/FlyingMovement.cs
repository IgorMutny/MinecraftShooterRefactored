using UnityEngine;

public class FlyingMovement : Movement
{
    public FlyingMovement(Character character, CharacterInfo characterInfo)
        : base(character, characterInfo)
    {
        Rigidbody.useGravity = false;
    }

    public override void OnTick()
    {
        if (IsAlive == false)
        {
            return;
        }

        base.OnTick();

        Move();

        if (RotationInput.x != 0)
        {
            Rotate();
        }

        if (RotationInput.y != 0)
        {
            Head.RotateX(RotationInput.y);
        }
    }

    private void Move()
    {
        MovementVector =
            (Transform.right * MovementInput.x +
            Transform.forward * MovementInput.y +
            Transform.up * MovementInput.z).normalized;

        Rigidbody.velocity =
            MovementVector * MovementSpeed * Character.AppliedEffects.SpeedMultiplier;
    }

    private void Rotate()
    {
        if (RotationSpeed == 0)
        {
            Transform.Rotate(0, RotationInput.x * Time.fixedDeltaTime, 0);
        }
        else
        {
            Transform.Rotate
                (0, Mathf.Sign(RotationInput.x) * RotationSpeed * Time.fixedDeltaTime, 0);
        }
    }
}
