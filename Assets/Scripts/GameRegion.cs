using System.Collections;
using UnityEngine;

public class GameRegion : MonoBehaviour
{
	[SerializeField]
	private RegionData regionData = new RegionData();

    private Material regionMat;
    private const float _selectEmission = 0.2f;

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        regionMat = renderer.material;
    }

    public void Select()
    {
        Debug.Log("Selected: " + regionData.location.ToString());

        regionMat.SetFloat("SelectionEmission", _selectEmission);
    }

    public void Deselect()
    {
        regionMat.SetFloat("SelectionEmission", 0.0f);
    }
}