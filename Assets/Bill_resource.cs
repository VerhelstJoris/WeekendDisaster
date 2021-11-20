using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill_resource : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer _image;

    [SerializeField]
    private TextMesh _text;

    public void UpdateValue(int val)
    {
        if(val==0)
        {
            this.gameObject.active = false;
        }
        else
        {
        if(_image != null && _text != null)
        {
            _text.text = "x " + val;
        }
        }
    } 
}
