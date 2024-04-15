using UnityEngine;

public class WalkingMovement : Movement
{
    private Vector3 _jumpVector = new Vector3(0, 6f, 0);
    private Vector3 _gravityVector = new Vector3(0, -4f, 0);
    private float _jumpDuration = 0.25f;
    private float _obstacleRaycastDistance => Controller.radius + 0.5f;
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

        Controller.Move(MovementVector * MovementSpeed
            * Character.AppliedEffects.SpeedMultiplier * Time.fixedDeltaTime);

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
            Controller.Move(_jumpVector * Time.fixedDeltaTime);
        }
        else
        {
            Controller.Move(_gravityVector * Time.fixedDeltaTime);
        }
    }

    private void StopJumping()
    {
        _isJumping = false;
    }

    private bool ShouldJump()
    {
        if (MovementVector == Vector3.zero)
        {
            return false;
        }

        bool wayIsBlocked = IsWayBlocked(Character.LowerObstacleChecker);
        bool wayAboveIsBlocked = IsWayBlocked(Character.UpperObstacleChecker);

        return wayIsBlocked == true && wayAboveIsBlocked == false;
    }

    private bool IsWayBlocked(Transform transform)
    {
        Vector3[] vectors = new Vector3[]
        {
            new Vector3(Mathf.Sign(MovementVector.x), 0, 0),
            new Vector3(0, 0, Mathf.Sign(MovementVector.z)),
        };

        for (int i = 0; i < vectors.Length; i++)
        { 
            if (IsWayBlockedInDirection(transform, vectors[i]) == true)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsWayBlockedInDirection(Transform transform, Vector3 direction)
    {
        RaycastHit[] hits = Physics.RaycastAll
            (transform.position, direction, _obstacleRaycastDistance);

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
