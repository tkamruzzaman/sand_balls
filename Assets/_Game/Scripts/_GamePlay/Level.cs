﻿using UnityEngine;
public class Level : MonoBehaviour
{
    [SerializeField] private Ball[] startBalls;
    [SerializeField] private Transform[] holesTomake;
    [SerializeField] [Range(0.5f, 3.0f)] private float radius = 2.0f;

    [SerializeField] private PlaneDeformer[] sandPlanes;
    [SerializeField] private Vector3[] planeCenters;

    private int m_TotalBalls;

    public int TotalBalls { get => m_TotalBalls; private set => m_TotalBalls = value; }

    [SerializeField] private Collider m_RightBarrierCollider;

    private void Awake()
    {
        sandPlanes = GetComponentsInChildren<PlaneDeformer>();
        planeCenters = new Vector3[sandPlanes.Length];
        for (int i = 0; i < planeCenters.Length; i++)
        {
            planeCenters[i] = sandPlanes[i].gameObject.transform.GetComponent<Renderer>().bounds.center;
        }

        m_TotalBalls = GetComponentsInChildren<Ball>().Length;
    }

    private void Start()
    {
        foreach (Ball ball in startBalls)
        {
            ball.TransitionToState(ball.activeState);
        }

        foreach (Transform tf in holesTomake)
        {
            CreateHoleInMesh(tf.position, radius, /*planeDistance*/ 45);
        }

        Service.GameEvents.OnLevelSpawned?.Invoke();
    }

    public void DoformMesh(RaycastHit hit, float planeDistance)
    {
        // check the distance of all hit distances
        for (int i = 0; i < planeCenters.Length; i++)
        {
            if ((planeCenters[i] - hit.point).sqrMagnitude < planeDistance)
            {
                // deform this planes area;
                sandPlanes[i].DeformMesh(hit.point);
            }
        }
        if (hit.transform.CompareTag("Ring"))
        {
            Destroy(hit.transform.gameObject);
        }
    }

    private void CreateHoleInMesh(Vector3 position, float radius, float planeDistance)
    {
        for (int i = 0; i < planeCenters.Length; i++)
        {
            if ((planeCenters[i] - position).sqrMagnitude < planeDistance)
            {
                // deform this planes area;
                sandPlanes[i].CreateHole(position, radius);
            }
        }
    }

    private void OnEnable() => Service.GameEvents.OnPressedResultNextButton += DisableBarrier;

    private void OnDisable() => Service.GameEvents.OnPressedResultNextButton -= DisableBarrier;

    private void DisableBarrier() => m_RightBarrierCollider.enabled = false;

}