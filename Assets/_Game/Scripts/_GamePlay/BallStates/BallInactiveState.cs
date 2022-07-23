using UnityEngine;

public class BallInactiveState : BallBaseState
{
    public override void EnterState(Balls ball)
    {
        ball.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    public override void OnCollisionEnter(Balls ball, Collision collision)
    {
        if (collision.collider.CompareTag("Active"))
        {
            ball.TransitionToState(ball.activeState);
        }
    }

}
