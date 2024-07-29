using System.Collections.Generic;
using UnityEngine;

public class Pistol : RangedWeapon, ISubject
{
    protected override void Attack()
    {
        base.Attack();
        
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.AttackDamage);
            Notify();
        }
    }

    /***********************Observer Pattern*****************************/

    #region Observer Pattern

    private readonly List<IObserver> _observers = new();
    
    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
        else
            Debug.LogError($"{observer} is not exist in {_observers}");
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.OnNotify();
        }
    }

    #endregion
    
}