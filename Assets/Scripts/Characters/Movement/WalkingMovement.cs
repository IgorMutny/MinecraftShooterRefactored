using UnityEngine;

public class WalkingMovement : Movement
{
    private float _jumpForce = 300f;

    public WalkingMovement(Character character, CharacterInfo characterInfo)
        : base(character, characterInfo) { }

    public override void OnTick()
    {
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
            MovementVector * MovementSpeed * Character.SpeedMultiplier
            + verticalVector;

        //if (_movementVector != Vector3.zero)
        //{
        //    _soundHandler.Walk();
        //}
    }

    private void Rotate()
    {
        if (RotationSpeed == 0)
        {
            Transform.Rotate(0, RotationInput.x * Time.fixedDeltaTime, 0);
        }
        else
        {
            Transform.Rotate(0, Mathf.Sign(RotationInput.x) * RotationSpeed * Time.fixedDeltaTime, 0);
        }
    }

    private void DoJumping()
    {
        if (IsGrounded() == true)
        {
            if (ShouldJump() == true)
            {
                Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
        RaycastHit[] hits = Physics.RaycastAll(transform.position, MovementVector, 1f);
        foreach (RaycastHit hit in hits)
        { 
            if (hit.collider.gameObject.GetComponent<SolidBlock>() != null)
            {
                return true;
            }
        }

        return false;
    }

    //protected override void Animate()
    //{
    //    _animator.SetBool(_isWalkingBoolean, _movementVector != Vector3.zero);
    //}
}
