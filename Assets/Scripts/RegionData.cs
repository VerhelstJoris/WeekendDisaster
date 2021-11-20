using System;
using UnityEngine;

[Serializable]
public class RegionData 
{
    public Regions location;
    public long population;

    public float share_global_cumulative_co2;
    public float happinessStat = 1.0f;
    public float moneyStat = 1.0f;
    public float energy = 1.0f;

    public float co2;
    public float co2_growth_abs;
    public float co2_per_capita;
    public float share_global_co2;
    public float cumulative_co2;
    public float coal_co2;
    public float cement_co2;
    public float flaring_co2;
    public float gas_co2;
    public float oil_co2;
    public float other_industry_co2;
    public float cement_co2_per_capita;
    public float coal_co2_per_capita;
    public float flaring_co2_per_capita;
    public float gas_co2_per_capita;
    public float oil_co2_per_capita;
    public float other_co2_per_capita;
    public float share_global_cement_co2;
    public float share_global_coal_co2;
    public float share_global_flaring_co2;
    public float share_global_gas_co2;
    public float share_global_oil_co2;
    public float share_global_other_co2;
    public float cumulative_cement_co2;
    public float cumulative_coal_co2;
    public float cumulative_flaring_co2;
    public float cumulative_gas_co2;
    public float cumulative_oil_co2;
    public float cumulative_other_co2;
    public float share_global_cumulative_cement_co2;
    public float share_global_cumulative_coal_co2;
    public float share_global_cumulative_flaring_co2;
    public float share_global_cumulative_gas_co2;
    public float share_global_cumulative_oil_co2;
    public float share_global_cumulative_other_co2;
    
    public void SetCO2(SimulationIndustries industry, float value)
    
    {
        switch (industry)
        {
            case SimulationIndustries.Coal:
                coal_co2 *= value;
                break;
            case SimulationIndustries.Cement:
                cement_co2 *= value;
                break;
            case SimulationIndustries.Flaring:
                flaring_co2 *= value;
                break;
            case SimulationIndustries.Gas:
                gas_co2 *= value;
                break;
            case SimulationIndustries.Oil:
                oil_co2 *= value;
                break;
            case SimulationIndustries.Other:
                other_industry_co2 *= value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(industry), industry, $"No idea how to update for industry type {Enum.GetName(typeof(SimulationIndustries), industry)}");
        }
    }
}

public enum SimulationMode
{
    Step,
    Continuous
}

public enum SimulationIndustries
{ 
    Coal,
    Cement,
    Flaring,
    Gas,
    Oil,
    Other
}
