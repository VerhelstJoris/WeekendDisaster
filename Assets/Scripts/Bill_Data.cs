using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Regions
{
    America =1 << 0, 
    Netherlands = 1 <<1,
    Ligma = 1 <<2
}

[CreateAssetMenu(fileName = "BillData", menuName = "ScriptableObjects/BillData", order = 1)]
public class Bill_Data : ScriptableObject
{
   public string BillName;

   public string BillText;

   public Regions RegionsAffected;


}

