using System;
using UnityEngine;

public class PetController : CharacterController, IObserver
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Vector2 _offsetFromPlayer;

    [Space] [SerializeField] private Transform _arm;

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
        Move();

        if (Data.CurrentWeapon is null)
        {
            return;
        }

        if (Data.CurrentWeapon.enabled is false)
        {
            return;
        }
        
        if (IsTargetNullOrInactive())
        {
            Target = FindNearestTarget();
        }
        else
        {
            Data.CurrentWeapon.UpdateAttack();
        }
    }

    public void OnNotify()
    {
        if (_playerController.Target == null)
        {
            if (!IsPlayerStateIdleAndWeaponNotNull()) return;

            return;
        }

        Target = _playerController.Target;
    }

    private void Move()
    {
        Vector3 newPos = _playerController.transform.position + (Vector3)_offsetFromPlayer;
        
        //화면 밖으로 나가지 않게 하기
        newPos.x = Mathf.Clamp(newPos.x, -screenBound.x, screenBound.x);
        newPos.y = Mathf.Clamp(newPos.y, -screenBound.y, screenBound.y);
        
        transform.position = newPos;

        if (Data.CurrentWeapon == null)
        {
            return;
        }
        
        GameObject target = Data.CurrentWeapon.owner.Target;
        if (target == null)
        {
            return;
        }

        if (target.transform.position.x > transform.position.x)
            this.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        else
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        if(target == null)
            this.transform.rotation = _playerController.transform.rotation;
    }
    
    private bool IsPlayerStateIdleAndWeaponNotNull()
    {
        return _playerController.CurrentState is PlayerController.State.IDLE && Data.CurrentWeapon is not null;
    }
    
    private bool IsTargetNullOrInactive()
    {
        return Target is null || Target.activeSelf is false;
    }

    public override void AttachWeapon(WeaponBase weapon)
    {
        Data.SetCurrentWeapon(weapon);
        
        weapon.transform.position = this.transform.position;
        
        /******************무기 부모 설정********************/
        weapon.transform.SetParent(_arm);
        
        /******************무기 크기 조정********************/
        this.transform.localScale = new Vector3(0.6f, 0.6f, 0f);
        
        /******************무기 위치 조정********************/
        weapon.transform.localRotation = weapon.Pivot.GetOriginRotation();
        
        FloatingIdleMotion floatingIdleMotion = GetComponent<FloatingIdleMotion>();
        floatingIdleMotion.PlayFloatingIdle(weapon.transform);
        floatingIdleMotion.ShowShadow(true);
    }

    public override void DetachWeapon()
    {
        Target = null;
        Data.CurrentWeapon.transform.SetParent(null);
        
        this.transform.localScale = new Vector3(1.0f, 1.0f, 0f);
        
        FloatingIdleMotion floatingIdleMotion = GetComponent<FloatingIdleMotion>();
        floatingIdleMotion.StopFloatingIdle();
        floatingIdleMotion.ShowShadow(false);
        
        Data.SetCurrentWeapon(null);
    }
}