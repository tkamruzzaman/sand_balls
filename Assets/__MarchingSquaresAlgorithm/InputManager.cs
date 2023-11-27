using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event EventHandler<Vector3> OnClick;

    private bool isClicking = false;

    private const int MAX_RAY_CAST_DISTANCE = 50;
    private Ray ray;
    private RaycastHit[] raycastHits = new RaycastHit[1];

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicking = true;
        }
        else if (Input.GetMouseButton(0))
        {
            Clicking();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isClicking = false;
        }
    }

    private void Clicking()
    {
        if (!isClicking) { return; }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.RaycastNonAlloc(ray, raycastHits, MAX_RAY_CAST_DISTANCE) == 0)
        {
            return;
        }
        OnClick?.Invoke(this, raycastHits[0].point);
    }
}
