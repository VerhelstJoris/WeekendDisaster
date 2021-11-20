using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill_Object : MonoBehaviour
{
    public Bill_Data Data;

    public TextMesh Title;
    public TextMesh Description;
    public TextMesh Countries;
    public TextMesh Costs; 
    public TextMesh Effects; 


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


        }

    }
}
