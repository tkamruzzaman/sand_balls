using UnityEngine;

public abstract class BallBaseState
{
    public abstract void EnterState(Ball ball);

    public abstract void OnCollisionEnter(Ball ball, Collision collision);
}
