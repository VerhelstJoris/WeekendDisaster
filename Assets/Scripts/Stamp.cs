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
    public StampMode Mode;

    public StampSet Set;

    public GameObject stampObj;

    public void DoStamp()
    {
        GameState.Instance.TryStamp(stampObj,Mode);
    }
}
