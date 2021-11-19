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
    Europe = 1 <<5
}

public struct BillCosts
{
    int CurrencyCost;
    int EnergyCost;
    int HappinessCost;
}

public struct BillEffects : BillCosts
{
    int CarbonEffect;
}

[CreateAssetMenu(fileName = "BillData", menuName = "ScriptableObjects/BillData", order = 1)]
public class Bill_Data : ScriptableObject
{
   public string BillName;

   public string BillText;

   public Regions RegionsAffected;

   public BillCosts BillCosts;

   public BillEffects;

}

