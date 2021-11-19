using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill_Object : MonoBehaviour
{
    public Bill_Data Data;


    public TextMesh Title;
    public TextMesh Description;
    public TextMesh Countries;
    public TextMesh Effects; 


    void Start()
    {
        Title.text = Data.BillName;
        Description.text = Data.BillText;
    }
}
