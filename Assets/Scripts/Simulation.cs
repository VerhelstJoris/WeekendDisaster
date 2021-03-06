using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Simulation : MonoBehaviour
{
    public event Action OnStepped;
    public event Action OnSimPaused;
    public event Action OnSimFinalDateReached;
    public event Action OnSimLedToFailure;

    // Data Input
    public TextAsset dataFile;

    // Statics
    public readonly float StartTemp = 0.8f;
    public readonly float EndTemp = 2.0f;
    public readonly DateTime EndDate = new DateTime(2040, 1, 1, 0, 0, 0);


    // Current Data
    public DateTime CurrentDate = new DateTime(2021,1, 1, 0,0,0);
    public Dictionary<Regions, RegionData> worldData = new Dictionary<Regions, RegionData>();

    [FormerlySerializedAs("activeModifiers")]
    public List<Bill_Data> bills = new List<Bill_Data>();

    public float globalCO2 = 0;
    public float globalCO2TargetMultiplier = 13.5f;
    public float globalCO2Target = 0;
    public float currentRelativeTemp = 0;
    public float currentTemp = 0;

    public Dictionary<SimulationIndustries, float> GlobalIndustryOutput = new Dictionary<SimulationIndustries, float>();

    // Settings
    public SimulationMode simMode = SimulationMode.Step;
    public bool runSim = false;
    public bool gameWon = false;
    public bool gameLost = false;
    public int stepMonths = 3;
    public float stepDays = 29;
    public float nonCO2StatGainBoost = 1.0f;

    public List<RegionStatsCustom> regionStatsCustom = new List<RegionStatsCustom>();
    public bool useCustomRegionData = true;

    public bool simpleMode = true;

    [Range(0.75f, 1.25f)] public float CO2IncresePerIndustryMultiplier = 1.15f;

    private DateTime PauseDate;
    private float monthsTickingThisSim;

    private void Awake()
    {
        LoadData();
        Debug.Log("Loaded the Sim data!");
    }

    IEnumerator TimedUpdate()
    {
        while (runSim)
        {
            StepSim();
            if (OnStepped != null)
            {
                OnStepped();
            }

            yield return new WaitForSeconds(0.4f);
        }
    }

    private void Start()
    {
        //if (simMode == SimulationMode.Continuous)
        //{
        //    Debug.Log("Starting Sim in continuous mode");
        //    StartCoroutine(TimedUpdate());
        //    return;
        //}
        //Debug.Log("Starting Sim in Step mode, Don't forget to call step!");
    }

    public void StartSim(int monthsToTick)
    {
        monthsTickingThisSim = monthsToTick;
        PauseDate = CurrentDate.AddMonths(monthsToTick).AddDays(Random.value * stepDays);
        runSim = true;
        StartCoroutine(TimedUpdate());
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
                _ => throw new NotImplementedException(
                    $"There is some continent ({data[0]}) , I have never heard of in the data! FIXME!")
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

        // Use Custom region data if required 
        if (useCustomRegionData)
        {
            foreach (var r in regionStatsCustom)
            {
                if (simpleMode)
                {
                    worldData[r.region].co2 = r.Carbon;
                    worldData[r.region].cumulative_co2 = r.Carbon;
                }

                worldData[r.region].happinessStat = r.Happiness;
                worldData[r.region].moneyStat = r.Money;
                worldData[r.region].energy = r.Energy;
            }
        }


        #region SimSetup

        // Global State Values
        globalCO2 = worldData.Values.Sum(data => data.co2);
        globalCO2Target = globalCO2 * globalCO2TargetMultiplier;

        foreach (var r in worldData.Values)
        {
            r.share_global_cumulative_co2 = Mathf.Clamp01(r.co2 / globalCO2);
        }

        // Work out new doomsday value (temp increase)
        float diff = globalCO2Target - globalCO2;
        currentTemp = 1 - (diff / globalCO2Target);

        #endregion

        Debug.Log("Finished setting up sim data");
    }

    public void StepSim()
    {
        //  Update the Quarterly
        CurrentDate = CurrentDate.AddMonths(stepMonths).AddDays(Random.value * stepDays);
        if (CurrentDate >= EndDate)
        {
            gameWon = true;
        }
        if (currentTemp >= 0.99f)
        {
            gameLost = true;
        }

        if (CurrentDate >= PauseDate)
        {
            runSim = false;
            Debug.Log("Sim has paused");
            if (OnSimPaused != null)
            {
                OnSimPaused();
            }
            bills.Clear();

            return;
        }

        if (gameLost)
        {
            bills.Clear();
            runSim = false;
            Debug.Log("Sim has led to fail state");
            if (OnSimLedToFailure != null)
            {
                OnSimLedToFailure();
            }
            return;
        }

        if (gameWon)
        {
            bills.Clear();
            runSim = false;
            Debug.Log("Sim has finished");
            if (OnSimFinalDateReached != null)
            {
                OnSimFinalDateReached();
            }

            return;
        }

        Debug.Log($"Stepping Sim for date {CurrentDate.ToString(CultureInfo.InvariantCulture)}");

        // Update regional values based on increase multiplier

        foreach (var r in worldData.Values)
        {
            if (simpleMode)
            {
                r.co2 *= CO2IncresePerIndustryMultiplier;
            }
            else
            {
                r.coal_co2 *= CO2IncresePerIndustryMultiplier;
                r.cement_co2 *= CO2IncresePerIndustryMultiplier;
                r.flaring_co2 *= CO2IncresePerIndustryMultiplier;
                r.gas_co2 *= CO2IncresePerIndustryMultiplier;
                r.oil_co2 *= CO2IncresePerIndustryMultiplier;
                r.other_industry_co2 *= CO2IncresePerIndustryMultiplier;
            }
            
        }

        // Update the bills
        foreach (var currentBill in bills)
        {
            var affectedRegions = new List<Regions>();

            var effects = currentBill.accepted ? currentBill.BillAcceptedEffects : currentBill.BillDeniedEffects;

            if (effects.RegionsAffected.HasFlag(Regions.North_America)) affectedRegions.Add(Regions.North_America);
            if (effects.RegionsAffected.HasFlag(Regions.South_America)) affectedRegions.Add(Regions.South_America);
            if (effects.RegionsAffected.HasFlag(Regions.Africa)) affectedRegions.Add(Regions.Africa);
            if (effects.RegionsAffected.HasFlag(Regions.Australia)) affectedRegions.Add(Regions.Australia);
            if (effects.RegionsAffected.HasFlag(Regions.Asia)) affectedRegions.Add(Regions.Asia);
            if (effects.RegionsAffected.HasFlag(Regions.Europe)) affectedRegions.Add(Regions.Europe);
            if (effects.RegionsAffected.HasFlag(Regions.All))
            {
                affectedRegions.AddRange(Enum.GetValues(typeof(Regions)).Cast<Regions>());
            }

            BillEffects effectsPerTick = new BillEffects();
            float rootFactor = monthsTickingThisSim / stepMonths;
            effectsPerTick.Carbon = Mathf.Pow(effects.Carbon, 1f / rootFactor);
            effectsPerTick.Happiness = Mathf.Pow(nonCO2StatGainBoost * effects.Happiness, 1f / rootFactor);
            effectsPerTick.Money = Mathf.Pow(nonCO2StatGainBoost * effects.Money, 1f / rootFactor);
            effectsPerTick.Energy = Mathf.Pow(nonCO2StatGainBoost * effects.Energy, 1f / rootFactor);

            // Update the regions values based on the bills
            foreach (var r in worldData.Values.Where(r => affectedRegions.Contains(r.location)))
            {
                r.happinessStat = Mathf.Clamp01(r.happinessStat * effectsPerTick.Happiness);
                r.moneyStat = Mathf.Clamp01(r.moneyStat * effectsPerTick.Money);
                r.energy = Mathf.Clamp01(r.energy * effectsPerTick.Energy);

                if (simpleMode)
                {
                    r.co2_growth_rate_per_year *= effectsPerTick.Carbon;
                    r.co2_growth_rate_per_year = Mathf.Clamp(r.co2_growth_rate_per_year, RegionData.base_co2_growth_rate_per_year, 2.0f);

                    r.co2 *= Mathf.Pow(r.co2_growth_rate_per_year, 1f / rootFactor);
                }
                else
                {
                    r.SetCO2(effects.Industry, effectsPerTick.Carbon);
                }

                r.share_global_cumulative_co2 = Mathf.Clamp01(r.co2 / globalCO2);
            }
            foreach (var r in worldData.Values.Where(r => !affectedRegions.Contains(r.location)))
            {
                if (simpleMode)
                {
                    r.co2 *= Mathf.Pow(r.co2_growth_rate_per_year, 1f / rootFactor);
                }
                else
                {
                    //r.SetCO2(effects.Industry, effectsPerTick.Carbon);
                }

                r.share_global_cumulative_co2 = Mathf.Clamp01(r.co2 / globalCO2);
            }
        }

        // Update Global Totals
        if (!simpleMode)
        {
            GlobalIndustryOutput[SimulationIndustries.Coal] = worldData.Values.Sum(date => date.coal_co2);
            GlobalIndustryOutput[SimulationIndustries.Cement] = worldData.Values.Sum(date => date.cement_co2);
            GlobalIndustryOutput[SimulationIndustries.Flaring] = worldData.Values.Sum(date => date.flaring_co2);
            GlobalIndustryOutput[SimulationIndustries.Gas] = worldData.Values.Sum(date => date.gas_co2);
            GlobalIndustryOutput[SimulationIndustries.Oil] = worldData.Values.Sum(date => date.oil_co2);
            GlobalIndustryOutput[SimulationIndustries.Other] =
                worldData.Values.Sum(date => date.other_industry_co2);
        }

        // Update Global CO2 the previous world total and the new amount released
        globalCO2 = worldData.Values.Sum(data => data.co2);

        // Work out new doomsday value (temp increase)
        float diff = globalCO2Target - globalCO2;
        currentTemp = 1 - (diff / globalCO2Target);


        Debug.Log($"Current Temp {currentTemp}");

        // Debug output for globals
        // foreach (var industry in Enum.GetValues(typeof(SimulationIndustries)))
        // {
        //     var i = (SimulationIndustries) industry;
        //     Debug.Log($"{Enum.GetName(typeof(SimulationIndustries), industry)} - [{GlobalIndustryOutput[i]}]");
        // }
    }

    public bool IsConditionMet(BillCondition condi)
    {
        foreach (var r in worldData.Values.Where(r => condi.RegionsAffected.HasFlag(r.location)))
        {
            float compareValue = 0;
            switch (condi.Resource)
            {
                case ResourceType.None:
                    continue;
                case ResourceType.Carbon:
                    compareValue = r.energy;
                    break;
                case ResourceType.Money:
                    compareValue = r.moneyStat;
                    break;
                case ResourceType.Energy:
                    compareValue = r.energy;
                    break;
                case ResourceType.Happiness:
                    compareValue = r.happinessStat;
                    break;
                default:
                    continue;
            }

            if (condi.MoreThan)
            {
                if (condi.Value > compareValue)
                {
                    continue;
                }
            }
            else
            {
                if (condi.Value <= compareValue)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }
}