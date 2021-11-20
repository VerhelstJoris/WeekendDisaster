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

    private static GameState _instance;

    public static GameState Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Bill_Object LeftBill;
    public Bill_Object RightBill;

    public Transform DeskViewPos;
    public Transform MapViewPos;

    private Bill_Object _selectedBill = null;

    public Camera MainCam;


    private bool _currentlyMoving = false;
    private Transform _startTrans;
    private Transform _goalTransform;
    private float _startTime;
    private float _travelTime = 0.5f;

    public void Start()
    {
        MainCam.gameObject.transform.position = DeskViewPos.position;
        MainCam.gameObject.transform.rotation = DeskViewPos.rotation;

    }

    public void TryStamp(Bill_Object obj)
    {
        if (_selectedBill == obj && !_currentlyMoving)
        {
            Debug.LogError("STAMP STAMP");
        }
    }

    public void TrySelectBill(Bill_Object obj)
    {
        if (_selectedBill != obj)
        {
            LerpCamera(obj.CamPos);
        }
    }

    public void TrySelectMap()
    {

    }

    public void TrySelectDesk()
    {
        LerpCamera(DeskViewPos);
    }

    public void Update()
    {
        if (_currentlyMoving)
        {
            //time moved
            float distCovered = (Time.time - _startTime);

            Debug.Log("DistCovered: " + distCovered);

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / _travelTime;
            Debug.Log("Fraction: " + fractionOfJourney);


            MainCam.transform.position = Vector3.Lerp(_startTrans.position, _goalTransform.position, fractionOfJourney);
            MainCam.transform.rotation = Quaternion.Lerp(_startTrans.rotation, _goalTransform.rotation, fractionOfJourney);


            if (fractionOfJourney >= 1.0f)
            {
                _currentlyMoving = false;
            }

        }
    }

    private void LerpCamera(Transform goal)
    {
        if (goal == null)
        {
            Debug.LogError("No goal set");
            return;
        }

        if (!_currentlyMoving)
        {
            _startTrans = MainCam.gameObject.transform;
            _goalTransform = goal;
            _currentlyMoving = true;
            _startTime = Time.time;
        }

    }

}
