using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{   
    public Vector3[] loc = {new Vector3(-0.5f, 0.5f, 0), new Vector3(0, 0.5f, 0), 
    new Vector3(0.5f, 0.5f, 0), new Vector3(-0.5f, 0, 0), new Vector3(0.5f, 0, 0), 
    new Vector3(-0.5f, -0.5f, 0), new Vector3(0, -0.5f, 0), new Vector3(0.5f, -0.5f, 0)};
    KeyCode[] keyCodes = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, 
     KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8};
    string[] petName = {"pet1", "pet2", "pet3", "pet4", "pet5", "pet6", "pet7", "pet8"};
    string[] petWeaponImageName = {"pet1_weaponImage", "pet2_weaponImage", "pet3_weaponImage", "pet4_weaponImage"
                                , "pet5_weaponImage", "pet6_weaponImage", "pet7_weaponImage", "pet8_weaponImage"};
    string[] weaponSlotName = {"PetSlot0", "PetSlot1", "PetSlot2", "PetSlot3", "PetSlot4"
                                , "PetSlot5", "PetSlot6", "PetSlot7"};
    public GameObject[] pet = new GameObject[8];
    public GameObject[] weaponSlot = new GameObject[8];
    public GameObject[] petWeaponImage = new GameObject[8];
    public Weapon[] weapon = new Weapon[8];
    public bool isAttackable;
    public GameObject player;
    public Vector3 playerPos;
    public State playerState;

    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            pet[i] = GameObject.Find(petName[i]);
            petWeaponImage[i] = GameObject.Find(petWeaponImageName[i]);  
            weapon[i] = pet[i].GetComponent<Pet>().weapon.GetComponent<Weapon>();
            weaponSlot[i] = GameObject.Find(weaponSlotName[i]);
            pet[i].SetActive(false);
        }
        player = GameObject.Find("player");
    }

    void Update()
    {
        playerState = player.GetComponent<PlayerManager>().state;
        playerPos = player.transform.position;
        for(int i=0; i<8; i++)
        {
            if(pet[i].activeSelf)
            {
                pet[i].transform.position = playerPos + loc[i];
            }
        }
        WaitEquipWeapon();
        DoingwithState(playerState);
    }

    void WaitEquipWeapon()
    {
        for(int i = 0; i < 8; i++)
        {
            if(weaponSlot[i].GetComponent<WeaponSlotUI>().hasWeapon)
            {
                Equip(i);
            }
            else if(weaponSlot[i].GetComponent<WeaponSlotUI>().hasWeapon == false && pet[i].activeSelf)
            {
                Unequip(i);
            }
        }

    }

    void Equip(int i)
    {
        playerPos = player.transform.position;
        pet[i].SetActive(true);
        weapon[i].AddToSkillList();
        string weaponIconPath = "WeaponIcon/" + weaponSlot[i].GetComponent<WeaponSlotUI>().weaponID;
        SpriteRenderer sr = petWeaponImage[i].GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load(weaponIconPath, typeof(Sprite)) as Sprite;
    }

    void Unequip(int i)
    {
        pet[i].SetActive(false);
    }

    void DoingwithState(State playerState)
    {
        switch(playerState)
        {
            case State.HOLD:
            case State.STAY:
            case State.CHASE:
                isAttackable = true;
                break;
            case State.STOP:
            case State.MOVE:
                isAttackable = false;
                break;
        }
    }
}