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



    void Start()
    {
      
        if(Data !=null)
        {
            Title.text = Data.BillName;
            Description.text = Data.BillText;

            //set up the list of countries affected
            string countryText = "Affects: \r\n";

            if(Data.RegionsAffected.HasFlag(Regions.All))
            {
                countryText = countryText +("\t - All \r\n");
            }
            else
            {
                if(Data.RegionsAffected.HasFlag(Regions.North_America))
                {
                    countryText = countryText +("\t - North-America \r\n");
                }
                if(Data.RegionsAffected.HasFlag(Regions.South_America))
                {
                    countryText = countryText +("\t - South-America \r\n");
                }
                if(Data.RegionsAffected.HasFlag(Regions.Africa))
                {
                    countryText = countryText + ("\t - Africa \r\n");
                }
                if(Data.RegionsAffected.HasFlag(Regions.Australia))
                {
                    countryText = countryText +("\t - Australia \r\n");
                }
                if(Data.RegionsAffected.HasFlag(Regions.Asia))
                {
                    countryText = countryText +("\t - Asia \r\n");
                }
                if(Data.RegionsAffected.HasFlag(Regions.Europe))
                {
                    countryText = countryText +("\t - Europe \r\n");
                }
            }

            Countries.text = countryText;

            CostEnergy.UpdateValue(Data.BillCosts.Energy);
            CostMoney.UpdateValue(Data.BillCosts.Money);
            CostHappiness.UpdateValue(Data.BillCosts.Happiness);

            EffectEnergy.UpdateValue(Data.BillEffects.Energy);
            EffectMoney.UpdateValue(Data.BillEffects.Money);
            EffectHappiness.UpdateValue(Data.BillEffects.Happiness);
            EffectCarbon.UpdateValue(Data.BillEffects.Carbon);

        }

    }
}
