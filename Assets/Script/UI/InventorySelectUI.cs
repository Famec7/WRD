using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelectUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targetInventory;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = targetInventory.transform.position;
    }
}
