using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event EventHandler<Vector3> OnClick;

    private bool isClicking = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicking = true;
        }
        else if (Input.GetMouseButton(0))
        {
            Clicking();
        }else if (Input.GetMouseButtonUp(0))
        {
            isClicking= false;
        }
    }

    private void Clicking()
    {
        int maxRaycastDistance = 50;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, maxRaycastDistance);

        if(hit.collider == null) { return; }

        OnClick?.Invoke(this, hit.point);
    }
}
