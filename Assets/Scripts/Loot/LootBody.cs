using UnityEngine;

public class LootBody : MonoBehaviour
{
    private float _rotationTime;
    private float _timer;

    public void SetAnimation(float rotationTime)
    {
        _rotationTime = rotationTime;
        _timer = _rotationTime;
    }

    private void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer < -_rotationTime)
        {
            _timer = _rotationTime;
        }

        float moment = Mathf.Abs(_timer / _rotationTime);

        transform.position = transform.parent.position + Vector3.up * Mathf.Sin(moment * Mathf.PI);

        transform.Rotate(0, 360f / _rotationTime * Time.fixedDeltaTime, 0);
    }
}
