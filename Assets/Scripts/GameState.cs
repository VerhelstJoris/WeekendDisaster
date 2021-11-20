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

    public Bill_Object MainBill = null;

    private Bill_Object _selectedBill = null;

    public AvailableBillList AvailableBills;

    public Camera MainCam;
    public GameObject StampObj;
    private bool _setTurned = false;
    private bool _turningSet = false;
    private float _startTurnTime;


    private bool _currentlyMoving = false;
    private bool _shouldUpdateBill = false;
    private Transform _startTrans;
    private Transform _goalTransform;
    private float _startTime;
    private float _travelTime = 0.5f;

    private bool _optionChosen = false;
    private bool _movingStamp =false;
    private bool _moveStampDown = true;
    private float _stampStartTime;
    private StampMode _chosenMode;
    private GameObject _chosenStampObj =null;
    float _stampMove = 0.5f;
    Vector3 _stampStartPos;


    public SFXPlaying soundManager; 

    private AudioSource _source;

    [SerializeField]
    private Simulation sim;

    [SerializeField]
    private WorldMapStats worldStats;

    public void PlayAudioClip(AudioClip Clip)
    {
        _source.PlayOneShot(Clip);
    }    

    public void Start()
    {
        MainCam.gameObject.transform.position = DeskViewPos.position;
        MainCam.gameObject.transform.rotation = DeskViewPos.rotation;
        _source = MainCam.gameObject.GetComponent<AudioSource>();

        if (sim != null)
        {
            sim.OnStepped += UpdateWorldStats;
        }
    }

    private void UpdateWorldStats()
    {
        worldStats.UpdateStats(sim);
    }

    public void TryStamp(GameObject obj,  StampMode mode)
    {

        if(!_currentlyMoving && !_setTurned)
        {
            TryTurnSet();
        }

        if (!_currentlyMoving && _setTurned && !_optionChosen)
        {
            _optionChosen = true;
            _movingStamp = true;
            _moveStampDown = true;
            _stampStartTime= Time.time;
            _chosenMode = mode;

            _chosenStampObj = obj;
            _stampStartPos = _chosenStampObj.transform.localPosition;
            if(mode == StampMode.APPROVE)
            {
                _selectedBill.Data.accepted = true;
            }
            sim.bills.Add(_selectedBill.Data);

        }
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
        if(_selectedBill == null)
        {
            TrySelectBill(MainBill);
        }
        else
        {
            if(!_turningSet)
            {
                _turningSet = true;
                _startTurnTime = Time.time;
            }
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

        if (!_currentlyMoving && _shouldUpdateBill)
        {
            _selectedBill.UpdateBillData(AvailableBills.SelectNextBill(_selectedBill.Data, sim));
            _shouldUpdateBill = false;
        }

        if (_turningSet)
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

            if(_moveStampDown)
            {
                float distCovered = (Time.time - _stampStartTime);

                float time = 0.25f;

                float fractionOfJourney = distCovered / time;

                _chosenStampObj.transform.localPosition =new Vector3(  _chosenStampObj.transform.localPosition.x,
                Mathf.Lerp(_stampStartPos.y,_stampStartPos.y - _stampMove, fractionOfJourney),
                 _chosenStampObj.transform.localPosition.z);


                 if(fractionOfJourney >= 1.0f)
                 {
                    _stampStartPos = _chosenStampObj.transform.localPosition;
                    _moveStampDown = false;

                    _stampStartTime = Time.time;

                    Debug.Log("FINISHED MOVING DOWN");
                    //AUDIO HERE
                    soundManager.PlayStamp();
                 }
            }
            else
            {
                float distCovered = (Time.time - _stampStartTime);


                float time = 0.25f;

                float fractionOfJourney = distCovered / time;


                 _chosenStampObj.transform.localPosition =new Vector3(  _chosenStampObj.transform.localPosition.x,
                Mathf.Lerp(_stampStartPos.y,_stampStartPos.y + _stampMove, fractionOfJourney),
                 _chosenStampObj.transform.localPosition.z);


                 if(fractionOfJourney >= 1.0f)
                 {
                     _moveStampDown = true;
                     _movingStamp = false;

                    LerpCamera(MapViewPos);
                    TryTurnSet();
                    _optionChosen = false;
                 }

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
