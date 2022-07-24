using UnityEngine;

public class InputController : MonoBehaviour
{
    // public references
    public GameObject cylinderPrefab;
    public float planeDistance;
    public GameObject board;


    // private references
    private PlaneDeformer[] sandPlanes;
    private Vector3[] planeCenters;
    private Ray ray;
    private RaycastHit hit;
    [SerializeField] private Camera cam;
    private bool startTouching;

    private void Awake()
    {
        //cam = this.transform.GetComponent<Camera>();
        sandPlanes = board.GetComponentsInChildren<PlaneDeformer>();
        planeCenters = new Vector3[sandPlanes.Length];
        for (int i = 0; i < planeCenters.Length; i++)
        {
            planeCenters[i] = sandPlanes[i].gameObject.transform.GetComponent<Renderer>().bounds.center;
        }
        startTouching = false;


        startTouching = true;
    }

    //private void Update()
    //{
    //    startTouching = !StartScreen.activeSelf;
    //}

    private void FixedUpdate()
    {
        if (startTouching && Input.GetMouseButton(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            DoformMesh();
        }

        //     if (startTouching && Input.touchCount > 0)
        //     {
        //         foreach (Touch touch in Input.touches)
        //         {
        //             ray = cam.ScreenPointToRay(touch.position);
        //             doformMesh();
        //         }
        //     }
    }


    private void DoformMesh()
    {
        if (Physics.Raycast(ray, out hit))
        {
            // sandPlanes.deformThePlane(hit.point);
            // check the distance of all hit distances
            for (int i = 0; i < planeCenters.Length; i++)
            {
                if ((planeCenters[i] - hit.point).sqrMagnitude < planeDistance)
                {
                    // deform this planes area;
                    sandPlanes[i].DeformThePlane(hit.point);
                }
            }
            if (hit.transform.CompareTag("Ring"))
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    public void DefromToHoles(Vector3 position, float radius)
    {
        for (int i = 0; i < planeCenters.Length; i++)
        {
            if ((planeCenters[i] - position).sqrMagnitude < planeDistance)
            {
                // deform this planes area;
                sandPlanes[i].Puthole(position, radius);
            }
        }
    }
}
