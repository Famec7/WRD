using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkEffecter : MonoBehaviour
{
    [SerializeField] float animSpeed = 1f;
    [SerializeField] float animSizeChangeAmount = 0.1f;

    Vector3 defaultSize;

    void Awake()
    {
        defaultSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = defaultSize * (1f + Mathf.Sin(Time.time * animSpeed) * animSizeChangeAmount);
    }
}
