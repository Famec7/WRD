using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo : MonoBehaviour
{
    public List<MissionSlot> _missionSlots;

    private void Awake()
    {
        int count = this.transform.childCount;
        _missionSlots = new List<MissionSlot>();
        for (int i = 0; i < count; i++)
            _missionSlots.Add(this.transform.GetChild(i).GetComponent<MissionSlot>());
    }

    private void Start()
    {
        for (int i = 0; i < _missionSlots.Count; i++)
        {
            _missionSlots[i].InitMissionSlot(i);
        }
    }
}