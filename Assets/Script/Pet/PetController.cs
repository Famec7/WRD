using System;
using UnityEngine;

public class PetController : CharacterController, IObserver
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Vector2 _offsetFromPlayer;

    private void Start()
    {
        _offsetFromPlayer = this.transform.position;
        _playerController.AddObserver(this);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _offsetFromPlayer, Data.MoveSpeed * Time.deltaTime);
    }

    public void OnNotify()
    {
        if (_playerController.Target == null)
        {
            if (_playerController.CurrentState == PlayerController.State.IDLE)
            {
                FindNearestTarget();
            }

            return;
        }

        Target = _playerController.Target;
    }
}