using UnityEngine;

public class TruckCollider : MonoBehaviour
{
    private Truck m_Truck;

    private void Awake()
    {
        m_Truck = GetComponentInParent<Truck>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != PhysicsLayers.Ball) { return; }
        if (!collision.gameObject.CompareTag("Active")) { return; }

        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null) { return; }

        m_Truck.m_CollisionCount++;

        if (m_Truck.m_CollisionCount > 10)
        {
            //ball.gameObject.SetActive(false);
        }
        //ball.ballRigidbody.constraints = RigidbodyConstraints.None;
        //ball.ballRigidbody.isKinematic = true;
        //ball.ballRigidbody.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != PhysicsLayers.Ball) { return; }
        if (!other.gameObject.CompareTag("Active")) { return; }

        Ball ball = other.gameObject.GetComponent<Ball>();
        if (ball == null) { return; }

        ball.transform.SetParent(m_Truck.transform);

    }
}
