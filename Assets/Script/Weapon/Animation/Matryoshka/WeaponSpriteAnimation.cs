using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 마트료시카 스프라이트 돌아가게 하는 클래스
/// </summary>
public class WeaponSpriteAnimation : MonoBehaviour
{
    [Header("Inside Setting")]
    [SerializeField] Transform insideSprite;
    [SerializeField] float insideRotateSpeed = 1f;

    [Header("Outside Setting")]
    [SerializeField] Transform outsideSprite;
    [SerializeField] float outsideRotateSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        insideSprite.eulerAngles = Vector3.forward * (insideSprite.eulerAngles.z + insideRotateSpeed * Time.deltaTime);

        outsideSprite.eulerAngles = Vector3.forward * (outsideSprite.eulerAngles.z + outsideRotateSpeed * Time.deltaTime);
    }
}
