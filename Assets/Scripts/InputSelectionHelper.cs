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
                else
                {
                    SelectRegion(null);
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

                Desk hitDesk = hitInfo.collider.GetComponent<Desk>();
                if (hitDesk != null)
                {
                    GameState.Instance.TrySelectDesk();
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
        if (newRegion != null)
        {
            newRegion.Select();
        }
    }
}
