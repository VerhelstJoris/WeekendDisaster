using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public enum SimulationMode
{
    Step,
    Continuous
}

public class Simulation : MonoBehaviour
{
    // Data Input
    public TextAsset dataFile;
    
    // Statics
    public readonly float StartTemp = 0.8f;
    public readonly float EndTemp = 2.0f;
    public readonly DateTime StartDate = DateTime.UtcNow;
    public readonly DateTime EndDate = new DateTime(2040, 1,1, 0,0,0).ToUniversalTime();
    
    
    // Current Data
    public DateTime CurrentDate = DateTime.UtcNow;
    public Dictionary<Regions, RegionData> worldData = new Dictionary<Regions, RegionData>();
    public List<Bill_Data> activeModifiers = new List<Bill_Data>();
    
    public float globalC02 = 0;
    public float currentRelativeTemp = 0;

    // Settings
    public SimulationMode simMode = SimulationMode.Step;
    public bool runSim = true;
    public int DaysPerTick = 7;

    private void Awake()
    {
        LoadData();
        Debug.Log("Loaded the Sim data!");
    }

    IEnumerator TimedUpdate()
    {
        while(runSim)
        {
            StepSim();
            yield return new WaitForSeconds(1);
        }
    }
    
    private void Start()
    {
        if (simMode == SimulationMode.Continuous)
        {
            Debug.Log("Starting Sim in continuous mode");
            StartCoroutine(TimedUpdate());
        }
        Debug.Log("Starting Sim in Step mode, Don't forget to call step!");
    }

    private void LoadData()
    {

        #region LoadDataFromDisk

        

        var lines = dataFile.text.Split('\n');

        var skipFirstLine = true;
        
        foreach (var l in lines)
        {
            if (skipFirstLine)
            {
                skipFirstLine = false;
                continue;
            }
            
            var data = l.Split(',');
            RegionData region = new RegionData();

            region.location = data[0] switch
            {
                "Africa" => Regions.Africa,
                "Asia" => Regions.Asia,
                "Australia" => Regions.Australia,
                "Europe" => Regions.Europe,
                "NorthAmerica" => Regions.North_America,
                "SouthAmerica" => Regions.South_America,
                _ => throw new NotImplementedException($"There is some continent ({data[0]}) , I have never heard of in the data! FIXME!")
            };
            
             region.co2 = float.Parse(data[1]);
             region.co2_growth_abs = float.Parse(data[2]);
             region.co2_per_capita = float.Parse(data[3]);
             region.share_global_co2 = float.Parse(data[4]);
             region.cumulative_co2 = float.Parse(data[5]);
             region.share_global_cumulative_co2 = float.Parse(data[6]);
             region.coal_co2 = float.Parse(data[7]);
             region.cement_co2 = float.Parse(data[8]);
             region.flaring_co2 = float.Parse(data[9]);
             region.gas_co2 = float.Parse(data[10]);
             region.oil_co2 = float.Parse(data[11]);
             region.other_industry_co2 = float.Parse(data[12]);
             region.cement_co2_per_capita = float.Parse(data[13]);
             region.coal_co2_per_capita = float.Parse(data[14]);
             region.flaring_co2_per_capita = float.Parse(data[15]);
             region.gas_co2_per_capita = float.Parse(data[16]);
             region.oil_co2_per_capita = float.Parse(data[17]);
             region.other_co2_per_capita = float.Parse(data[18]);
             region.share_global_cement_co2 = float.Parse(data[19]);
             region.share_global_coal_co2 = float.Parse(data[20]);
             region.share_global_flaring_co2 = float.Parse(data[21]);
             region.share_global_gas_co2 = float.Parse(data[22]);
             region.share_global_oil_co2 = float.Parse(data[23]);
             region.share_global_other_co2 = float.Parse(data[24]);
             region.cumulative_cement_co2 = float.Parse(data[25]);
             region.cumulative_coal_co2 = float.Parse(data[26]);
             region.cumulative_flaring_co2 = float.Parse(data[27]);
             region.cumulative_gas_co2 = float.Parse(data[28]);
             region.cumulative_oil_co2 = float.Parse(data[29]);
             region.cumulative_other_co2 = float.Parse(data[30]);
             region.share_global_cumulative_cement_co2 = float.Parse(data[31]);
             region.share_global_cumulative_coal_co2 = float.Parse(data[32]);
             region.share_global_cumulative_flaring_co2 = float.Parse(data[33]);
             region.share_global_cumulative_gas_co2 = float.Parse(data[34]);
             region.share_global_cumulative_oil_co2 = float.Parse(data[35]);
             region.share_global_cumulative_other_co2 = float.Parse(data[36]);
             
             region.population = long.Parse(data[37]);

             worldData[region.location] = region;
        }
        
        #endregion

        #region SimSetup

        globalC02 = worldData.Values.Sum(data => data.co2);
        currentRelativeTemp = 0.8f;

        #endregion

    }
    
    public void StepSim()
    {
         
        
    }
}






