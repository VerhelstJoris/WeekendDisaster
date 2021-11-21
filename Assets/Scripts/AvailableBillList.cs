using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailableBillList", menuName = "ScriptableObjects/AvailableBillList", order = 1)]
public class AvailableBillList : ScriptableObject
{
    [SerializeField]
    List<Bill_Data> Bills;

    public Bill_Data SelectNextBill(Bill_Data lastBill, Simulation Sim)
    {
        if (lastBill != null)
        {
            if (lastBill.accepted && lastBill.FollowUpAccepted != null)
            {
                return lastBill.FollowUpAccepted;
            }
            if (!lastBill.accepted && lastBill.FollowUpDenied != null)
            {
                return lastBill.FollowUpDenied;
            }
        }

        System.Random random = new System.Random();
        while (true)
        {
            Bill_Data nextBill = Bills[random.Next(Bills.Count)];
            bool meetsConditions = true;
            foreach (BillCondition condi in nextBill.BillConditions)
            {
                if (!Sim.IsConditionMet(condi))
                {
                    meetsConditions = false;
                    break;
                }
            }
            if (meetsConditions)
            {
                return nextBill;
            }
        }
    }
}