using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill_resource : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer _image;

    [SerializeField]
    private TextMesh _text;

    public void UpdateValue(float val, ref float pos)
    {
        if(val==0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
        if(_image != null && _text != null)
        {
            _text.text = "x " + val;
            this.gameObject.transform.localPosition = 
            new Vector3(this.gameObject.transform.localPosition.x, 
            pos, this.gameObject.transform.localPosition.z);
            pos -= 0.75f;
        }
        }
    } 
}
