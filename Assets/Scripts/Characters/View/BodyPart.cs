using Unity.VisualScripting;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private bool _isStopped;
    private Vector3 _prevPosition;
    private Collider _collider;
    private Rigidbody _rigidbody;


    public void Initialize()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = true;
        _rigidbody = transform.AddComponent<Rigidbody>();
        _isStopped = false;
        _prevPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (_isStopped == false)
        {
            if (_prevPosition == transform.position)
            {
                Stop();
            }

            _prevPosition = transform.position;
        }
    }

    private void Stop()
    {
        _isStopped = true;
        Destroy(_rigidbody);
        Destroy(_collider);
    }
}
