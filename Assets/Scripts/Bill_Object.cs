using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Bill_Object : MonoBehaviour
{
    public Bill_Data Data;

    public TextMeshPro Title;
    public TextMeshPro Description;
    public TextMeshPro Countries;

    public Bill_resource CostEnergy;
    public Bill_resource CostMoney;
    public Bill_resource CostHappiness;

    public Bill_resource EffectEnergy;
    public Bill_resource EffectMoney;
    public Bill_resource EffectHappiness;
    public Bill_resource EffectCarbon;

    public Transform CamPos;
    void Start()
    {
      UpdateText();
        
    }
    
    public void UpdateBillData(Bill_Data data)
    {
        Data = data;
        UpdateText();
    }

    private void UpdateText()
    {
        // if (Data !=null)
        // {
        //     Title.text = Data.BillName;
        //     Description.text = Data.BillText;
        //
        //     //set up the list of countries affected
        //     string countryText = "Affects: \r\n";
        //
        //     if(Data.BillAcceptedEffects.RegionsAffected.HasFlag(Regions.All))
        //     {
        //         countryText = countryText +("\t - All \r\n");
        //     }
        //     else
        //     {
        //         if(Data.RegionsAffected.HasFlag(Regions.North_America))
        //         {
        //             countryText = countryText +("\t - North-America \r\n");
        //         }
        //         if(Data.RegionsAffected.HasFlag(Regions.South_America))
        //         {
        //             countryText = countryText +("\t - South-America \r\n");
        //         }
        //         if(Data.RegionsAffected.HasFlag(Regions.Africa))
        //         {
        //             countryText = countryText + ("\t - Africa \r\n");
        //         }
        //         if(Data.RegionsAffected.HasFlag(Regions.Australia))
        //         {
        //             countryText = countryText +("\t - Australia \r\n");
        //         }
        //         if(Data.RegionsAffected.HasFlag(Regions.Asia))
        //         {
        //             countryText = countryText +("\t - Asia \r\n");
        //         }
        //         if(Data.RegionsAffected.HasFlag(Regions.Europe))
        //         {
        //             countryText = countryText +("\t - Europe \r\n");
        //         }
        //     }
        //
        //     Countries.text = countryText;
        //
        //     float defaultPos = CostEnergy.gameObject.transform.localPosition.y;
        //     float copyPos = defaultPos;
        //
        //     CostEnergy.UpdateValue(Data.BillCosts.Energy, ref defaultPos);
        //     CostMoney.UpdateValue(Data.BillCosts.Money, ref defaultPos);
        //     CostHappiness.UpdateValue(Data.BillCosts.Happiness, ref defaultPos);
        //
        //     EffectEnergy.UpdateValue(Data.BillEffects.Energy,ref copyPos);
        //     EffectMoney.UpdateValue(Data.BillEffects.Money, ref copyPos);
        //     EffectHappiness.UpdateValue(Data.BillEffects.Happiness, ref copyPos);
        //     EffectCarbon.UpdateValue(Data.BillEffects.Carbon, ref copyPos);
        // }

    }

    //clicking on the stamp?
    public void OnCollisionEnter()
    {
        Debug.Log("HIT STAMP");
    }

}

