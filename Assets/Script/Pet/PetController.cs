using System;
using UnityEngine;

public class PetController : CharacterController, IObserver
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Vector2 _offsetFromPlayer;

    private Vector3 screenBound;

    private void Start()
    {
        _offsetFromPlayer = this.transform.position;
        _playerController.AddObserver(this);

        screenBound =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    private void Update()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, _offsetFromPlayer, Data.MoveSpeed * Time.deltaTime);
        //화면 밖으로 나가지 않게 하기
        newPos.x = Mathf.Clamp(newPos.x, -screenBound.x, screenBound.x);
        newPos.y = Mathf.Clamp(newPos.y, -screenBound.y, screenBound.y);
        
        transform.position = newPos;
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