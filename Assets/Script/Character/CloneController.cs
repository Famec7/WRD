using System;
using UnityEngine;

public class CloneController : CharacterController
{
    private float _spawnTime = 0.0f;
    private const float DespawnTime = 1.0f;
    
    public void Spawn(Vector2 position)
    {
        this.transform.position = position;
        this.gameObject.SetActive(true);
        
        Data.CurrentWeapon.transform.localPosition = Vector3.zero;
    }
    
    public void Despawn()
    {
        this.gameObject.SetActive(false);
        _spawnTime = 0.0f;
    }
    
    public override void AttachWeapon(WeaponBase weapon)
    {
        ;
    }

    public override void DetachWeapon()
    {
        ;
    }

    private void Update()
    {
        if (_spawnTime > DespawnTime)
        {
            Despawn();
        }
        else
        {
            _spawnTime += Time.deltaTime;
        }
    }
}