using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InputSelectionHelper : MonoBehaviour
{
    private GameRegion selectedRegion;
    private Camera cam;

    public GameState GameState;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            HandleTouch(Input.mousePosition, TouchPhase.Ended);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleTouch(touch.position, touch.phase);
        }
    }

    private void HandleTouch(Vector2 TouchPos, TouchPhase TouchPhase)
    {
        if (TouchPhase == TouchPhase.Ended)
        {
            Ray ray = cam.ScreenPointToRay(TouchPos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameRegion hitRegion = hitInfo.collider.GetComponent<GameRegion>();
                if (hitRegion != null)
                {
                    SelectRegion(hitRegion);
                }

                Stamp hitStamp = hitInfo.collider.GetComponent<Stamp>();
                if (hitStamp != null)
                {
                    hitStamp.DoStamp();
                }

                Bill_Object hitBill = hitInfo.collider.GetComponent<Bill_Object>();
                if (hitBill != null)
                {
                    GameState.Instance.TrySelectBill(hitBill);
                }
            }
        }
    }

    private void SelectRegion(GameRegion newRegion)
    {
        if (selectedRegion != null)
        {
            selectedRegion.Deselect();
        }
        selectedRegion = newRegion;
        newRegion.Select();
    }
}
