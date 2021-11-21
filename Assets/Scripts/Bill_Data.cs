using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Regions
{
    North_America =1 << 0, 
    South_America = 1 <<1,
    Africa = 1 <<2,
    Australia = 1 <<3,
    Asia = 1 <<4,
    Europe = 1 <<5,
    All = 1 << 6
}

public enum ResourceType
{
    None,
    Carbon,
    Money,
    Energy,
    Happiness,
}

[System.Serializable]
public class BillEffects
{
    public Regions RegionsAffected;
    public float Carbon;
    public SimulationIndustries Industry;
    
    [Range(0, 2)] 
    public float Money = 1;
    [Range(0, 2)] 
    public float Energy = 1;
    [Range(0, 2)] 
    public float Happiness = 1;
}

[System.Serializable]
public class BillCondition
{
    public Regions RegionsAffected;
    public ResourceType Resource;
    public bool MoreThan;
    public float Value;
}

[CreateAssetMenu(fileName = "BillData", menuName = "ScriptableObjects/BillData", order = 1)]
public class Bill_Data : ScriptableObject
{
    public string BillName;

    public string BillText;

    public BillEffects BillAcceptedEffects;
    public BillEffects BillDeniedEffects;

    public Bill_Data FollowUpAccepted;
    public Bill_Data FollowUpDenied;

    public List<BillCondition> BillConditions;

    [HideInInspector]
    public bool accepted;
}