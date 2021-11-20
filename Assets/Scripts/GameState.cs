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
    public Transform DeskViewPos;
    public Transform MapViewPos;

    private Bill_Object _selectedBill = null;

    public Camera MainCam;
    public GameObject StampObj;
    private bool _setTurned = false;
    private bool _turningSet = false;
    private float _startTurnTime;


    private bool _currentlyMoving = false;
    private Transform _startTrans;
    private Transform _goalTransform;
    private float _startTime;
    private float _travelTime = 0.5f;

    private bool _optionChosen = false;
    private bool _movingStamp =false;
    private bool _moveStampDown = true;
    private float _stampStartTime;
    private StampMode _chosenMode;


    public SFXPlaying soundManager; 

    private AudioSource _source; 

    public void PlayAudioClip(AudioClip Clip)
    {
        _source.PlayOneShot(Clip);
    }    

    public void Start()
    {
        MainCam.gameObject.transform.position = DeskViewPos.position;
        MainCam.gameObject.transform.rotation = DeskViewPos.rotation;
        _source = MainCam.gameObject.GetComponent<AudioSource>();

    }

    public void TryStamp(Bill_Object obj,  StampMode mode)
    {
        if (!_currentlyMoving && _setTurned && !_optionChosen)
        {
            _optionChosen = true;
            _movingStamp = true;
            _moveStampDown = true;
            _stampStartTime= Time.deltaTime;
            _chosenMode = mode;
            if(mode == StampMode.APPROVE)
            {
                ApproveBill();
            }
            else
            {
                DenyBill();
            }
        }
    }

    public void ApproveBill()
    {

    }

    public void DenyBill()
    {

    }

    public void TrySelectBill(Bill_Object obj)
    {
        if (_selectedBill != obj)
        {
            soundManager.PlayBookPage();
            LerpCamera(obj.CamPos);
            _selectedBill = obj;
        }
    }

    public void TrySelectMap()
    {
        LerpCamera(MapViewPos);
        _selectedBill = null;
    }

    public void TrySelectDesk()
    {
        LerpCamera(DeskViewPos);
        _selectedBill = null;
    }

    public void TryTurnSet()
    {
        if(!_turningSet)
        {
            _turningSet = true;
            _startTurnTime = Time.time;
        }
    }


    public void Update()
    {
        if (_currentlyMoving)
        {
            //time moved
            float distCovered = (Time.time - _startTime);

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / _travelTime;


            MainCam.transform.position = Vector3.Lerp(_startTrans.position, _goalTransform.position, fractionOfJourney);
            MainCam.transform.rotation = Quaternion.Lerp(_startTrans.rotation, _goalTransform.rotation, fractionOfJourney);


            if (fractionOfJourney >= 1.0f)
            {
                _currentlyMoving = false;
            }

        }

        if(_turningSet)
        {
            float distCovered = (Time.time - _startTurnTime);

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / _travelTime;


            if(!_setTurned)
            {
                StampObj.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0,90,0), Quaternion.Euler(0,0,0), fractionOfJourney);
            }
            else
            {
                StampObj.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0,0,0), Quaternion.Euler(0,90,0), fractionOfJourney);

            }


            if (fractionOfJourney >= 1.0f)
            {
                _setTurned = !_setTurned;
                _turningSet = false;

                
            }
        }

        if(_movingStamp)
        {
            float distCovered = (Time.time - _stampStartTime);

            // Fraction of journey completed equals current distance divided by total distance.

            if(!_moveStampDown)
            {
                float fractionOfJourney = distCovered / 0.1f;

                //StampObj.transform.position.y = float.Lerp(Quaternion.Euler(0,90,0), Quaternion.Euler(0,0,0), fractionOfJourney);
            }
            else
            {
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
