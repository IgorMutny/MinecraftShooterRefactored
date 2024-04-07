using UnityEngine;

public class WalkingMovement : Movement
{
    private Vector3 _jumpVector = new Vector3(0, 6f, 0);
    private float _jumpDuration = 0.25f;
    private float _jumpDurationCounter;
    private float _obstacleRaycastDistance = 1.5f;

    public WalkingMovement(Character character, CharacterInfo characterInfo)
        : base(character, characterInfo)
    {

    }

    public override void OnTick()
    {
        if (IsAlive == false)
        {
            return;
        }

        base.OnTick();

        DoJumping();
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
            Transform.right * MovementInput.x + 
            Transform.forward * MovementInput.y;
        Vector3 verticalVector = Transform.up * Rigidbody.velocity.y;

        Rigidbody.velocity = 
            MovementVector * MovementSpeed * Character.AppliedEffects.SpeedMultiplier
            + verticalVector;
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

    private void DoJumping()
    {
        DecreaseJumpCounter();

        if (IsGrounded() == true)
        {
            _jumpDurationCounter = 0;

            if (ShouldJump() == true)
            {
                _jumpDurationCounter = _jumpDuration;
            }
        }

        if (_jumpDurationCounter > 0)
        {
            Rigidbody.MovePosition(Transform.position + _jumpVector * Time.fixedDeltaTime);
        }
    }

    private void DecreaseJumpCounter()
    {
        if (_jumpDurationCounter > 0)
        {
            _jumpDurationCounter -= Time.fixedDeltaTime;

            if (_jumpDurationCounter < 0)
            {
                _jumpDurationCounter = 0;
            }
        }
    }

    private bool ShouldJump()
    {
        bool wayIsBlocked = IsWayBlocked(Character.LowerObstacleChecker);
        bool wayAboveIsBlocked = IsWayBlocked(Character.UpperObstacleChecker); 

        return wayIsBlocked == true && wayAboveIsBlocked == false;
    }

    private bool IsWayBlocked(Transform transform)
    {
        RaycastHit[] hits = Physics.RaycastAll
            (transform.position, MovementVector, _obstacleRaycastDistance);

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
