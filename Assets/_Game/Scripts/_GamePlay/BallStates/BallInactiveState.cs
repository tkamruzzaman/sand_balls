using UnityEngine;

public class BallInactiveState : BallBaseState
{
    public override void EnterState(Ball ball)
    {
        ball.ballRenderer.material.SetColor("_Color", Color.white);

        ball.ballRigidbody.useGravity = false;
        ball.ballRigidbody.isKinematic = true;
    }

    public override void OnCollisionEnter(Ball ball, Collision collision)
    {
        if (collision.collider.CompareTag("Active"))
        {
            ball.TransitionToState(ball.activeState);
        }
    }
}
