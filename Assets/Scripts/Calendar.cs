using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class Calendar : MonoBehaviour
{

    public TextMeshPro Month;
    public TextMeshPro Year;

    public Simulation Sim;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Sim)
        {
            Month.text = Sim.CurrentDate.ToString("MMM", CultureInfo.InvariantCulture).ToUpper();
            Year.text = Sim.CurrentDate.ToString("yyyy", CultureInfo.InvariantCulture).ToUpper();
        }
    }
}
