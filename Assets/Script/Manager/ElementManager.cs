using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum ElementType
    {
        FIRE,
        WATER,
        LEAF,
        LIGHT,
        DARK
    }

    [HideInInspector]
    public static ElementManager instance;

    //fire leaf water light dark =>
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetElement(int cnt)
    {
        for (int i =0; i <cnt; i++)
        {
            int elementType = Random.Range(0, 5);
            GameManager.Instance.weaponCnt[elementType]++;
        }

    }
}
