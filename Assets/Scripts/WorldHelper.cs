using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class WorldSim : MonoBehaviour
    {
		private Simulation sim = new Simulation();		

        private GameRegion[] Regions;
        private List<Bill_Data> BillsInEffect;

        void Start()
        {
            Regions = FindObjectsOfType<GameRegion>();
        }

        void Update()
        {

        }

        void TickSim()
        {
            foreach (GameRegion region in Regions)
            {
                TickRegionSim(region);
            }
        }

        void TickRegionSim(GameRegion region)
        {

        }
    }
}