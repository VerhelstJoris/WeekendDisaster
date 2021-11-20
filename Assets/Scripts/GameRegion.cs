using System.Collections;
using UnityEngine;

public class GameRegion : MonoBehaviour
{
	[SerializeField]
	private RegionData regionData = new RegionData();
	
    private float happiness = 1.0f, money = 1.0f, energy = 1.0f;

    public float Happiness
    {
        get { return happiness; }
    }

    public float Money {
        get { return money; }
    }

    public float Energy
    {
        get { return energy; }
    }

    public void Select()
    {
        Debug.Log("Selected: " + regionData.location.ToString());
    }

    public void Deselect()
    {

    }
}