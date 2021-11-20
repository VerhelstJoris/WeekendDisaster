using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StampMode
{
    APPROVE,
    DENY
}

public class Stamp : MonoBehaviour
{
    public Bill_Object obj;

    public StampMode Mode;

    public StampSet Set;

    public void DoStamp()
    {
        Debug.Log("DO");
        GameState.Instance.TryStamp(obj,Mode);
    }
}
