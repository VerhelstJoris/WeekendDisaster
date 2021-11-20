using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    public Bill_Object obj;

    public void DoStamp()
    {
        GameState.Instance.TryStamp(obj);
    }
}
