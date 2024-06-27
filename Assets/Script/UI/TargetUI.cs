using System;
using UnityEngine;

public class TargetUI : MonoBehaviour
{
    private Transform _target;

    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;
            this.gameObject.SetActive(value != null);
        }
    }

    private void Update()
    {
        if (_target != null)
            this.transform.position = Target.position;
    }
}