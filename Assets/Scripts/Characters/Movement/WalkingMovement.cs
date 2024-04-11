using UnityEngine;

public class WalkingMovement : Movement
{
    private Vector3 _jumpVector = new Vector3(0, 6f, 0);
    private float _jumpDuration = 0.25f;
    private float _obstacleRaycastDistance = 1.5f;
    private TimerWrapper _timer;
    private bool _isJumping;

    public WalkingMovement(Character character, CharacterInfo characterInfo)
        : base(character, characterInfo)
    { 
        _timer = ServiceLocator.Get<TimerWrapper>();
        _isJumping = false;
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
        Vector3 prevMovementVector = MovementVector;

        MovementVector = 
            Transform.right * MovementInput.x + 
            Transform.forward * MovementInput.y;
        Vector3 verticalVector = Transform.up * Rigidbody.velocity.y;

        Rigidbody.velocity = 
            MovementVector * MovementSpeed * Character.AppliedEffects.SpeedMultiplier
            + verticalVector;

        if (prevMovementVector != MovementVector)
        {
            if (prevMovementVector == Vector3.zero)
            {
                Character.View.StartMovement();
            }

            if (MovementVector == Vector3.zero)
            {
                Character.View.StopMovement();
            }
        }
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
        if (IsGrounded() == true)
        {
            StopJumping();

            if (ShouldJump() == true)
            {
                _isJumping = true;
                _timer.AddSignal(_jumpDuration, StopJumping);
            }
        }

        if (_isJumping == true)
        {
            Rigidbody.MovePosition(Transform.position + _jumpVector * Time.fixedDeltaTime);
        }
    }

    private void StopJumping()
    {
        _isJumping = false;
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
