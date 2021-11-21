using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapStats : MonoBehaviour
{
    [SerializeField]
    private StatsView NorthAmericaStats;
    [SerializeField]
    private StatsView SouthAmericaStats;
    [SerializeField]
    private StatsView EuropeStats;
    [SerializeField]
    private StatsView AfricaStats;
    [SerializeField]
    private StatsView AsiaStats;
    [SerializeField]
    private StatsView AustraliaStats;
    [SerializeField]
    private Slider TemperatureSlider;

    public void UpdateStats(Simulation sim)
    {
        foreach (var data in sim.worldData)
        {
            switch (data.Key)
            {
                case Regions.North_America:
                    UpdateStatsView(NorthAmericaStats, data.Value);
                    break;
                case Regions.South_America:
                    UpdateStatsView(SouthAmericaStats, data.Value);
                    break;
                case Regions.Africa:
                    UpdateStatsView(AfricaStats, data.Value);
                    break;
                case Regions.Australia:
                    UpdateStatsView(AustraliaStats, data.Value);
                    break;
                case Regions.Asia:
                    UpdateStatsView(AsiaStats, data.Value);
                    break;
                case Regions.Europe:
                    UpdateStatsView(EuropeStats, data.Value);
                    break;
                case Regions.All:
                    break;
                default:
                    break;
            }
        }

        TemperatureSlider.value = sim.currentTemp;
    }

    private void UpdateStatsView(StatsView view, RegionData data)
    {
        view.co2Bar.SetBarValue(data.share_global_cumulative_co2);
        view.moneyBar.SetBarValue(data.moneyStat);
        view.happinessBar.SetBarValue(data.happinessStat);
        view.energyBar.SetBarValue(data.energy);
    }
}
