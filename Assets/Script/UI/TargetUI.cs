using System;
using UnityEngine;

public class TargetUI : MonoBehaviour
{
    public Transform Target { get; set; }

    private void Update()
    {
        if (Target != null)
            this.transform.position = Target.position;
        
        if (Target.gameObject.activeSelf is false)
        {
            Target = null;
            this.gameObject.SetActive(false);
        }
    }
}