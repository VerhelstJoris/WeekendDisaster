using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CurrentPos
{
    Map,
    Desk,
    Bill
}

public class GameState : MonoBehaviour
{
    public Bill_Object LeftBill;
    public Bill_Object RightBill;

    public Transform DeskViewPos;
    public Transform MapViewPos;
    public Transform LeftBillViewPos;
    public Transform RightBillViewPos;

    public Camera MainCam;

    public void Start()
    {
        MainCam.gameObject.transform.position = DeskViewPos.position;
        MainCam.gameObject.transform.rotation = DeskViewPos.rotation;

    }
}
