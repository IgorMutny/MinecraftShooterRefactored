using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField] private Transform _head;

    private Head _headHandler;
    private Transform _target;

    private void Awake()
    {
        _headHandler = new Head(_head, new Vector2(60, 30));
    }

    private void FixedUpdate()
    {
        if (_target == null)
        {
            _target = ServiceLocator.Get<CharacterCollection>().Player.transform;
        }

        if (_target != null)
        {
            _headHandler.RotateToTarget(_target);
        }
    }

    private void OnDestroy()
    {
        _headHandler = null;
        _target = null;
    }

}
