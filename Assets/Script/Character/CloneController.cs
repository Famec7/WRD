using UnityEngine;

public class CloneController : CharacterController
{
    public void Spawn(Vector2 position)
    {
        this.transform.position = position;
        this.gameObject.SetActive(true);
        
        Data.CurrentWeapon.transform.localPosition = Vector3.zero;
    }
    
    public void Despawn()
    {
        this.gameObject.SetActive(false);
    }
    
    public override void AttachWeapon(WeaponBase weapon)
    {
        ;
    }

    public override void DetachWeapon()
    {
        ;
    }
}